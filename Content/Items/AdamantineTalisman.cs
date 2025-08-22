using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Common;

namespace TheBindingOfRarria.Content.Items;

public class AdamantineTalisman : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.height = 30;
        Item.width = 26;
        Item.defense = 5;
        Item.rare = ItemRarityID.LightRed;
        Item.value = Item.sellPrice(0, 4, 0, 4);
    }

    public override void UpdateAccessory(Player player, bool hideVisual) => player.GetModPlayer<LuckRollPlayer>().Talisman = true;
    
    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ModContent.ItemType<WoodenDice>())
            .AddIngredient(ItemID.WhitePearl)
            .AddIngredient(ItemID.AdamantiteOre, 20)
            .AddIngredient(ModContent.ItemType<PaleOre>(), 20)
            .AddIngredient(ItemID.Diamond, 10)
            .AddTile(TileID.AdamantiteForge)
            .Register();
    }
}