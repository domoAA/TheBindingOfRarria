using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items;

[AutoloadEquip(EquipType.Legs)]
public class DivingGreaves : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.width = 26;
        Item.height = 18;
        Item.defense = 3;
        Item.rare = ItemRarityID.Orange;
        Item.value = Item.sellPrice(0, 0, 89, 76);
    }

    public override void UpdateEquip(Player player)
    {
        player.statLifeMax2 += 30;
    }
}