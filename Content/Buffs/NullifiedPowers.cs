
namespace TheBindingOfRarria.Content.Buffs
{
    public class NullifiedPowers : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            //npc.damage = 0;
        }
    }
}