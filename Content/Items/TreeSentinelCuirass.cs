using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items;

[AutoloadEquip(EquipType.Body)]
public class TreeSentinelCuirass : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetStaticDefaults()
    {
        ArmorIDs.Body.Sets.HidesTopSkin[Item.bodySlot] = true;
    }

    public override void SetDefaults()
    {
        Item.width = 30;
        Item.height = 24;
        Item.defense = 29;
        Item.rare = ItemRarityID.Lime;
        Item.value = Item.sellPrice(0, 6, 0, 0);
    }

    public override void UpdateEquip(Player player)
    {
        player.GetAttackSpeed(DamageClass.Melee) += 0.06f;
        player.GetCritChance(DamageClass.Melee) += 6f;
        player.aggro += 200;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.ChlorophytePlateMail)
            .AddIngredient(ItemID.GoldChainmail)
            .AddIngredient(ItemID.SoulofLight, 15)
            .AddIngredient(ItemID.Wood, 40)
            .AddTile(TileID.AdamantiteForge)
            .Register();
    }
}