using System;
using Terraria;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items
{
    public class AnemoiBracelet : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 24;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<NatureDodgePlayer>().HasBracelet = true;
        }
    }
    public class NatureDodgePlayer : ModPlayer
    {
        public bool HasBracelet = false;
        public override void ResetEffects()
        {
            HasBracelet = false;
        }
        public override bool FreeDodge(Player.HurtInfo info)
        {
            if (!HasBracelet)
                return base.FreeDodge(info);

            var rand = new Random();
            var chance = Math.Min(0.07 + Player.moveSpeed / 10, 0.21);
            if (rand.NextDouble() < chance) {
                NatureDodge();
                return true; }
            else
                return false;
        }
        public void NatureDodge()
        {
            Player.NinjaDodge();
            Player.immune = true;
            Player.SetImmuneTimeForAllTypes(Player.longInvince ? 150 : 90);
        }
    }
}