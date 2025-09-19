using TheBindingOfRarria.Common;

namespace TheBindingOfRarria.Content.Items;

public class WispfireLantern : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.width = 26;
        Item.height = 28;
        Item.rare = ItemRarityID.Green;
        Item.value = Item.sellPrice(0, 0, 7, 20);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {

    }

}