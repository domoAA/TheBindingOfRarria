using TheBindingOfRarria.Common.Systems;

namespace TheBindingOfRarria.Content.Items;

public class BlueSeal : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.width = 30;
        Item.height = 38;
        Item.accessory = true;
        Item.rare = ItemRarityID.Blue;
        Item.value = Item.sellPrice(0, 1);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.statLifeMax2 += 40;
    }
}