using Terraria;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Buffs;

public class BerserkersPauldronBuff : ModBuff
{
    public override string Texture => ContentPath + "Buffs/" + Name;

    public override void Update(Player player, ref int buffIndex) => player.GetDamage(DamageClass.Generic) += 0.25f;
}