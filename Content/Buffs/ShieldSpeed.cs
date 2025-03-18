using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Buffs
{
    public class ShieldSpeed : ModBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            player.moveSpeed *= 1.1f;
            player.DefenseEffectiveness *= 1.03f;
        }
    }
}