
namespace TheBindingOfRarria.Content.Buffs;

public class RustyBerserk : ModBuff
{
    public override string Texture => ContentPath + "Buffs/" + Name;

    public override void Update(Player player, ref int buffIndex) => player.GetDamage(DamageClass.Generic) += 0.15f;
}