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

namespace TheBindingOfRarria.Content.Items;

public class Sangreal : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.height = 30;
        Item.width = 26;
        Item.rare = ItemRarityID.LightRed;
        Item.value = Item.buyPrice(0, 4);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        var p = player.GetModPlayer<SangrealPlayer>();
        p.Noble = true;
        p.counter--;

        if (player.HasBuff(ModContent.BuffType<NobleWine>()) && p.Stored == 151)
        {
            player.Heal(150);
            p.Stored = 150;
        }
    }

    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        string key = KeybindSystem.SangrealKey.GetAssignedKeys().FirstOrDefault();
        if (key == "" || key == null)
            key = "J";

        string text = string.Format(Language.GetTextValue("Mods.TheBindingOfRarria.Items.Sangreal.Tooltip"), key);


        int index = tooltips.FindIndex(line => line.Name == "Tooltip0");
        if (index != -1)
        {
            text = text.Remove(text.LastIndexOf($"\n"));
            text = text.Remove(text.LastIndexOf($"\n"));
            text = text.Remove(text.LastIndexOf($"\n"));
            tooltips[index].Text = text;
        }
    }
}

public class SangrealPlayer : ModPlayer
{
    public bool Noble = false;
    public int counter = 0;
    public int Stored = 0;

    public override void ResetEffects()
    {
        if (!Noble && Player.HasBuff(ModContent.BuffType<NobleWine>()))
            Player.ClearBuff(ModContent.BuffType<NobleWine>());

        Noble = false;
    }

    public override void PostUpdateEquips()
    {
        if (!Player.HasBuff(ModContent.BuffType<NobleWine>()) && Stored > 0)
        {
            var DamageSource = PlayerDeathReason.ByCustomReason(NetworkText.FromKey("Mods.TheBindingOfRarria.Items.Sangreal.DeathMessage", Player.name));
            Player.Hurt(DamageSource, Stored * 2, 1, dodgeable: false, knockback: 0);

            Stored = 0;
        }
    }

    public override void OnHurt(Player.HurtInfo info)
    {
        if (Stored > 0)
            Stored = Math.Max(0, Stored - info.Damage);
    }

    public override void ProcessTriggers(TriggersSet triggersSet)
    {
        if (Noble && (KeybindSystem.SangrealKey.JustPressed || (KeybindSystem.SangrealKey.GetAssignedKeys().FirstOrDefault() == null && Main.keyState.IsKeyDown(Keys.J))) && Main.myPlayer == Player.whoAmI)
        {
            if (counter <= 0)
            {
                Player.AddBuff(ModContent.BuffType<NobleWine>(), 90);
                Stored = 151;

                counter = 270;
            }
        }
    }
}
