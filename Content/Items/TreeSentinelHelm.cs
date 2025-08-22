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

namespace TheBindingOfRarria.Content.Items;

[AutoloadEquip(EquipType.Head)]
public class TreeSentinelHelm : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetStaticDefaults()
    {
        ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;
        ArmorIDs.Head.Sets.IsTallHat[Item.headSlot] = true;
    }

    public override void SetDefaults()
    {
        Item.width = 28;
        Item.height = 30;
        Item.defense = 20;
        Item.lifeRegen = 6;
        Item.rare = ItemRarityID.Lime;
        Item.value = Item.sellPrice(0, 5, 0, 0);
    }

    public override void UpdateEquip(Player player)
    {
        player.GetDamage(DamageClass.Melee) += 0.05f;
        player.aggro += 300;
    }

    public override bool IsArmorSet(Item head, Item body, Item legs)
    {
        return head.type == Item.type && body.type == ModContent.ItemType<TreeSentinelCuirass>() && legs.type == ModContent.ItemType<TreeSentinelGreaves>();
    }

    public override void UpdateArmorSet(Player player)
    {
        string key = KeybindSystem.TreeSentinelKey.GetAssignedKeys().FirstOrDefault();
        if (key == "" || key == null)
            key = "L";
        player.setBonus = string.Format(Language.GetTextValue($"Mods.TheBindingOfRarria.Items.{Name}.SetBonus"), key);

        player.noKnockback = true;
        player.GetModPlayer<TreeSentinelPlayer>().TreeSentinel = true;
    }

    public override void ArmorSetShadows(Player player)
    {
        if (player.GetModPlayer<TreeSentinelPlayer>().counter >= 0)
            player.armorEffectDrawOutlines = true;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.ChlorophyteMask)
            .AddIngredient(ItemID.GoldHelmet)
            .AddIngredient(ItemID.SoulofLight, 10)
            .AddIngredient(ItemID.Wood, 30)
            .AddTile(TileID.AdamantiteForge)
            .Register();
    }
}

public class TreeSentinelPlayer : ModPlayer
{
    public bool TreeSentinel = true;
    
    public int counter = 0;

    public const int duration = 360;

    public const int cooldown = 900;

    public override void ResetEffects()
    {
        TreeSentinel = false;
    }

    public override void PostUpdate()
    {
        if (counter > 0)
        {
            if (!TreeSentinel) 
            { 
                counter = 0; 
                return;
            }
            counter--;
            if (counter == 0)
                counter = -cooldown;
        }
        else if (counter < 0)
            counter++;
    }

    public override void ProcessTriggers(TriggersSet triggersSet)
    {
        if (counter == 0 && TreeSentinel && Main.myPlayer == Player.whoAmI)
        {
            if (KeybindSystem.TreeSentinelKey.JustPressed || (KeybindSystem.TreeSentinelKey.GetAssignedKeys().FirstOrDefault() == null && Main.keyState.IsKeyDown(Keys.L)))
            {
                counter = duration;
                SoundEngine.PlaySound(SoundID.Item150 with { Pitch = -0.7f });
            }
        }
    }

    public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
    {
        if (counter > 0 && TreeSentinel)
        {
            modifiers.Cancel();
            proj.GetReflected();

            for (int i = 0; i < 6; i++)
                Dust.NewDust(proj.Center - new Vector2(8), 16, 16, DustID.GoldCoin);
        }
    }

    public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
    {
        if (counter <= 0)
            return;

        var color = Color.PaleGoldenrod.ToVector4();

        r *= color.X; g *= color.Y; b *= color.Z; a *= color.W;
    }
}