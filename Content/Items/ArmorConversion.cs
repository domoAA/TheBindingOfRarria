using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items
{
    public class ArmorConversion : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 32;
            Item.accessory = true;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.buyPrice(0, 10);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statLifeMax2 += 5 * player.statDefense;
            player.statDefense -= player.statDefense / 2;
            player.lifeRegenCount += player.statLifeMax2 / 100;
        }
        public override void AddRecipes()
        {
            Recipe.Create(Item.type)
                .AddIngredient(ItemID.PaladinsShield)
                .AddIngredient(ItemID.AegisCrystal)
                .AddIngredient(ItemID.LifeCrystal, 2)
                .AddIngredient(ItemID.LifeforcePotion)
                .AddIngredient(ItemID.RegenerationPotion, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();

            base.AddRecipes();
        }
    }
}