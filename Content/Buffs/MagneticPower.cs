
using Terraria.ModLoader;
using Terraria;
using TheBindingOfRarria.Content.Items;

namespace TheBindingOfRarria.Content.Buffs
{
    public class MagneticPower : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
        }
        public float power = 0.2f;
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<MagnetizedNPC>().power = power;
        }
        public override bool ReApply(NPC npc, int time, int buffIndex)
        {
            power += 0.2f;
            return base.ReApply(npc, time, buffIndex);
        }
    }
}