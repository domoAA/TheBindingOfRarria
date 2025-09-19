using Terraria.Localization;
using TheBindingOfRarria.Content.Buffs;

namespace TheBindingOfRarria.Content.Items;

public class RustyAxeHead : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.width = 20;
        Item.height = 22;
        Item.accessory = true;
        Item.value = Item.sellPrice(0, 3);
        Item.rare = ItemRarityID.Orange;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<RustyPlayer>().Rusty = true;
    }
}
public class RustyPlayer : ModPlayer
{
    public const int duration = 600;

    public bool Rusty = false;

    public override void ResetEffects()
    {
        Rusty = false;
    }

    public override void OnHurt(Player.HurtInfo info)
    {
        if (Rusty)
        {
            Player.AddBuff(ModContent.BuffType<RustyBerserk>(), duration);
        }
    }
    public override void CatchFish(FishingAttempt attempt, ref int itemDrop, ref int npcSpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition)
    {
        if (Player.ZoneNormalCaverns && attempt.heightLevel == 3 && !attempt.inHoney && !attempt.inLava && attempt.legendary && Main.rand.NextBool(4))
        {
            npcSpawn = -1;
            sonar.Color = Color.Orange;
            sonar.Text = Language.GetTextValue("Mods.TheBindingOfRarria.Items.RustyAxeHead.DisplayName");
            itemDrop = ModContent.ItemType<RustyAxeHead>();
        }
    }
}