using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace TheBindingOfRarria.Content.Items;

[AutoloadEquip(EquipType.Body)]
public class RakshasaPlatemail : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetStaticDefaults()
    {
        ArmorIDs.Body.Sets.HidesTopSkin[Item.bodySlot] = true;
    }

    public override void SetDefaults()
    {
        Item.width = 30;
        Item.height = 26;
        Item.defense = 13;
        Item.lifeRegen = 2;
        Item.rare = ItemRarityID.LightRed;
        Item.value = Item.sellPrice(0, 3, 50, 0);
    }

    public override void UpdateEquip(Player player)
    {
        player.GetDamage(DamageClass.Generic) += 0.09f;
        player.moveSpeed += 0.09f;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.AdamantiteBreastplate)
            .AddIngredient(ItemID.CrimsonScalemail)
            .AddIngredient(ItemID.SoulofNight, 15)
            .AddIngredient(ItemID.BloodWater, 4)
            .AddTile(TileID.AdamantiteForge)
            .Register();
    }
}