using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TheBindingOfRarria.Common.Config;
using TheBindingOfRarria.Content.Projectiles;

namespace TheBindingOfRarria.Content.Items;

[AutoloadEquip(EquipType.Head)]
public class SolitudeHelmet : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetStaticDefaults()
    {
        ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;
        ArmorIDs.Head.Sets.IsTallHat[Item.headSlot] = true;
    }

    public override void SetDefaults()
    {
        Item.width = 24;
        Item.height = 22;
        Item.defense = 7;
        Item.value = Item.sellPrice(0, 1, 0, 0);
        Item.expert = true;
    }

    public override void UpdateEquip(Player player)
    {
        player.aggro += 100;
    }

    public override bool IsArmorSet(Item head, Item body, Item legs)
    {
        return head.type == Item.type && body.type == ModContent.ItemType<SolitudePlatemail>() && legs.type == ModContent.ItemType<SolitudeGreaves>();
    }

    public override void UpdateArmorSet(Player player)
    {
        player.setBonus = Language.GetTextValue($"Mods.TheBindingOfRarria.Items.{Name}.SetBonus");

        var p = player.GetModPlayer<SolitudePlayer>();
        p.HavelNotHavel = true;
        if (p.Solo)
        {
            player.GetDamage(DamageClass.Generic) += 0.1f;
            player.manaRegen += 4;
        }
    }

    public override void ArmorSetShadows(Player player)
    {
        if (player.GetModPlayer<SolitudePlayer>().Solo)
            player.armorEffectDrawOutlinesForbidden = true;
    }
}

public class LockBoxLootSolitude : GlobalItem
{
    public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
    {
        if (item.type == ItemID.LockBox)
        {
            IItemDropRule rule = ItemDropRule.ByCondition(new Conditions.IsExpert(), ModContent.ItemType<SolitudeHelmet>(), 9);
            itemLoot.Add(rule);
            rule = ItemDropRule.ByCondition(new Conditions.IsExpert(), ModContent.ItemType<SolitudePlatemail>(), 9);
            itemLoot.Add(rule);
            rule = ItemDropRule.ByCondition(new Conditions.IsExpert(), ModContent.ItemType<SolitudeGreaves>(), 9);
            itemLoot.Add(rule);
        }
    }
}

public class SolitudePlayer : ModPlayer
{
    public bool Solo = false;

    public bool HavelNotHavel = false;
    
    public int Range = 300;

    public override void ResetEffects()
    {
        HavelNotHavel = false;
    }

    public override void PostUpdate()
    {
        Solo = true;

        if (Main.npc.Any(t => t.active && !t.immortal && t.Center.DistanceSQ(Player.Center) < Range * Range))
            Solo = false;
    }

    public override void UpdateLifeRegen()
    {
        if (Solo && HavelNotHavel)
        {
            Player.lifeRegen += 4;
        }
    }
}