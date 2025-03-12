
namespace TheBindingOfRarria.Content.Buffs
{
    public class AnubisCurse : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
        }
    }
    public class MummyPlayer : ModPlayer
    {
        public override void PostUpdate()
        {
            if (Player.HasBuff(ModContent.BuffType<AnubisCurse>()))
                Player.lifeRegen -= 2;
        }
    }
}