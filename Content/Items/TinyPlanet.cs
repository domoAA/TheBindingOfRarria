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
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PlanetPlayer>().planet = true;
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
                Orbiting = Main.netMode == NetmodeID.SinglePlayer ? true : null;

                if (Main.myPlayer == projectile.owner)
                {
                    IndividualOffset = (Main.rand.NextFloat() - 0.5f) * 128;
                    projectile.netUpdate = true;
                }
            }
            base.OnSpawn(projectile, source);
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
        }
        public override void SendExtraAI(Projectile projectile, BitWriter bitWriter, BinaryWriter binaryWriter)
        {
            base.SendExtraAI(projectile, bitWriter, binaryWriter);
            if (Orbiting == null) 
            {
                if (Main.myPlayer == projectile.owner && Main.netMode == NetmodeID.MultiplayerClient)
                {
                    binaryWriter.Write((int)TheBindingOfRarria.PacketTypes.OrbitInfo);
                    binaryWriter.Write(IndividualOffset);
                    Orbiting = true;
                }
                else if (Main.netMode == NetmodeID.Server)
                {
                    binaryWriter.Write((int)TheBindingOfRarria.PacketTypes.OrbitInfo);
                    binaryWriter.Write(IndividualOffset);
                    Orbiting = true;
                }
            } 
        }
        public override void ReceiveExtraAI(Projectile projectile, BitReader bitReader, BinaryReader binaryReader)
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