using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Buffs;

namespace TheBindingOfRarria.Content.Items;

public class GeneSickle : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.height = 30;
        Item.width = 32;
        Item.value = Item.sellPrice(0, 3);
        Item.rare = ItemRarityID.Expert;
        Item.expert = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.AddBuff(ModContent.BuffType<LifePool>(), 2);
        var p = player.GetModPlayer<GeneThiefPlayer>();
        p.counter--;

        if (p.counter < 0)
        {
            p.counter = 300;
            p.genePool -= p.genePool > 0 ? 1 : 0;
        }

        player.statLifeMax2 += p.genePool;

        player.GetArmorPenetration(DamageClass.Generic) += p.maxHP / 100;
    }

    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        float value = Main.LocalPlayer.GetModPlayer<GeneThiefPlayer>().maxHP / 100;

        string text = string.Format(Language.GetTextValue($"Mods.TheBindingOfRarria.Items.{Name}.Tooltip"), $"{value}");

        int index = tooltips.FindIndex(line => line.Name == "Tooltip0");
        if (index != -1)
        {
            text = text[..text.IndexOf($"\n")];
            tooltips[index].Text = text;
        }
    }
}

public class GeneticPeakNPC : GlobalNPC
{
    public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
    {
        if (npc.type == NPCID.GoblinShark)
            npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsExpert(), ModContent.ItemType<GeneSickle>(), 15));

        base.ModifyNPCLoot(npc, npcLoot);
    }
}

public class GeneThiefPlayer : ModPlayer
{
    public int maxHP = 0;

    public int genePool;
    public int counter = 300;

    public override void PostUpdate()
    {
        if (!Player.HasBuff(ModContent.BuffType<LifePool>()))
            genePool = 0;

        genePool = Math.Min(genePool, 150);

        maxHP = Player.statLifeMax2;
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (!target.active && genePool < 150)
            genePool += 1;

        base.OnHitNPC(target, hit, damageDone);
    }
}