using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items;

[AutoloadEquip(EquipType.Shield)]
public class MoltenShield : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;
    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.width = 30;
        Item.height = 32;
        Item.defense = 3;
        Item.rare = ItemRarityID.LightRed;
        Item.value = Item.buyPrice(0, 1, 11, 11);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.DefenseEffectiveness *= (player.DefenseEffectiveness.Value + 0.1f) / player.DefenseEffectiveness.Value;
        player.noKnockback = true;
        player.buffImmune[BuffID.OnFire] = true;
        player.buffImmune[BuffID.OnFire3] = true;
    }

    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        float value = Main.LocalPlayer.DefenseEffectiveness.Value;

        string text = string.Format(Language.GetTextValue($"Mods.TheBindingOfRarria.Items.{Name}.Tooltip"), $"{(int)(value * 100)}%");

        int index = tooltips.FindIndex(line => line.Name == "Tooltip1");
        if (index != -1)
        {
            text = text.Remove(text.LastIndexOf($"\n"));
            text = text[(text.LastIndexOf($"\n") + 1)..];
            tooltips[index].Text = text;
            tooltips[index].OverrideColor = Color.Gray;
        }
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.ObsidianShield)
            .AddIngredient(ItemID.HellstoneBar, 13)
            .AddTile(TileID.MythrilAnvil)
            .Register();
    }
}

public class MoltenCatchPlayer : ModPlayer
{
    public override void CatchFish(FishingAttempt attempt, ref int itemDrop, ref int npcSpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition)
    {
        if (attempt.fishingLevel > 30 && attempt.veryrare && attempt.inLava && attempt.CanFishInLava && Main.hardMode && Main.rand.NextBool(5))
        {
            npcSpawn = -1;
            sonar.Color = new Color(255, 150, 150);
            sonar.Text = Language.GetTextValue("Mods.TheBindingOfRarria.Items.MoltenShield.DisplayName");
            itemDrop = ModContent.ItemType<MoltenShield>();
        }
    }
}