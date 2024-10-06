using Terraria.ModLoader;
using System;
using Terraria;

namespace TheBindingOfRarria.Content.Buffs
{
    public class AnubisCurse : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[ModContent.BuffType<AnubisCurse>()] = true;
        }
    }
    public class MummyPlayer : ModPlayer
    {
        public int PrevHP = 200;
        public override void PostUpdate()
        {
            if (Player.HasBuff(ModContent.BuffType<AnubisCurse>()))
                Player.statLife = Math.Min(Player.statLife, PrevHP);
            PrevHP = Player.statLife;
        }
    }
}