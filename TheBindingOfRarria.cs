using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections;
using TheBindingOfRarria.Content;
using TheBindingOfRarria.Content.Items;
using Terraria.Graphics.Shaders;
using System.IO;
using TheBindingOfRarria.Content.Projectiles;
using System.Diagnostics;

namespace TheBindingOfRarria
{
    // Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
    public class TheBindingOfRarria : Mod
    {
        //public static int[] reflectItems = [ModContent.ItemType<GodHead>(), ModContent.ItemType<AfterimageMirror>(), ModContent.ItemType<SlippedRib>()];
        //public static int[] dodgeItems = [ModContent.ItemType<AnemoiBracelet>(), ModContent.ItemType<CarefreeMelody>(), ItemID.BrainOfConfusion, ItemID.MasterNinjaGear, ItemID.BlackBelt];
        //public static int[] invulItems = [ModContent.ItemType<ToothAndNail>(), ModContent.ItemType<GnawedLeaf>()];
        public override void Load()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                
            }
        }
        public override void Unload()
        {
            if (Main.netMode != NetmodeID.Server)
            {

            }
        }
        public enum PacketTypes
        {
            ProjectileReflection,
            EntitySlow,
            DustSpawn,
            Default
        }
        public enum State
        {
            Default,
            Slow,
            Fast
        }
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            var type = reader.ReadInt32();

            if (type == ((int)PacketTypes.ProjectileReflection))
            {


                var reflected = reader.ReadBoolean();
                var id = reader.ReadInt32();
                var friendly = reader.ReadBoolean();

                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    foreach (var proj in Main.ActiveProjectiles)
                    {
                        if (proj.identity == id)
                        {
                            proj.reflected = true;
                        }
                    }
                    return;
                }
                else
                {
                    ModPacket packet = GetPacket();
                    packet.Write(type);
                    packet.Write(reflected);
                    packet.Write(id);
                    packet.Write(friendly);
                    packet.Send();
                    foreach (var proj in Main.ActiveProjectiles)
                    {
                        if (proj.identity == id)
                        {
                            if (reflected && !proj.reflected)
                                proj.GetReflected(friendly);

                            proj.reflected = true;
                        }
                    }
                    return;
                }
            }
            else if (type == ((int)PacketTypes.EntitySlow))
            {
                var slow = reader.ReadInt32();
                var duration = reader.ReadInt32();
                var entityType = reader.ReadBoolean();
                var id = reader.ReadInt32();

                if (Main.netMode == NetmodeID.Server)
                {
                    ModPacket packet = GetPacket();
                    packet.Write(type);
                    packet.Write(slow);
                    packet.Write(duration);
                    packet.Write(entityType);
                    packet.Write(id);
                    packet.Send();
                }

                if (entityType)
                {
                    foreach (var proj in Main.ActiveProjectiles)
                    {
                        if (proj.identity == id)
                        {
                            proj.GetGlobalProjectile<SlowedGlobalProjectile>().Slowed = ((State)slow, duration);
                        }
                    }
                }
                else
                {
                    foreach (var npc in Main.ActiveNPCs)
                    {
                        if (npc.whoAmI == id)
                        {
                            npc.GetGlobalNPC<NPCExtensions.SlowedGlobalNPC>().Slowed = ((State)slow, duration);
                        }
                    }
                }
                return;
            }
            else if (type == ((int)PacketTypes.DustSpawn))
            {
                var position = reader.ReadVector2();
                var direction = reader.ReadVector2();
                if (Main.netMode == NetmodeID.Server)
                {
                    ModPacket packet = GetPacket();
                    packet.Write(type);
                    packet.WriteVector2(position);
                    packet.WriteVector2(direction);
                    packet.Send();
                }
                else
                {
                    Main.LocalPlayer.GetModPlayer<NatureDodgePlayer>().blocked = true;
                    Main.LocalPlayer.GetModPlayer<NatureDodgePlayer>().position = position;
                    Main.LocalPlayer.GetModPlayer<NatureDodgePlayer>().direction = direction;
                }
                return;
            }
            
            base.HandlePacket(reader, whoAmI);
            return;
        }
    }
}
