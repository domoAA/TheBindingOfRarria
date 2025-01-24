using Terraria;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Buffs
{
    public class RevengeDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[ModContent.BuffType<RevengeDebuff>()] = true;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.takenDamageMultiplier = 2;
        }
    }
}