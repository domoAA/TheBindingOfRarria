using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items;

public class ArmorConversion : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.width = 26;
        Item.height = 32;
        Item.accessory = true;
        Item.rare = ItemRarityID.Yellow;
        Item.value = Item.sellPrice(0, 10);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.noKnockback = true;
        player.statLifeMax2 += 80;
        player.statDefense -= 8;
        player.lifeRegenCount += 4;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.PaladinsShield)
            .AddIngredient(ItemID.AegisCrystal)
            .AddIngredient(ItemID.LifeCrystal, 2)
            .AddIngredient(ItemID.RegenerationPotion, 5)
            .AddTile(TileID.MythrilAnvil)
            .Register();
    }
}