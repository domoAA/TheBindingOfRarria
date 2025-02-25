using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using TheBindingOfRarria.Content.Buffs;

namespace TheBindingOfRarria.Content.Items
{
    public class Backstab : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 26;
            Item.accessory = true;
            // rarity & value
            // & recipe/whatever
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<BackshotsPlayer>().Rat = true;
        }
    }
    public class BackshotsPlayer : ModPlayer
    {
        public bool Rat = false;
        public override void ResetEffects()
        {
            Rat = false;
        }
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            var dir = proj.Center.DirectionTo(target.Center).ToRotation();
            if (dir < target.direction * MathHelper.PiOver2 + MathHelper.PiOver4 && dir > target.direction * MathHelper.PiOver2 - MathHelper.PiOver4)
            {
                modifiers.ScalingBonusDamage += 1;
                target.AddBuff(ModContent.BuffType<BleedDOT>(), 300);
            }
            else
                base.ModifyHitNPCWithProj(proj, target, ref modifiers);
        }
    }
}