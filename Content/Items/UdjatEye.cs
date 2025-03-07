
namespace TheBindingOfRarria.Content.Items
{
    public class UdjatEye : ModItem
    {
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
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.FossilOre, 40)
                .AddIngredient(ItemID.SpelunkerPotion, 8)
                .AddTile(TileID.Solidifier)
                .Register();

            base.AddRecipes();
        }
    }
    public class UdjatExtractinatorDrop : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override void ExtractinatorUse(int extractType, int extractinatorBlockType, ref int resultType, ref int resultStack)
        {
            if (extractType == ItemID.DesertFossil && Main.rand.NextFloat() < 0.005) 
            {
                resultStack = 1;
                resultType = ModContent.ItemType<UdjatEye>();
            }
            else
                base.ExtractinatorUse(extractType, extractinatorBlockType, ref resultType, ref resultStack);
        }
    }
}