
namespace TheBindingOfRarria.Content.Items
{
    public class NightLight : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 18;
            Item.height = 34;
            Item.value = Item.buyPrice(0, 8);
            Item.rare = ItemRarityID.Pink;
        }
        public override void UpdateAccessory(Player player, bool hideVisual) => player.SpawnProjectileIfNotSpawned(ModContent.ProjectileType<LightCone>(), player.GetSource_Accessory(Item));
        
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.FastClock)
                .AddIngredient(ItemID.SoulofLight, 10)
                .AddIngredient(ItemID.Glass, 20)
                .AddIngredient(ItemID.UltrabrightHelmet)
                .AddTile(TileID.MythrilAnvil)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.FastClock)
                .AddIngredient(ItemID.SoulofLight, 10)
                .AddIngredient(ItemID.Glass, 20)
                .AddIngredient(ItemID.UltrabrightTorch, 20)
                .AddTile(TileID.MythrilAnvil)
                .Register();

            base.AddRecipes();
        }
    }
}