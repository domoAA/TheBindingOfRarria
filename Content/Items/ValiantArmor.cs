using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items
{
    public class ValiantArmor : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.accessory = true;
            Item.defense = 5;
            Item.value = Item.buyPrice(0, 9);
            Item.rare = ItemRarityID.LightPurple;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statDefense *= 1.05f;
            player.statLifeMax2 += player.statLifeMax2 * 15 / 100;
            player.aggro += 500;
        }
        public override void AddRecipes()
        {
            Recipe.Create(Item.type)
                .AddIngredient(ItemID.ShieldStatue)
                .AddIngredient(ItemID.HeroShield)
                .AddIngredient(ItemID.AegisFruit)
                .AddIngredient(ItemID.LifeforcePotion)
                .AddIngredient(ItemID.SoulofMight, 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();

            Recipe.Create(Item.type)
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