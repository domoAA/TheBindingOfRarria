using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items;

[AutoloadEquip(EquipType.Body, EquipType.Back)]
public class NightriderPlatemail : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetStaticDefaults()
    {
        ArmorIDs.Body.Sets.HidesTopSkin[Item.bodySlot] = true;
        ArmorIDs.Body.Sets.IncludedCapeBackFemale[Item.bodySlot] = Item.backSlot;
        ArmorIDs.Body.Sets.IncludedCapeBack[Item.bodySlot] = Item.backSlot;
    }

    public override void SetDefaults()
    {
        Item.width = 30;
        Item.height = 28;
        Item.defense = 15;
        Item.rare = ItemRarityID.LightRed;
        Item.value = Item.sellPrice(0, 4, 0, 0);
    }

    public override void UpdateEquip(Player player)
    {
        player.GetCritChance(DamageClass.Melee) += 6f;
        player.GetDamage(DamageClass.Melee) += 0.06f;
        player.aggro -= 200;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.TitaniumBreastplate)
            .AddIngredient(ItemID.ShadowScalemail)
            .AddIngredient(ItemID.SoulofNight, 15)
            .AddTile(TileID.AdamantiteForge)
            .Register();
    }
}