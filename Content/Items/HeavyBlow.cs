using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Projectiles;

namespace TheBindingOfRarria.Content.Items
{
    public class HeavyBlow : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.height = 30;
            Item.width = 30;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<KnockbackAccessoryPlayer>().KnockbackItem = Item;
        }
    }
    public class KnockbackAccessoryPlayer : ModPlayer
    {
        public Item KnockbackItem = null;
        public override void ResetEffects()
        {
            KnockbackItem = null;
        }
        public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (KnockbackItem != null)
            {
                modifiers.Knockback += 1.5f;
                modifiers.FinalDamage += modifiers.Knockback.ApplyTo(0.67f);

                var position = target.Center + target.Center.DirectionTo(Player.Center) * target.Hitbox.Size() / 2;
                Projectile.NewProjectile(Player.GetSource_Accessory(KnockbackItem), position, Player.Center.DirectionTo(position), ModContent.ProjectileType<HeavyBlowThing>(), 0, 0, Player.whoAmI);
            }
            base.ModifyHitNPCWithItem(item, target, ref modifiers);
        }
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (KnockbackItem != null)
            {
                modifiers.Knockback += 1.5f;
                modifiers.FinalDamage += modifiers.Knockback.ApplyTo(0.67f);

                var direction = target.Center + proj.velocity / proj.velocity.Length();
                var position = proj.Center + proj.Center.DirectionTo(target.Center) * proj.Hitbox.Size() / 2;
                if (proj.aiStyle == ProjAIStyleID.Flail || proj.aiStyle == ProjAIStyleID.SolarEffect || position.Distance(direction) > 250 || proj.aiStyle == ProjAIStyleID.Whip || proj.velocity.LengthSquared() < 1)
                {
                    direction = target.Center.DirectionFrom(proj.Center);
                    position = target.Center + target.Center.DirectionTo(proj.Center) * target.Hitbox.Size() / 2;
                }
                Projectile.NewProjectile(Player.GetSource_Accessory(KnockbackItem), position, direction, ModContent.ProjectileType<HeavyBlowThing>(), 0, 0, proj.owner);
            }
            base.ModifyHitNPCWithProj(proj, target, ref modifiers);
        }
    }
}