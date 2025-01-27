using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Projectiles
{
    public class MirrorCrack : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.width = 30;
            Projectile.height = 80;
            Projectile.damage = 0;
            Projectile.netImportant = true;
            Projectile.timeLeft = 180;
        }
        public override void AI()
        {
            if (Projectile.timeLeft > 160)
                Projectile.ReflectProjectiles(true, 1);

            Projectile.rotation = Projectile.ai[0];
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.DrawWithTransparency(lightColor, 150);
            return false;
        }
    }
}