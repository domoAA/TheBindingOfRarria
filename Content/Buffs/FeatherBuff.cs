using System;
using Terraria;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Buffs
{
    public class FeatherBuff : ModBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.velocity.Y > 1)
                player.velocity.Y *= 0.6f;

            player.jumpSpeedBoost += 2f;
        }
    }
}