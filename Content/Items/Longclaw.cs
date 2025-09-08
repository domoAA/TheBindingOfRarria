namespace TheBindingOfRarria.Content.Items;

public class Longclaw : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.height = 32;
        Item.width = 34;
        Item.value = Item.sellPrice(0, 8);
        Item.rare = ItemRarityID.Expert;
        Item.expert = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetArmorPenetration(DamageClass.Melee) += 7;
        player.GetModPlayer<LongclawPlayer>().SlayQueen = true;
    }
}

public class LongclawPlayer : ModPlayer
{
    public bool SlayQueen = false;

    public override void ResetEffects() => SlayQueen = false;

    public override void ModifyItemScale(Item item, ref float scale)
    {
        if (SlayQueen)
        {
            scale *= 1.3f;
        }
    }

}

public class LongclawItemNPCShop : GlobalNPC
{
    public override void ModifyShop(NPCShop shop)
    {
        if (shop.NpcType == NPCID.BestiaryGirl)
        {
            shop.Add(new Item(ModContent.ItemType<Longclaw>()), Condition.InExpertMode, Condition.DownedPlantera);
        }
    }
}