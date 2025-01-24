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
        public static int[] blockItems = [ModContent.ItemType<GodHead>(), ModContent.ItemType<AnemoiBracelet>(), ModContent.ItemType<AfterimageMirror>()];
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
    }
}
