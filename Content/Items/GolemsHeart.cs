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
}