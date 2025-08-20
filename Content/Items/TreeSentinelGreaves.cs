using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items;

[AutoloadEquip(EquipType.Legs)]
public class TreeSentinelGreaves : ModItem
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
        Item.defense = 20;
        Item.rare = ItemRarityID.Lime;
        Item.value = Item.sellPrice(0, 5, 0, 0);
    }

    public override void UpdateEquip(Player player)
    {
        player.GetDamage(DamageClass.Melee) += 0.05f;
        player.aggro += 200;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.ChlorophyteGreaves)
            .AddIngredient(ItemID.GoldGreaves)
            .AddIngredient(ItemID.SoulofLight, 5)
            .AddIngredient(ItemID.Wood, 20)
            .AddTile(TileID.AdamantiteForge)
            .Register();
    }
}