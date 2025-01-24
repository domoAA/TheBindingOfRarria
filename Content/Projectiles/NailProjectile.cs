using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Projectiles
{
    
    public class NailProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.height = 10;
            Projectile.width = 10;
            Projectile.friendly = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.timeLeft = 500;
        }
        public enum State
        {
            Flying,
            Sticking
        }
        public State state = State.Flying;
        public NPC Target = null;
        public Vector2 offset;
        public override void AI()
        {
            if (Target != null && !Projectile.Colliding(Projectile.Hitbox, Target.Hitbox))
                state = State.Flying;

            if (state == State.Flying) {
                Projectile.velocity *= 0.98f;
                Projectile.velocity.Y += 0.01f * (500 - Projectile.timeLeft);
                Projectile.rotation = Projectile.velocity.ToRotation();
                return; }

            if (Target == null || !Target.active)
                Projectile.Kill();

            Projectile.Center = Target.Center - offset;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.penetrate == 1 && state == State.Flying) {
                state = State.Sticking;
                Projectile.penetrate = 3;
                Target = target; 
                offset = (target.Center - Projectile.Center);
                Projectile.velocity = Projectile.oldVelocity / 2;
            }
        }
        public override bool ShouldUpdatePosition()
        {
            return state == State.Flying;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            var texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, texture.Bounds, lightColor, Projectile.rotation, texture.Size() / 2, Projectile.scale * Main.GameZoomTarget, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
            return false;
        }
    }
}