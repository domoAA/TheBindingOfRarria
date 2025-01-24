using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items
{
    public class CursedWoodMask : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 28;
            Item.height = 32;
            Item.defense = 1;
            Item.value = Item.buyPrice(0, 6);
            Item.rare = ItemRarityID.Lime;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.maxMinions += 1; // check if it resets

            foreach (var member in Main.ActiveNPCs)
            {
                if (member.friendly || member == null || member.Center.DistanceSQ(player.Center) > 500 * 500) 
                    continue;

                var mult = 0f;
                foreach (var buff in member.buffType)
                {
                    mult += 0.1f;
                }

                member.takenDamageMultiplier += mult; // check if it resets
            }
        }
        public override void AddRecipes()
        {
            Recipe.Create(Item.type)
                .AddIngredient(ItemID.PygmyNecklace)
                .AddIngredient(ItemID.SoulofNight, 10)
                .AddIngredient(ItemID.CursedFlame, 10)
                .AddIngredient(ItemID.FlaskofPoison, 10)
                .AddIngredient(ItemID.RichMahogany, 30)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();

            base.AddRecipes();
        }
    }
}