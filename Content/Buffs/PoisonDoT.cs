using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
namespace TheBindingOfRarria.Content.Buffs 
{
    public class PoisonDoT : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffDoubleApply[ModContent.BuffType<PoisonDoT>()] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<DamageOverTtimeUserInterfacePlayer>().poison = 1;
        }
    }
    public class DamageOverTtimeUserInterfacePlayer : ModPlayer
    {
        public int poison = 0;
        public static Dictionary<(Player, DamageOverTimeType), int> DamageOverTimeUICollection = [];
        public enum DamageOverTimeType
        {
            Poison,
            Fire
        }
        public override void PostUpdateBuffs()
        {
            if (poison == 0 && DamageOverTimeUICollection.ContainsKey((Player, DamageOverTimeType.Poison)))
                DamageOverTimeUICollection.Remove((Player, DamageOverTimeType.Poison));
            else if (poison == 0)
                return;
            else if (!DamageOverTimeUICollection.ContainsKey((Player, DamageOverTimeType.Poison)))
                DamageOverTimeUICollection.Add((Player, DamageOverTimeType.Poison), poison);
        }
    }
}