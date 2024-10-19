using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace TheBindingOfRarria.Content.Items
{
    public class GolemsHeart : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 24;
            Item.accessory = true;
            Item.defense = 1;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.DefenseEffectiveness *= (0.08f + player.DefenseEffectiveness.Value) / player.DefenseEffectiveness.Value;

            Item.defense = 1;
            player.GetModPlayer<GolemPlayer>().StoneHeart = true;
        }
        public override void UpdateInventory(Player player)
        {
            Item.defense = 1;
        }
        public override int ChoosePrefix(UnifiedRandom rand)
        {
            var chance = rand.NextFloat() / 2;
            if (chance < 0.1f)
                return PrefixID.Warding;
            else if (chance < 0.2f)
                return PrefixID.Armored;
            else if (chance < 0.3f)
                return PrefixID.Hard;
            else 
                return base.ChoosePrefix(rand);
        }
    }
    public class GolemPlayer : ModPlayer
    {
        public bool StoneHeart = false;
        public override void ResetEffects()
        {
            StoneHeart = false;
        }
        public override void PostUpdateEquips()
        {
            if (!StoneHeart)
                return;

            var index = Array.FindIndex(Player.armor, eek => eek.type == ModContent.ItemType<GolemsHeart>() && !eek.social);
            if (index == -1)
                return;

            foreach (var item in Player.armor)
            {
                if (!item.social && item.shieldSlot != -1) //WARNING!   doesnt seem to be workin
                {
                    Player.armor[index].defense++;
                }
            }
        }
    }
}