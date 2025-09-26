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
        Item.rare = ItemRarityID.Green;
        Item.value = Item.sellPrice(0, 0, 7, 20);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<FasthealPlayer>().Fast = true;
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