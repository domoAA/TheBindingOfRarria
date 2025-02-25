
using Terraria.ModLoader;
using Terraria;

namespace TheBindingOfRarria.Content.Buffs
{
    public class BleedDOT : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.lifeRegen -= 32;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.lifeRegen -= 8;
            player.lifeRegenTime -= 4;
        }
    }
}