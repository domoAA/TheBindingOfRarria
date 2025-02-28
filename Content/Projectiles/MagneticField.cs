using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using System;

namespace TheBindingOfRarria.Content.Projectiles
{
    public class MagneticField : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.width = 200;
            Projectile.height = 200;
        }
        public override void AI()
        {
            Projectile.scale = Projectile.ai[0];
            Projectile.width = (int)(200 * Projectile.scale);
            Projectile.height = (int)(200 * Projectile.scale);

            foreach (var projectile in Main.ActiveProjectiles)
            {
                if (Projectile.ReflectCheck(projectile, p => p != null && p.CanBeReflected() && p.Colliding(p.getRect(), Projectile.getRect())))
                {
                    projectile.velocity += projectile.Center.DirectionTo(Projectile.Center) * 2 * Projectile.ai[1];
                    projectile.netUpdate = true;
                }
            }

            Lighting.AddLight(Projectile.Center, Color.DeepSkyBlue.ToVector3() * Projectile.scale / 4);
            Projectile.netUpdate = true;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            var color = Color.SlateGray;
            Projectile.DrawWithTransparency(color, 100);
            return false;
        }
    }
}