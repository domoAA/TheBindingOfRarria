using Terraria;
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
            Projectile.timeLeft = 60;
            Projectile.DamageType = DamageClass.Generic;
        }
        public override void AI()
        {
            if (Projectile.ai[0] != 0)
                Projectile.timeLeft = (int)(Projectile.timeLeft * Projectile.ai[0]);

            Projectile.ai[0] = 0;

            if (Projectile.wet)
                Projectile.Kill();

            for (int i = 3; i > 0; i--) {
                Dust.NewDust(Projectile.position, Projectile.width * 2, Projectile.height * 2, Terraria.ID.DustID.FlameBurst); }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
            target.AddBuff(Terraria.ID.BuffID.OnFire, 60);
        }
    }
}