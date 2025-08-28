using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items;

public class BlessedDewTalisman : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.width = 20;
        Item.height = 30;
        Item.accessory = true;
        Item.rare = ItemRarityID.LightRed;
        Item.value = Item.sellPrice(0, 1);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<BlessedDewPlayer>().Blessed = true;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.BottledHoney)
            .AddIngredient(ItemID.SoulofLight, 10)
            .AddTile(TileID.ImbuingStation)
            .Register();
    }
}

public class BlessedDewPlayer : ModPlayer
{
    public bool Blessed = false;

    public override void ResetEffects()
    {
        Blessed = false;
    }

    public override void NaturalLifeRegen(ref float regen)
    {
        if (Player.lifeRegenTime > 0 && Blessed)
        {
            Player.lifeRegenTime = Math.Max(Player.shinyStone ? 3000 : 900, Player.lifeRegenTime);
        }
    }
}