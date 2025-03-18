
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
            Projectile.friendly = true;
            Projectile.netImportant = true;
            Projectile.light = 0.4f;
        }
        public override void AI()
        {
            Projectile.width = (int)(110 * Projectile.scale);
            Projectile.height = (int)(110 * Projectile.scale);

            Projectile.CenteredOnPlayer();
            Projectile.ReflectProjectiles(0.12f);

            if (Projectile.ai[0] != 0)
                shining = true;

            Projectile.ai[0] = 0;

            Projectile.ai[1] += shining == true ? 1f : shining == false ? -0.5f : 0;

            if (Projectile.ai[1] > 13)
                shining = false;
            else if (Projectile.ai[1] <= 0)
                shining = null;

            Projectile.netUpdate = true;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            byte alpha = (byte)(7 + Projectile.ai[1]);

            Projectile.DrawWithTransparency(new Rectangle(0, 0, 256, 256), Color.LightYellow, alpha, 6, 3, 0.015f);
            return false;
        }
    }
}