using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Dusts;

namespace TheBindingOfRarria.Content.Buffs;

public class ReverseCT : ModBuff
{
    public override string Texture => ContentPath + "Buffs/" + Name;

    public override void Update(Player player, ref int buffIndex) => player.lifeRegenCount += player.statLifeMax2 / 5;

    public override bool ReApply(Player player, int time, int buffIndex) => true;
}