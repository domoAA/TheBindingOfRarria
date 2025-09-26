using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.Localization;
using TheBindingOfRarria.Common.Systems;

namespace TheBindingOfRarria.Content.Items;

public class WardingBell : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.width = 20;
        Item.height = 26;
        Item.rare = ItemRarityID.Master;
        Item.value = Item.sellPrice(0, 2, 28);
        Item.master = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        var p = player.GetModPlayer<WardingPlayer>();
        p.Warding = true;
        player.statDefense += p.counter.num * p.counter.stack;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.Bell)
            .AddIngredient(ModContent.ItemType<PaleOre>(), 20)
            .AddTile(TileID.Anvils)
            .Register();
    }

    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        int index = tooltips.FindIndex(t => t.Name == "Tooltip1");
        if (index != -1)
        {
            var p = Main.LocalPlayer.GetModPlayer<WardingPlayer>();
            var t = $"{p.counter.num}x{p.counter.stack}";
            
            string text = string.Format(Language.GetTextValue($"Mods.TheBindingOfRarria.Items.{Name}.Tooltip"), t);

            text = text[..text.LastIndexOf($"\n")];
            text = text[(text.LastIndexOf($"\n")+ 1)..];
            tooltips[index].Text = text;
            tooltips[index].OverrideColor = Color.Gray;
        }
    }
}

public class WardingPlayer : ModPlayer
{
    public bool Warding = false;

    public (int num, int stack) counter = (0, 0);

    public override void PostUpdate()
    {
        if (!Player.HasBuff(BuffID.PotionSickness) || !Warding)
            counter.stack = 0;

        counter.num = Player.armor.Count(i => i.prefix == PrefixID.Warding);
    }

    public override void ResetEffects()
    {
    }

    public override void Load()
    {
        On_Player.ApplyLifeAndOrMana += On_Player_ApplyLifeAndOrMana;
        On_Player.ApplyPotionDelay += On_Player_ApplyPotionDelay;
    }

    private void On_Player_ApplyPotionDelay(On_Player.orig_ApplyPotionDelay orig, Player self, Item sItem)
    {
        var p = self.GetModPlayer<WardingPlayer>();
        if (!self.HasBuff(BuffID.PotionSickness))
            p.counter.stack = 0;


        orig(self, sItem);
    }

    private static void On_Player_ApplyLifeAndOrMana(On_Player.orig_ApplyLifeAndOrMana orig, Player self, Item item)
    {
        var p = self.GetModPlayer<WardingPlayer>();

        if (item.healLife > 0 && p.Warding)
            p.counter.stack++;


        orig(self, item);
    }
}