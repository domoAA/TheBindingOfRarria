using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Items;

namespace TheBindingOfRarria.Content.Buffs
{
    public class PoisonImmunity : ModBuff
    {
        public MedicatedPlayer.DiseaseImmunity PrevImmune = MedicatedPlayer.DiseaseImmunity.Poison;
        public override void Update(Player player, ref int buffIndex)
        {
            player.buffImmune[BuffID.Poisoned] = true;
            player.buffImmune[BuffID.Venom] = true;

            var cur = player.GetModPlayer<MedicatedPlayer>().CurrentImmunity;
            if (cur == MedicatedPlayer.DiseaseImmunity.Poison)
                player.buffTime[buffIndex] = 2;
            else if (PrevImmune != cur)
                player.buffTime[buffIndex] = 300;

            PrevImmune = cur;
        }
    }
    public class FireImmunity : ModBuff
    {
        public MedicatedPlayer.DiseaseImmunity PrevImmune = MedicatedPlayer.DiseaseImmunity.Fire;
        public override void Update(Player player, ref int buffIndex)
        {
            player.buffImmune[BuffID.OnFire] = true;
            player.buffImmune[BuffID.OnFire3] = true;

            var cur = player.GetModPlayer<MedicatedPlayer>().CurrentImmunity;
            if (cur == MedicatedPlayer.DiseaseImmunity.Fire)
                player.buffTime[buffIndex] = 2;
            else if (PrevImmune != cur)
                player.buffTime[buffIndex] = 300;

            PrevImmune = cur;
        }
    }
}