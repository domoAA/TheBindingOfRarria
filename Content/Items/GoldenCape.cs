using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items
{
    public class GoldenCape : ModItem
    {
        public override void SetDefaults()
        {
            Item.height = 30;
            Item.width = 30;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.noFallDmg = true;
            player.GetModPlayer<SpikeImmunePlayer>().SpikeImmune = true;

        }
    }
    public class SpikeImmunePlayer : ModPlayer
    {
        public bool SpikeImmune;
        public override void ResetEffects()
        {
            SpikeImmune = false;
        }
        public override bool ImmuneTo(PlayerDeathReason damageSource, int cooldownCounter, bool dodgeable)
        {
            if (damageSource.SourceOtherIndex == 3 && SpikeImmune)
                return true;

            return false;
        }
    }
}
