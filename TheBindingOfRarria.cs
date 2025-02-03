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
        public static Asset<Texture2D> Circle;
        public static Asset<Texture2D> ChargeIndicatorBar;
        public static Asset<Texture2D> ChargeIndicatorCircleExtra;
        public static Asset<Texture2D> BeamEnd;
        public static Asset<Texture2D> BeamBody;
        public static Asset<Texture2D> CenserExtra;
        public static RenderTarget2D genderTarget;
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

                Main.RunOnMainThread(() =>
                {
                    genderTarget = new RenderTarget2D(Main.instance.GraphicsDevice, Main.screenWidth / 2, Main.screenHeight / 2);
                }).Wait();
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
                genderTarget = null;
            }
        }
        public enum PacketTypes
        {
            ProjectileReflection,
            Default
        }
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            var name = reader.ReadInt32();

            if (name != ((int)PacketTypes.ProjectileReflection))
            {
                base.HandlePacket(reader, whoAmI);
                return;
            }


            var reflected = reader.ReadBoolean();
            var id = reader.ReadInt32();
            var friendly = reader.ReadBoolean();

            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                foreach (var proj in Main.ActiveProjectiles)
                {
                    if (proj.identity == id)
                    {
                        proj.GetGlobalProjectile<GlobalProjectileReflectionBlacklist>().Reflected = true;
                    }
                }
                return;
            }
            else
            {
                ModPacket packet = GetPacket();
                packet.Write(name);
                packet.Write(reflected);
                packet.Write(id);
                packet.Write(friendly);
                packet.Send();
                foreach (var proj in Main.ActiveProjectiles)
                {
                    if (proj.identity == id)
                    {
                        if (reflected && !proj.GetGlobalProjectile<GlobalProjectileReflectionBlacklist>().Reflected)
                            proj.GetReflected(friendly);

                        proj.GetGlobalProjectile<GlobalProjectileReflectionBlacklist>().Reflected = true;
                    }
                }
            }
        }
    }
}
