using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheBindingOfRarria
{
	// Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
	public class TheBindingOfRarria : Mod
	{
        private const string PoisonTexturePath = "TheBindingOfRarria/Content/Buffs/PoisonSign";
        public static Asset<Texture2D> PoisonTexture;
        public override void Load()
        {
            PoisonTexture = ModContent.Request<Texture2D>(PoisonTexturePath);
        }
    }
}
