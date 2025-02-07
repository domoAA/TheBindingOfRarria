using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using TheBindingOfRarria.Content.Projectiles;

namespace TheBindingOfRarria.Content.Items
{
    public class CarefreeMelody : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 34;
            Item.height = 30;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<GrimmTroupeBanisherPlayer>().Melody = Item;
        }
    }
    public class GrimmTroupeBanisherPlayer : ModPlayer
    {
        public int Hits = 0;
        public Item Melody = null;
        public override void ResetEffects()
        {
            Melody = null;
        }
        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
        {
            if (Melody != null)
            {
                Hits++;
                if (Hits >= 6)
                {
                    Hits = 0;
                    modifiers.Cancel();
                    Projectile.NewProjectile(Player.GetSource_Accessory_OnHurt(Melody, modifiers.DamageSource), Player.Center, Vector2.Zero, ModContent.ProjectileType<CarefreePower>(), 0, 0, Player.whoAmI);
                }
            }
            base.ModifyHitByNPC(npc, ref modifiers);
        }
        public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
        {
            if (Melody != null)
            {
                Hits++;
                if (Hits >= 6)
                {
                    Hits = 0;
                    modifiers.Cancel();
                    Projectile.NewProjectile(Player.GetSource_Accessory_OnHurt(Melody, modifiers.DamageSource), Player.Center, Vector2.Zero, ModContent.ProjectileType<CarefreePower>(), 0, 0, Player.whoAmI);
                }
            }
            base.ModifyHitByProjectile(proj, ref modifiers);
        }
    }
}