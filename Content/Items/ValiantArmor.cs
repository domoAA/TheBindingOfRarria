
namespace TheBindingOfRarria.Content.Items
{
    public class ValiantArmor : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.accessory = true;
            Item.value = Item.buyPrice(0, 9);
            Item.rare = ItemRarityID.LightPurple;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statLifeMax2 += 50;
            player.aggro += 500;
            player.endurance += 0.1f;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.ShieldStatue)
                .AddIngredient(ItemID.HeroShield)
                .AddIngredient(ItemID.AegisFruit)
                .AddIngredient(ItemID.LifeforcePotion)
                .AddIngredient(ItemID.SoulofMight, 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.ShieldStatue)
                .AddIngredient(ItemID.HeroShield)
                .AddIngredient(ItemID.AegisFruit)
                .AddIngredient(ItemID.LifeCrystal, 3)
                .AddIngredient(ItemID.SoulofMight, 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();

            base.AddRecipes();
        }
    }
}