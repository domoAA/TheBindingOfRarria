using System;
using Terraria;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items
{
    public class PioneersShield : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 30;
            Item.accessory = true;
            Item.defense = 3;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            Item.defense = 3;
        }
    }
    public class PioneerKnightPlayer : ModPlayer
    {
        public int shields = 0;
        public override void PostUpdateEquips()
        {
            shields = 0;
            foreach (var item in Player.armor)
            {
                if (!item.social && item.shieldSlot != -1)
                    shields++;
            }
            var shield = Array.Find(Player.armor, item => item.type == ModContent.ItemType<PioneersShield>() && !item.social);
            if (shield == null)
                return;

            shield.defense += shields;
        }
    }
}