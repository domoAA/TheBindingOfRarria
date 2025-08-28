using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Audio;
using TheBindingOfRarria.Common.Config;
using TheBindingOfRarria.Content.Projectiles;
using TheBindingOfRarria.Common.Helpers;

namespace TheBindingOfRarria.Content.Items;

[AutoloadEquip(EquipType.Head)]
public class NightriderHelmet : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetStaticDefaults()
    {
        ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;
        ArmorIDs.Head.Sets.IsTallHat[Item.headSlot] = true;
    }

    public override void SetDefaults()
    {
        Item.width = 26;
        Item.height = 28;
        Item.defense = 11;
        Item.rare = ItemRarityID.LightRed;
        Item.value = Item.sellPrice(0, 3, 50, 0);
    }

    public override void UpdateEquip(Player player)
    {
        player.GetDamage(DamageClass.Melee) += 0.06f;
        player.aggro -= 200;
    }

    public override bool IsArmorSet(Item head, Item body, Item legs)
    {
        return head.type == Item.type && body.type == ModContent.ItemType<NightriderPlatemail>() && legs.type == ModContent.ItemType<NightriderGreaves>();
    }

    public override void UpdateArmorSet(Player player)
    {
        player.setBonus = Language.GetTextValue($"Mods.TheBindingOfRarria.Items.{Name}.SetBonus");

        player.GetModPlayer<NightriderPlayer>().Cavalry = true;
    }

    public override void ArmorSetShadows(Player player)
    {
        if (player.GetModPlayer<NightriderPlayer>().Cavalry)
        {
            player.armorEffectDrawShadow = true;
        }
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.TitaniumMask)
            .AddIngredient(ItemID.ShadowHelmet)
            .AddIngredient(ItemID.SoulofNight, 10)
            .AddTile(TileID.AdamantiteForge)
            .Register();
    }
}

public class NightriderPlayer : ModPlayer
{
    public bool Cavalry = true;

    public const float power = 0.03f;

    public override void ResetEffects()
    {
        Cavalry = false;
    }

    public override void PostUpdateEquips()
    {
        if (Main.dayTime && !Player.mount.Active)
            Cavalry = false;

    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (Cavalry && (hit.DamageType == DamageClass.Melee || hit.DamageType == DamageClass.MeleeNoSpeed) && target.canGhostHeal && Player.lifeSteal > 0)
        {
            var amount = (int)(damageDone * power);
            Player.Heal(amount);
            Player.lifeSteal -= damageDone * 0.3f;
        }
    }
}