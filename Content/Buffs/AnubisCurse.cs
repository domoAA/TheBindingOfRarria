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
        public override void PostUpdate()
        {
            if (Player.HasBuff(ModContent.BuffType<AnubisCurse>()))
                Player.lifeRegen -= 2;
        }
    }
}