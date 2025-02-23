using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace BindingTest.Content.Accessories
{
    public class SoulCatcher : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.height = 40;
            Item.width = 40;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<SoulPlayer>().IsSoul = true;

            // player.GetModPlayer<SoulPlayer>().manaHealed = false;
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
            base.ResetEffects();
        }

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {

            if (IsSoul && hit.DamageType == DamageClass.Melee && target.AnyInteractions() && !SoulTook)
            {
                SoulTook = true;
                Player.statMana += 6;
                Player.ManaEffect(6);
                base.OnHitNPC(target, hit, damageDone);

                return;
            }
            base.OnHitNPC(target, hit, damageDone);
        }
    }

}


