using Terraria.Audio;

namespace TheBindingOfRarria.Content.Items;

public class InjectorBand : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.width = 36;
        Item.height = 36;
        Item.rare = ItemRarityID.Expert;
        Item.value = Item.sellPrice(0, 0, 6, 66);
        Item.expert = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<FasthealPlayer>().Fast = true;
    }
}

public class InjectorItemNPCShop : GlobalNPC
{
    public override void ModifyShop(NPCShop shop)
    {
        if (shop.NpcType == NPCID.Merchant)
        {
            shop.Add(new Item(ModContent.ItemType<InjectorBand>()), Condition.DownedEowOrBoc, Condition.InExpertMode);
        }
    }
}

public class FasthealPlayer : ModPlayer
{
    public bool Fast = false;

    public override void ResetEffects()
    {
        Fast = false;
    }

    public override void Load()
    {
        On_Player.AddBuff_DetermineBuffTimeToAdd += On_Player_AddBuff_DetermineBuffTimeToAdd;
    }

    private static int On_Player_AddBuff_DetermineBuffTimeToAdd(On_Player.orig_AddBuff_DetermineBuffTimeToAdd orig, Player self, int type, int time1)
    {
        if (self.GetModPlayer<FasthealPlayer>().Fast && type == BuffID.PotionSickness)
            time1 = (int)MathHelper.Max(1, time1 - 600);

        return orig(self, type, time1);
    }
}