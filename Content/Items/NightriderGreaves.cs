using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items;

[AutoloadEquip(EquipType.Legs)]
public class NightriderGreaves : ModItem
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
        Item.defense = 9;
        Item.rare = ItemRarityID.LightRed;
        Item.value = Item.sellPrice(0, 3, 0, 0);
    }

    public override void UpdateEquip(Player player)
    {
        player.GetCritChance(DamageClass.Melee) += 6f;
        player.aggro -= 200;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.TitaniumLeggings)
            .AddIngredient(ItemID.ShadowGreaves)
            .AddIngredient(ItemID.SoulofNight, 5)
            .AddTile(TileID.AdamantiteForge)
            .Register();
    }
}