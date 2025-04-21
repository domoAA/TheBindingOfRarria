using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items;

[AutoloadEquip(EquipType.Body)]
public class DivingPlating : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.width = 28;
        Item.height = 22;
        Item.defense = 5;
        Item.rare = ItemRarityID.Green;
        Item.value = Item.buyPrice(0, 0, 89, 76);
    }

    public override void UpdateEquip(Player player)
    {
        player.statLifeMax2 += 30;
    }
}