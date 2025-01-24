using Terraria;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Buffs
{
    public class HolyProtection : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[ModContent.BuffType<HolyProtection>()] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 2;
        }
    }
}