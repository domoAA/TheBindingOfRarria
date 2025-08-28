using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TheBindingOfRarria.Common.Systems;

namespace TheBindingOfRarria.Content.Items;

public class MagicSeal : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.width = 34;
        Item.height = 28;
        Item.rare = ItemRarityID.Expert;
        Item.value = Item.sellPrice(0, 4);
        Item.expert = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        var p = player.GetModPlayer<MagicTriangePlayer>();
        p.MagicTriange = true;
    }

    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        float value = Main.LocalPlayer.statLifeMax2;

        string text = string.Format(Language.GetTextValue($"Mods.TheBindingOfRarria.Items.{Name}.Tooltip"), $"{(int)(value * 0.3f)}");

        int index = tooltips.FindIndex(line => line.Name == "Tooltip0");
        if (index != -1)
        {
            text = text[..text.LastIndexOf($"\n")];
            tooltips[index].Text = text;
        }
    }
}

public class MagicTriangePlayer : ModPlayer
{
    public float Limit = 0.3f;

    public bool MagicTriange = false;

    public override void ResetEffects()
    {
        MagicTriange = false;
        if (Main.GameMode == 2 && Main.getGoodWorld)
            Limit = 0.5f;
        else Limit = 0.3f;
    }

    public override void ModifyHurt(ref Player.HurtModifiers modifiers)
    {
        if (MagicTriange && modifiers.Dodgeable)
        {
            modifiers.SetMaxDamage((int)(Player.statLifeMax2 * Limit));
        }
    }
}

public class MagicTriangeItemNPCShop : GlobalNPC
{
    public override void ModifyShop(NPCShop shop)
    {
        if (shop.NpcType == NPCID.Wizard)
        {
            shop.Add(new Item(ModContent.ItemType<MagicSeal>()), Condition.InExpertMode);
        }
    }
}