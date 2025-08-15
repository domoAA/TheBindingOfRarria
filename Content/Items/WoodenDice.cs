using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Common;

namespace TheBindingOfRarria.Content.Items;

public class WoodenDice : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.width = 26;
        Item.height = 28;
        Item.rare = ItemRarityID.Green;
        Item.value = Item.sellPrice(0, 0, 7, 20);
    }

    public override void UpdateAccessory(Player player, bool hideVisual) => player.GetModPlayer<LuckRollPlayer>().Dice = true;

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.Wood)
            .AddIngredient(ItemID.BlackInk)
            .AddIngredient(ItemID.LuckPotion)
            .AddTile(TileID.WorkBenches)
            .Register();
    }
}