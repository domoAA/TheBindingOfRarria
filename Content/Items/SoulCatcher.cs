using Terraria.ModLoader;
using Terraria;
using System;

namespace TheBindingOfRarria.Content.Items
{
    public class SoulCatcher : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.height = 30;
            Item.width = 30;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<SoulPlayer>().IsSoul = true;
        }
    }
    public class SoulPlayer : ModPlayer
    {
        public bool IsSoul;
        public bool SoulTook;
        public override void ResetEffects()
        {
            if (SoulTook && Player.ItemAnimationEndingOrEnded)
                SoulTook = false;

            IsSoul = false;
        }

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (IsSoul && !SoulTook)
            {
                SoulTook = true;
                Player.statMana = Math.Min(Player.statMana + 6, Player.statManaMax2);
                Player.ManaEffect(6);
            }
        }
    }
}
