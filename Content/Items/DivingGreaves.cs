using Terraria;
using Terraria.ID;
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
        Item.rare = ItemRarityID.Green;
        Item.value = Item.buyPrice(0, 0, 89, 76);
    }

    public override void UpdateEquip(Player player)
    {
        player.statLifeMax2 += 30;
    }
}