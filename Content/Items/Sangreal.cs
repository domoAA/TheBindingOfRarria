using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Terraria.GameInput;
using Terraria.Localization;
using Terraria;
using Terraria.ModLoader;
using TheBindingOfRarria.Common.Config;
using TheBindingOfRarria.Content.Buffs;
using System.Linq;
using Terraria.ID;
using Terraria.DataStructures;
using System;
using Terraria.GameContent.ItemDropRules;

namespace TheBindingOfRarria.Content.Items;

public class Sangreal : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.height = 30;
        Item.width = 26;
        Item.lifeRegen += 8;
        Item.rare = ItemRarityID.Pink;
        Item.value = Item.sellPrice(0, 5);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<SangrealPlayer>().Noble = true;
    }

    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        var value = Main.LocalPlayer.GetModPlayer<SangrealPlayer>().Limit;

        string text = string.Format(Language.GetTextValue($"Mods.TheBindingOfRarria.Items.{Name}.Tooltip"), $"{value}");

        int index = tooltips.FindIndex(line => line.Name == "Tooltip1");
        if (index != -1)
        {
            text = text[..text.LastIndexOf($"\n")];
            text = text[(text.LastIndexOf($"\n") + 1)..];
            tooltips[index].Text = text;
        }
    }
}

public class SangrealPlayer : ModPlayer
{
    public bool Noble = false;

    public int Limit = 500;

    public override void Load()
    {
        On_Player.UpdateLifeRegen += On_Player_UpdateLifeRegen;
        On_Player.Heal += On_Player_Heal;
        On_Player.HealEffect += On_Player_HealEffect;
    }

    public override void PostUpdate()
    {
        Limit = (int)(Player.statLifeMax2 * 0.66f);
    }

    private static void On_Player_HealEffect(On_Player.orig_HealEffect orig, Player self, int healAmount, bool broadcast)
    {
        orig(self, healAmount, broadcast);

        var p = self.GetModPlayer<SangrealPlayer>();
        if (p.Noble)
            self.statLife = Math.Min(self.statLife, p.Limit);
    }

    private static void On_Player_Heal(On_Player.orig_Heal orig, Player self, int amount)
    {
        orig(self, amount);

        var p = self.GetModPlayer<SangrealPlayer>();
        if (p.Noble)
            self.statLife = Math.Min(self.statLife, p.Limit);
    }

    private static void On_Player_UpdateLifeRegen(On_Player.orig_UpdateLifeRegen orig, Player self)
    {
        orig(self);

        var p = self.GetModPlayer<SangrealPlayer>();
        if (p.Noble)
            self.statLife = Math.Min(self.statLife, p.Limit);
    }

    public override void ResetEffects()
    {
        if (Noble)
            Player.statLife = Math.Min(Player.statLife, Limit);
        
        Noble = false;
    }

    public override void ModifyHurt(ref Player.HurtModifiers modifiers)
    {
        if (Noble)
        {
            modifiers.IncomingDamageMultiplier *= 1.1f;
        }
    }
}

public class SangrealDropNPC : GlobalNPC
{
    public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
    {
        if (npc.type == NPCID.Vampire || npc.type == NPCID.VampireBat)
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Sangreal>(), 10));

        base.ModifyNPCLoot(npc, npcLoot);
    }
}