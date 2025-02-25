using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using System;
using Humanizer;
using System.Security.Cryptography.X509Certificates;
// using TheBindingOfRarria.Content.Projectiles;

// Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.

namespace BindingTest.Content.Accessories
{
    public class DeepFocus : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.height = 30;
            Item.width = 30;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<FocusedPlayer>().IsFocused = true;

            // Manualy multiplied the ticks by 1.2; Is there a better way?
            player.potionDelayTime = 4320;
            player.mushroomDelayTime = 2160;
            player.restorationDelayTime = 3240;
        }
    }

    public class FocusedPlayer : ModPlayer
    {
        public bool IsFocused;
        public override void ResetEffects()
        {
            IsFocused = false;
        }

        public override void GetHealLife(Item item, bool quickHeal, ref int healValue)
        {
            if (IsFocused)
            {
                healValue *= 2;
            }
            base.GetHealLife(item, quickHeal, ref healValue);
        }
    }


}