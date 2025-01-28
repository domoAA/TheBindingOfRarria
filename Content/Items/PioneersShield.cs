using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items
{
    [AutoloadEquip(EquipType.Shield)]
    public class PioneersShield : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 30;
            Item.accessory = true;
            Item.defense = 4;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.buyPrice(0, 2);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.buffImmune[BuffID.Slow] = true;
            player.noKnockback = true;
            player.moveSpeed += 0.07f;
        }
        public override void AddRecipes()
        {
            Recipe.Create(Item.type)
                .AddIngredient(ItemID.CobaltShield)
                .AddIngredient(ItemID.GoldBar, 13)
                .AddIngredient(ItemID.SoulofFlight, 7)
                .AddTile(TileID.MythrilAnvil)
                .Register();

            base.AddRecipes();
        }
    }
}