using System;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;

namespace TheBindingOfRarria.Content.Items
{
    public class TheLastFinger : ModItem
    {
        public override void SetDefaults()
        {
            Item.height = 30;
            Item.width = 30;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statManaMax2 += 100;
            player.aggro -= 400;
        }
    }
}


