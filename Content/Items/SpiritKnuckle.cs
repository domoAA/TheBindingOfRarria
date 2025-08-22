using System;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace TheBindingOfRarria.Content.Items;

public class SpiritKnuckle : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.height = 28;
        Item.width = 30;
        Item.defense = 6;
        Item.rare = ItemRarityID.Pink;
        Item.value = Item.sellPrice(0, 9, 60);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<SoulPlayer>().IsKnuckle = true;
        player.GetModPlayer<SoulPlayer>().IsSoul = true;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.FleshKnuckles)
            .AddIngredient(ModContent.ItemType<SoulCatcher>())
            .AddIngredient(ItemID.SoulofNight, 6)
            .AddTile(TileID.TinkerersWorkbench)
            .Register();
    }
}

public partial class SoulPlayer : ModPlayer
{
    public static void OnHitWithKnuckle(ref NPC target)
    {
        target.StrikeNPC(target.CalculateHitInfo(40, 1, damageType: DamageClass.Magic));
    }
}