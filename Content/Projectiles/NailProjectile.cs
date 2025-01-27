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
        public NPC Target;
        public Vector2 offset;
        public override void AI()
        {
            if (Target != null && !Projectile.Colliding(Projectile.Hitbox, Target.Hitbox))
                Projectile.ai[0] = 0;

            if (Projectile.ai[0] == 0)
            {
                Projectile.velocity *= 0.98f;
                Projectile.velocity.Y += 0.01f * (500 - Projectile.timeLeft);
                Projectile.rotation = Projectile.velocity.ToRotation();
                return;
            }

            if (Target == null || !Target.active)
                Projectile.Kill();

            if (Main.myPlayer == Projectile.owner) {
                Projectile.Center = Target.Center - new Vector2(Projectile.ai[1], Projectile.ai[2]);
                Projectile.netUpdate = true;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.penetrate == 1 && Projectile.ai[0] == 1) {
                Projectile.ai[0] = -1;
                Projectile.penetrate = 3;
                Target = target; 
                offset = (target.Center - Projectile.Center);
                Projectile.ai[1] = offset.X;
                Projectile.ai[2] = offset.Y;
                Projectile.velocity = Projectile.oldVelocity / 2;
                Projectile.netUpdate = true;
            }
        }
        public override bool ShouldUpdatePosition()
        {
            return Projectile.ai[0] != -1;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            var texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, texture.Bounds, lightColor, Projectile.rotation, texture.Size() / 2, Projectile.scale * Main.GameZoomTarget, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
            return false;
        }
    }
}