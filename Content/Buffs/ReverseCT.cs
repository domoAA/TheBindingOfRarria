using Terraria;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Buffs
{
    public class ReverseCT : ModBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            player.lifeRegenCount += player.statLifeMax2 / 5;
        }
        public override bool ReApply(Player player, int time, int buffIndex)
        {
            return true;
        }
    }
}