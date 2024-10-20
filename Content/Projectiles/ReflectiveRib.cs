using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Projectiles
{
    public class ReflectiveRib : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Default;
        }
        public override void AI()
        {
            Projectile.ai[0]++;
            var desiredPosition = (MathHelper.TwoPi / 360 * Projectile.ai[0]).ToRotationVector2() * 30;
            Projectile.Center = desiredPosition;
            ReflectingProjectiles();
        }
        public void ReflectingProjectiles()
        {
            var target = Array.Find(Main.projectile, proj => proj.active && proj.hostile && proj.Colliding(proj.getRect(), Projectile.getRect()));
            if (target != null)
            {
                target.velocity = -target.velocity;
                target.hostile = false;
                target.friendly = true;
                target.reflected = true;
            }
        }
    }
}