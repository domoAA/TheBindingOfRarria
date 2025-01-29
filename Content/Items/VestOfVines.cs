using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items
{
    [AutoloadEquip(EquipType.Body)]
    public class VestOfVines : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.height = 22;
            Item.width = 24;
            Item.value = Item.buyPrice(0, 7);
            Item.rare = ItemRarityID.Pink;
        }
        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<WoodBarkPlayer>().WiseAndMystical = true;
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
    public class WoodBarkPlayer : ModPlayer
    {
        public bool WiseAndMystical = false;
        public override void ResetEffects()
        {
            WiseAndMystical = false;
        }
        public override void UpdateLifeRegen()
        {
            if (WiseAndMystical)
                Player.lifeRegen += (int)(Player.endurance * 20);
            base.UpdateLifeRegen();
        }
    }
}