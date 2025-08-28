using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items;

[AutoloadEquip(EquipType.Body)]
public class SolitudePlatemail : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetStaticDefaults()
    {
        ArmorIDs.Body.Sets.HidesTopSkin[Item.bodySlot] = true;
    }

    public override void SetDefaults()
    {
        Item.width = 30;
        Item.height = 28;
        Item.defense = 7;
        Item.value = Item.sellPrice(0, 1, 25, 0);
        Item.expert = true;
    }

    public override void UpdateEquip(Player player)
    {
        player.aggro += 100;
    }
}