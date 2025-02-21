using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace TheBindingOfRarria.Content.Items
{

    // 20/02/2024: Added and removed experimental recipe.
    public class TinyPlanet : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 28;
            Item.height = 28;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.buyPrice(0, 0, 77, 49);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PlanetPlayer>().planet = true;
        }
        public override void AddRecipes()
        {
            Recipe.Create(Item.type)
                .AddIngredient(ItemID.Meteorite, 100)
                .AddIngredient(ItemID.Hellstone, 100)
                .AddIngredient(ItemID.IceBlock, 200)
                .AddTile(TileID.SkyMill)
                .Register();

            base.AddRecipes();
        }
    }
    public class PlanetPlayer : ModPlayer
    {
        public bool planet = false;
        public override void ResetEffects()
        {
            planet = false;
        }
    }
    public class OrbitingGlobalProjectile : GlobalProjectile
    {
        // null state will indicate that it needs to be sent across the network
        public bool? Orbiting = false;
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
        {
            return base.AppliesToEntity(entity, lateInstantiation);
        }

        public class PlanetPlayer : ModPlayer
        {
            if (!projectile.CanBeReflected())
                base.OnSpawn(projectile, source);
            else if (Main.player[projectile.owner].GetModPlayer<PlanetPlayer>().planet && Main.netMode != NetmodeID.Server)
            {
                planet = false;
            }
    }
    public class OrbitingGlobalProjectile : GlobalProjectile
    {
        // null state will indicate that it needs to be sent across the network
        public bool? Orbiting = false;
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
        {
            return entity.CanBeReflected();
        }

        // individual direction
        public float rotation = Main.rand.NextBool() ? MathHelper.PiOver2 : -MathHelper.PiOver2;

        // tracking speed
        public float speed = 0.9f;
        public float IndividualOffset = 0;
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (Main.player[projectile.owner].GetModPlayer<PlanetPlayer>().planet && Main.netMode != NetmodeID.Server)
            {
                projectile.timeLeft = 300;
                projectile.damage = (int)(0.85f * projectile.damage);
                IndividualOffset = (Main.rand.NextFloat() - 0.5f) * 128;
                projectile.netUpdate = true;
            }
        }
        }
        public override void PostAI(Projectile projectile)
        {
            base.PostAI(projectile);
            if (Orbiting == false)
                return;

            projectile.tileCollide = false;

            var owner = Main.player[projectile.owner];

            var r = projectile.Center.Distance(owner.Center);

            var velocity = projectile.velocity.RotatedBy(rotation);
            velocity.Normalize();

            if (r < 32)
                return;

            projectile.velocity += velocity * projectile.velocity.LengthSquared() / r;

            var gravity = 0.01f * projectile.Center.DirectionTo(owner.Center) * r;


            var offset = (projectile.Center.DirectionTo(owner.Center).ToRotation() + rotation) - projectile.velocity.ToRotation();

            projectile.velocity = projectile.velocity.RotatedBy(offset);


            // making sure it stays at a certain distance
            if (r > 270 + IndividualOffset)
                projectile.velocity += gravity;

            else if (r < 250 + IndividualOffset)
                projectile.velocity -= gravity;

            else
                projectile.velocity *= 1.03f;

            // acceleration
            speed += speed < 0.98f ? 0.005f : 0;

            projectile.velocity *= speed;
            projectile.netUpdate = true;
        }
        public override void SendExtraAI(Projectile projectile, BitWriter bitWriter, BinaryWriter binaryWriter)
        {
            base.SendExtraAI(projectile, bitWriter, binaryWriter);
            if (Orbiting == null)
            {
                base.ReceiveExtraAI(projectile, bitReader, binaryReader);
                int type = binaryReader.Read();

                if (type == (int)TheBindingOfRarria.PacketTypes.OrbitInfo)
                {
                    float offset = binaryReader.ReadSingle();

                    if (Main.netMode == NetmodeID.Server)
                    {
                        IndividualOffset = offset;
                        Orbiting = null;
                        projectile.netUpdate = true;
                    }
                    else
                    {
                        IndividualOffset = offset;
                        Orbiting = true;
                    }
                }
            }
        }
    }
}