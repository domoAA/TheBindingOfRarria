using Terraria;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Buffs;

public class NobleWine : ModBuff
{
    public override string Texture => ContentPath + "Buffs/" + Name;

    public override void Update(Player player, ref int buffIndex) => player.statLifeMax2 += 150;
}
