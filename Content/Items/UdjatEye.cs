using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items
{
    public class UdjatEye : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemTrader.ChlorophyteExtractinator.AddOption_FromAny(ModContent.ItemType<UdjatEye>(), ItemID.DesertFossil);
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.height = 20;
            Item.width = 32;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.buyPrice(0, 1, 12);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.dangerSense = true;
            player.findTreasure = true;
        }
    }
}