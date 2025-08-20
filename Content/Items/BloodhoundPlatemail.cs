using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items;

[AutoloadEquip(EquipType.Body)]
public class BloodhoundPlatemail : ModItem
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
        Item.defense = 6;
        Item.value = Item.sellPrice(0, 2, 0, 0);
        Item.master = true;
    }

    public override void UpdateEquip(Player player)
    {
        player.GetAttackSpeed(DamageClass.Melee) += 0.05f;
        player.GetCritChance(DamageClass.Melee) += 5f;
    }
}