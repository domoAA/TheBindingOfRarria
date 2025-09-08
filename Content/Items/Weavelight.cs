using System;

namespace TheBindingOfRarria.Content.Items;

public class Weavelight : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.width = 34;
        Item.height = 34;
        Item.rare = ItemRarityID.Expert;
        Item.value = Item.sellPrice(0, 4, 10);
        Item.expert = true;
    }

    public int counter = 0;

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        Lighting.AddLight(player.Center, new Vector3(0.5f));
        counter++;
        if (counter < 60)
            return;

        counter = 0;
        player.statMana = Math.Min(player.statMana + 5, player.statManaMax2);

    }
}

public class WeavelightItemNPCShop : GlobalNPC
{
    public override void ModifyShop(NPCShop shop)
    {
        if (shop.NpcType == NPCID.SkeletonMerchant)
        {
            shop.Add(new Item(ModContent.ItemType<Weavelight>()), Condition.InExpertMode);
        }
    }
}