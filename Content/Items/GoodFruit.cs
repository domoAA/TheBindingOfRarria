using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items
{
    public class GoodFruit : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.height = 24;
            Item.width = 28;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.buyPrice(0, 0, 90);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.AddBuff(BuffID.WellFed2, 2);
            player.AddBuff(BuffID.Sunflower, 2);
        }
        public override void AddRecipes()
        {
            Recipe.Create(Item.type)
                .AddIngredient(ItemID.Grapefruit)
                .AddIngredient(ItemID.Pineapple)
                .AddTile(TileID.CookingPots)
                .Register();

            base.AddRecipes();
        }
    }
}