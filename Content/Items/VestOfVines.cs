using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items
{
    public class VestOfVines : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.height = 24;
            Item.width = 26;
            Item.value = Item.buyPrice(0, 7);
            Item.rare = ItemRarityID.Pink;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var counter = 0f;
            for (int i = 0; i < player.buffType.Length; i++)
            {
                if (Main.debuff[player.buffType[i]])
                    counter += 0.02f;
            }
            player.endurance += counter;
        }
        public override void AddRecipes()
        {
            Recipe.Create(Item.type)
                .AddIngredient(ItemID.ArmorPolish)
                .AddIngredient(ItemID.CursedFlame, 10)
                .AddIngredient(ItemID.Wood, 20)
                .AddIngredient(ItemID.Vine, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();

            Recipe.Create(Item.type)
                .AddIngredient(ItemID.ArmorPolish)
                .AddIngredient(ItemID.Ichor, 10)
                .AddIngredient(ItemID.Wood, 20)
                .AddIngredient(ItemID.Vine, 5)
                .AddTile(TileID.MythrilAnvil)
                .Register();

            base.AddRecipes();
        }
    }
}