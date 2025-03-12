
namespace TheBindingOfRarria.Content.Buffs
{
    public class PoisonImmunity : ModBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            player.lifeRegen += 4;
        }
    }
    public class FireImmunity : ModBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            player.endurance += 0.1f;
        }
    }
}