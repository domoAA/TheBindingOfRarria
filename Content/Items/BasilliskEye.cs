using TheBindingOfRarria.Common.Systems;

namespace TheBindingOfRarria.Content.Items;

public class BasilliskEye : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.width = 34;
        Item.height = 42;
        Item.rare = ItemRarityID.Expert;
        Item.value = Item.sellPrice(0, 8, 80);
        Item.expertOnly = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual) => HPDecreaseSystem.Basillisk = true;

}

public class BasilliskItemNPCShop : GlobalNPC
{
    public override void ModifyShop(NPCShop shop)
    {
        if (shop.NpcType == NPCID.BestiaryGirl)
        {
            shop.Add(new Item(ModContent.ItemType<BasilliskEye>()), Condition.InExpertMode, Condition.DownedPlantera);
        }
    }
}