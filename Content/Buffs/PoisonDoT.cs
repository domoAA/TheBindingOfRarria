using Terraria.ModLoader;
using Terraria;
using TheBindingOfRarria.Content.Projectiles;
using System;
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

        }

    }
}