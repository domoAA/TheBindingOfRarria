
using Terraria;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Buffs;

public class FireImmunity : ModBuff
{
    public override void Update(Player player, ref int buffIndex) => player.endurance += 0.1f;
}