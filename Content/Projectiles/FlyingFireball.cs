using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Projectiles
{
    public class FlyingFireball : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.timeLeft = 3;
            Projectile.DamageType = DamageClass.Generic;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = Color.Orange;
            return base.PreDraw(ref lightColor);
        }
        public override void AI()
        {
            if (Projectile.ai[0] != 0)
                Projectile.timeLeft = (int)(Projectile.timeLeft * Projectile.ai[0]);

            Projectile.ai[0] = 0;

            if (Projectile.wet)
                Projectile.Kill();

            var dust = ModContent.DustType<FireDust>();
            for (int i = 2; i > 0; i--) {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dust, Projectile.velocity.X, Projectile.velocity.Y, 0, Color.White, 2f); }

            RotateConstantly();
        }
        public void RotateConstantly()
        {
            Projectile.ai[1]++;
            if (Projectile.ai[1] >= 360)
                Projectile.ai[1] -= 360;
            Projectile.rotation = MathHelper.TwoPi / 60 * Projectile.ai[1];
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
            target.AddBuff(Terraria.ID.BuffID.OnFire, 60);
        }
    }
    public class FireDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
        }
    }
}