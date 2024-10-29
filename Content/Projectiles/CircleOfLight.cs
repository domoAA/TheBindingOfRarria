using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace TheBindingOfRarria.Content.Projectiles
{
    public class CircleOfLight : ModProjectile
    {
        public bool? shining = null;
        public override void SetDefaults()
        {
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.width = 60;
            Projectile.height = 60;
        }
        public override void AI()
        {
            Projectile.timeLeft = 3;
            Projectile.ReflectProjectiles(true, 0.3f);
            Projectile.CenteredOnPlayer();
            Projectile.width = (int)(110 * Projectile.scale);
            Projectile.height = (int)(110 * Projectile.scale);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.ai[0] != 0)
                shining = true;

            Projectile.ai[0] = 0;

            Projectile.ai[1] += shining == true ? 1f : shining == false ? -0.5f : 0;

            if (Projectile.ai[1] > 13)
                shining = false;
            else if (Projectile.ai[1] <= 0)
                shining = null;

            byte alpha = (byte)(7 + Projectile.ai[1]);

            Projectile.DrawWithTransparency(Color.LightYellow, alpha, 5, 5, 0.015f);
            return false;
        }
    }
}