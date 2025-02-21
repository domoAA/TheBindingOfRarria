using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace TheBindingOfRarria.Content.Items
{
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
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
        {
            return base.AppliesToEntity(entity, lateInstantiation);
        }
        public enum State
        {
            Default,
            Orbiting,
            No
        }
        public State state = State.Default;

        // individual direction
        public int rotation = 1;

        // tracking speed
        public float speed = 0.9f;
        public float IndividualOffset = 0;
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (!projectile.CanBeReflected())
                base.OnSpawn(projectile, source);

            if (Main.myPlayer == projectile.owner)
            {
                if (Main.player[projectile.owner].GetModPlayer<PlanetPlayer>().planet)
                {
                    projectile.tileCollide = false;
                    projectile.timeLeft = 300;

                    projectile.damage = (int)(0.85f * projectile.damage);
                    IndividualOffset = (Main.rand.NextFloat() - 0.5f);
                    rotation = Main.rand.NextBool() ? 1 : -1;

                    ModPacket packet = ModContent.GetInstance<TheBindingOfRarria>().GetPacket();
                    packet.Write((int)TheBindingOfRarria.PacketTypes.OrbitInfo);
                    packet.Write((int)State.Orbiting);
                    packet.Write(projectile.identity);
                    packet.Write(IndividualOffset);
                    packet.Write(rotation);
                    packet.Send();
                }
                else
                {
                    ModPacket packet = ModContent.GetInstance<TheBindingOfRarria>().GetPacket();
                    packet.Write((int)TheBindingOfRarria.PacketTypes.OrbitInfo);
                    packet.Write((int)State.No);
                    packet.Write(projectile.identity);
                    packet.Send();
                }
            }
        }
        public override void PostAI(Projectile projectile)
        {
            if (state == State.Orbiting)
            {
                projectile.tileCollide = false;

                var owner = Main.player[projectile.owner];

                var r = projectile.Center.Distance(owner.Center);

                var velocity = projectile.velocity.RotatedBy(rotation);
                velocity.Normalize();

                if (r < 32)
                    return;

                projectile.velocity += velocity * projectile.velocity.LengthSquared() / r;

                var gravity = 0.01f * projectile.Center.DirectionTo(owner.Center) * r;


                var offset = (projectile.Center.DirectionTo(owner.Center).ToRotation() + rotation * MathHelper.PiOver2) - projectile.velocity.ToRotation();

                projectile.velocity = projectile.velocity.RotatedBy(offset);


                // making sure it stays at a certain distance
                if (r > 270 + IndividualOffset * 128)
                    projectile.velocity += gravity;

                else if (r < 250 + IndividualOffset * 128)
                    projectile.velocity -= gravity;

                else
                    projectile.velocity *= 1.03f;

                // acceleration
                speed += speed < 0.98f ? 0.005f : 0;

                projectile.velocity *= speed;
            }
        }
    }
}