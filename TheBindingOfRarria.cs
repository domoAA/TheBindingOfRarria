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

namespace TheBindingOfRarria
{
    // Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
    public class TheBindingOfRarria : Mod
    {
        public static Asset<Texture2D> Circle;
        public static Asset<Texture2D> ChargeIndicatorBar;
        public static Asset<Texture2D> ChargeIndicatorCircleExtra;
        public static Asset<Texture2D> BeamEnd;
        public static Asset<Texture2D> BeamBody;
        public static Asset<Texture2D> CenserExtra;
        public static Asset<Texture2D> MirrorCrack;
        public static int[] reflectItems = [ModContent.ItemType<GodHead>(), ModContent.ItemType<AfterimageMirror>(), ModContent.ItemType<SlippedRib>()];
        public static int[] dodgeItems = [ModContent.ItemType<AnemoiBracelet>(), ItemID.BrainOfConfusion, ItemID.MasterNinjaGear, ItemID.BlackBelt];
        public static int[] invulItems = [ModContent.ItemType<ToothAndNail>(), ModContent.ItemType<GnawedLeaf>()];
        public override void Load()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                Circle = ModContent.Request<Texture2D>("TheBindingOfRarria/Content/Projectiles/CircleOfLight");
                ChargeIndicatorBar = ModContent.Request<Texture2D>("TheBindingOfRarria/Common/Assets/ChargeIndicatorBar");
                ChargeIndicatorCircleExtra = ModContent.Request<Texture2D>("TheBindingOfRarria/Common/Assets/ChargeIndicatorCircleExtra");
                BeamEnd = ModContent.Request<Texture2D>("TheBindingOfRarria/Common/Assets/BeamEnd");
                BeamBody = ModContent.Request<Texture2D>("TheBindingOfRarria/Common/Assets/BeamBody");
                CenserExtra = ModContent.Request<Texture2D>("TheBindingOfRarria/Common/Assets/CenserExtra");
                MirrorCrack = ModContent.Request<Texture2D>("TheBindingOfRarria/Common/Assets/MirrorCrack");

            }
        }
        public override void Unload()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                Circle = null;
                ChargeIndicatorBar = null;
                ChargeIndicatorCircleExtra = null;
                BeamEnd = null;
                CenserExtra = null;
                MirrorCrack = null;

            }
        }
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            bool reflected;
            int id;
            bool friendly;
            if (Main.netMode != NetmodeID.Server && whoAmI == 255)
            {
                if (reader.ReadString() == "ProjectileReflection")
                {
                    reflected = reader.ReadBoolean();
                    id = reader.ReadInt32();
                    friendly = reader.ReadBoolean();
                    foreach ( var proj in Main.ActiveProjectiles)
                    {
                        if (proj.identity == id)
                        {
                            proj.GetGlobalProjectile<GlobalProjectileReflectionBlacklist>().Reflected = true;
                            if (reflected)
                                proj.GetReflected(friendly);
                        }
                    }
                    return;
                }
            }

            if (reader.ReadString() == "ProjectileReflection" && Main.netMode == NetmodeID.Server) {
                reflected = reader.ReadBoolean();
                id = reader.ReadInt32();
                friendly = reader.ReadBoolean(); }
            else
                return;

            ModPacket packet = GetPacket();
            packet.Write("ProjectileReflection");
            packet.Write(reflected);
            packet.Write(id);
            packet.Write(friendly);
            packet.Send();
        }
    }
}
