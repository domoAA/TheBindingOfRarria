using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items;

[AutoloadEquip(EquipType.Legs)]
public class SolitudeGreaves : ModItem
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
        Item.defense = 6;
        Item.value = Item.sellPrice(0, 0, 75, 0);
        Item.expert = true;
    }

    public override void UpdateEquip(Player player)
    {
        player.aggro += 100;
    }
}