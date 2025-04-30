using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
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
        Item.rare = ItemRarityID.Orange;
        Item.value = Item.buyPrice(0, 0, 89, 76);
    }

    public override void UpdateEquip(Player player)
    {
        player.statLifeMax2 += 30;
    }
}

public class DivingGreavesFishingPlayer : ModPlayer
{
    public override void CatchFish(FishingAttempt attempt, ref int itemDrop, ref int npcSpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition)
    {
        if (attempt.fishingLevel > 30 && attempt.veryrare && Main.rand.NextBool(6))
        {
            npcSpawn = -1;
            sonar.Color = new Color(255, 200, 150);
            sonar.Text = Language.GetTextValue("Mods.TheBindingOfRarria.Items.DivingGreaves.DisplayName");
            itemDrop = ModContent.ItemType<DivingGreaves>();
        }
    }
}
