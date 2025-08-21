using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace TheBindingOfRarria.Content.Items;

[AutoloadEquip(EquipType.Legs)]
public class RakshasaGreaves : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetStaticDefaults()
    {
        ArmorIDs.Legs.Sets.HidesBottomSkin[Item.legSlot] = true;
    }

    public override void SetDefaults()
    {
        Item.width = 22;
        Item.height = 16;
        Item.defense = 10;
        Item.lifeRegen = 2;
        Item.rare = ItemRarityID.LightRed;
        Item.value = Item.sellPrice(0, 3, 0, 0);
    }

    public override void UpdateEquip(Player player)
    {
        player.GetDamage(DamageClass.Generic) += 0.09f;
        player.moveSpeed += 0.09f;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.AdamantiteLeggings)
            .AddIngredient(ItemID.CrimsonGreaves)
            .AddIngredient(ItemID.SoulofNight, 5)
            .AddIngredient(ItemID.BloodWater, 2)
            .AddTile(TileID.AdamantiteForge)
            .Register();
    }
}