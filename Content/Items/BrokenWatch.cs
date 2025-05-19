using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TheBindingOfRarria.Common.Config;
using TheBindingOfRarria.Common.Helpers;
using TheBindingOfRarria.Content.Buffs;
using TheBindingOfRarria.Content.Projectiles;

namespace TheBindingOfRarria.Content.Items;

public class BrokenWatch : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.height = 24;
        Item.width = 22;
        Item.rare = ItemRarityID.Pink;
        Item.value = Item.buyPrice(0, 5);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<ZaWardoPlayer>().counter--;
        if (player.GetModPlayer<ZaWardoPlayer>().counter > 0)
            player.AddBuff(ModContent.BuffType<BrokenWatch_CD>(), player.GetModPlayer<ZaWardoPlayer>().counter);

        player.GetModPlayer<ZaWardoPlayer>().ZaWardo = true;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.FastClock)
            .AddIngredient(ItemID.StoneBlock)
            .AddTile(TileID.MythrilAnvil)
            .Register();
    }

    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        string key = KeybindSystem.ZaWardoKey.GetAssignedKeys().FirstOrDefault();
        if (key == "" || key == null)
            key = "P";

        string text = string.Format(Language.GetTextValue("Mods.TheBindingOfRarria.Items.BrokenWatch.Tooltip"), key);

        int index = tooltips.FindIndex(line => line.Name == "Tooltip0");
        if (index != -1)
        {
            text = text.Remove(text.LastIndexOf($"\n"));
            text = text.Remove(text.LastIndexOf($"\n"));
            tooltips[index].Text = text;
        }
    }
}

public class ZaWardoPlayer : ModPlayer
{
    public int counter = 0;
    public bool ZaWardo = false;

    public override void ResetEffects()
    {
        if ((!ZaWardo || counter <= 0) && Player.HasBuff(ModContent.BuffType<BrokenWatch_CD>()))
            Player.ClearBuff(ModContent.BuffType<BrokenWatch_CD>());

        ZaWardo = false;
    }

    public override void ProcessTriggers(TriggersSet triggersSet)
    {
        if ((KeybindSystem.ZaWardoKey.JustPressed || (KeybindSystem.ZaWardoKey.GetAssignedKeys().FirstOrDefault() == null && Main.keyState.IsKeyDown(Keys.P))) && Main.myPlayer == Player.whoAmI && counter <= 0 && ZaWardo)
        {
            var rand = (TheBindingOfRarria.State)Main.rand.Next(1, 3);

            foreach (var target in Main.ActiveNPCs)
                target?.GetSlowed(rand, 180);

            foreach (var proj in Main.ActiveProjectiles)
                proj?.GetSlowed(rand, 180);

            counter = 600;
            SoundEngine.PlaySound(SoundID.Shatter, Player.position);
        }
    }
}