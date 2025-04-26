using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Items;
using Terraria.Localization;
using System;
using Terraria.DataStructures;

namespace TheBindingOfRarria.Common;

public class DivingHelmOverride : GlobalItem
{
    public override bool InstancePerEntity => true;

    public override bool AppliesToEntity(Item entity, bool lateInstantiation)
    {
        return entity.type == ItemID.DivingHelmet;
    }

    public override void UpdateEquip(Item item, Player player)
    {
        player.statLifeMax2 += 40;
    }

    public override string IsArmorSet(Item head, Item body, Item legs)
    {
        if (head.type == ItemID.DivingHelmet && body.type == ModContent.ItemType<DivingPlating>() && legs.type == ModContent.ItemType<DivingGreaves>())
            return "TheBindingOfRarria: diving armor set";

        return base.IsArmorSet(head, body, legs);
    }

    public int counter = 3;

    public override void UpdateArmorSet(Player player, string set)
    {
        if (set == "TheBindingOfRarria: diving armor set")
        {
            // slows drowning process by 33%
            counter--;
            if (counter == 0)
            {
                counter = 3;
                player.breathCD = Math.Max(0, player.breathCD - 1);
            }

            player.buffImmune[BuffID.Suffocation] = true;

            player.setBonus = Language.GetTextValue("Mods.TheBindingOfRarria.Items.DivingHelm.SetBonus");
        }
    }

    public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
    {
        var index = tooltips.FindIndex(l => l.Name == "Defense");

        if (index != -1) 
        {
            var text = Language.GetTextValue("Mods.TheBindingOfRarria.Items.DivingHelm.Tooltip");
            tooltips.Insert(index + 1, new TooltipLine(Mod, "Tooltip1", text[(text.LastIndexOf("\n") + 1)..]));
            tooltips.Insert(index + 1, new TooltipLine(Mod, "Tooltip0", text[..text.LastIndexOf("\n")]));
        } 
    }
}

public class DivingArmorPlayer : ModPlayer
{
    public override void ModifyFishingAttempt(ref FishingAttempt attempt)
    {
        if (Player.setBonus == Language.GetTextValue("Mods.TheBindingOfRarria.Items.DivingHelm.SetBonus"))
        {
            if (Main.rand.NextBool(50))
            {
                if (attempt.legendary)
                    return;

                if (attempt.veryrare)
                    attempt.legendary = true;
                else if (attempt.rare)
                    attempt.veryrare = true;
                else if (attempt.uncommon)
                    attempt.rare = true;
                else if (attempt.common)
                    attempt.uncommon = true;
                else
                    attempt.common = true;

            }
        }
    }

    public override void CatchFish(FishingAttempt attempt, ref int itemDrop, ref int npcSpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition)
    {
        if (Main.rand.NextBool(8) && attempt.veryrare && Player.ZoneBeach)
        {
            npcSpawn = -1;
            sonar.Color = Color.Green;

            var text = Language.GetTextValue("Mods.TheBindingOfRarria.Items.DivingPlating.DisplayName");
            var type = ModContent.ItemType<DivingPlating>();

            if (Main.rand.NextBool())
            {
                text = Language.GetTextValue("Mods.TheBindingOfRarria.Items.DivingGreaves.DisplayName");
                type = ModContent.ItemType<DivingGreaves>();
            }

            sonar.Text = text;
            itemDrop = type;
        }
    }
}