using Terraria;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Buffs;

public class PoisonImmunity : ModBuff
{
    public override string Texture => ContentPath + "Buffs/" + Name;

    public override void Update(Player player, ref int buffIndex) => player.lifeRegen += 4;
}
