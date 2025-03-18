
namespace TheBindingOfRarria.Content.Items
{
    public class Censer : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 34;
            Item.accessory = true;
            Item.value = Item.buyPrice(0, 9);
            Item.rare = ItemRarityID.LightPurple;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (Main.myPlayer == player.whoAmI)
                player.SpawnProjectileIfNotSpawned(ModContent.ProjectileType<SlowingAura>(), player.Center, player.GetSource_Accessory(Item));
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.FastClock)
                .AddIngredient(ItemID.PutridScent)
                .AddIngredient(ItemID.SoulofLight, 10)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();

            base.AddRecipes();
        }
    }
}