using System;
using Terraria;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items
{
    public class ArmorConversion : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 32;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statLifeMax2 += (player.statLifeMax2 / 100) * player.statDefense;
            player.statDefense -= player.statDefense;
            player.GetModPlayer<ConvertedPlayer>().IsConverted = true;
        }
    }
    public class ConvertedPlayer : ModPlayer
    {
        public bool IsConverted = false;
        public override void ResetEffects()
        {
            IsConverted = false;
        }
        public override void UpdateLifeRegen()
        {
            if (IsConverted)
                Player.lifeRegen = Math.Max(Player.lifeRegen + Player.statLifeMax2 / 50, Player.statLifeMax2 / 50);
        }
        public override void UpdateBadLifeRegen()
        {
            if (IsConverted)
                Player.lifeRegen = Math.Max(Player.lifeRegen, 0);
        }
    }
}