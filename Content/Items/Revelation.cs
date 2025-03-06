using Terraria;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items
{
    public class Revelation : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 28;
            Item.height = 28;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            
        }
    }
}