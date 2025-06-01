using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace TheBindingOfRarria.Content.Items;

[AutoloadEquip(EquipType.Head)]
public class VesselMask0 : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.width = 26;
        Item.height = 28;
        Item.vanity = true;
        Item.value = Item.buyPrice(0, 1);
        Item.rare = ItemRarityID.Expert;
        Item.expert = true;
    }
}

[AutoloadEquip(EquipType.Head)]
public class VesselMask1 : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.width = 26;
        Item.height = 28;
        Item.vanity = true;
        Item.value = Item.buyPrice(0, 1);
        Item.rare = ItemRarityID.Expert;
        Item.expert = true;
    }
}