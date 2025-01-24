using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items
{
    public class DeadDove : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 20;
            Item.accessory = true;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.buyPrice(0, 1, 12);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.wingTimeMax = player.wingTimeMax * 3 / 2;
            player.wingRunAccelerationMult *= 1.08f;
        }
        public override void AddRecipes()
        {
            Recipe.Create(Item.type)
                .AddIngredient(ItemID.SoulofFlight, 16)
                .AddTile(TileID.Tombstones)
                .Register();

            base.AddRecipes();
        }
    }
}