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

namespace TheBindingOfRarria
{
    // Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
    public class TheBindingOfRarria : Mod
    {
        public static Asset<Texture2D> PoisonTexture;
        public static Asset<Texture2D> FireTexture;
        public static Asset<Texture2D> Circle;
        public static Asset<Texture2D> CenserExtra;
        public override void Load()
        {
            PoisonTexture = ModContent.Request<Texture2D>("TheBindingOfRarria/Content/Buffs/PoisonSign");
            FireTexture = ModContent.Request<Texture2D>("TheBindingOfRarria/Content/Buffs/FireSign");
            Circle = ModContent.Request<Texture2D>("TheBindingOfRarria/Content/Projectiles/CircleOfLight");
            CenserExtra = ModContent.Request<Texture2D>("TheBindingOfRarria/Content/Items/CenserExtra");
        }
    }
}
