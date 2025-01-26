using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace TheBindingOfRarria.Content.Projectiles
{
    public class SlowingAura : ModProjectile
    {
        public bool shining = true;
        public override void SetDefaults()
        {
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.width = 110;
            Projectile.height = 110;
            Projectile.timeLeft = 3;
        }
        public override void AI()
        {
            Projectile.width = (int)(110 * Projectile.scale);
            Projectile.height = (int)(110 * Projectile.scale);

            foreach (var proj in Main.ActiveProjectiles)
            {
                if (proj == null || proj.friendly)
                    continue;

                if (proj.Colliding(proj.getRect(), Projectile.getRect()))
                    proj.GetGlobalProjectile<SlowedGlobalProjectile>().Slowed = (true, 180);
            }

            foreach (var target in Main.ActiveNPCs)
            {
                if (target == null || target.friendly)
                    continue;

                if (Projectile.Colliding(target.getRect(), Projectile.getRect()))
                    target.GetGlobalNPC<NPCExtensions.SlowedGlobalNPC>().Slowed = (true, 60);
            }

            Projectile.CenteredOnPlayer();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            switch(Projectile.ai[1])
            {
                case > 7: 
                    shining = false;
                    Projectile.ai[1] = 6.9f;
                    break;
                case < -7:
                    shining = true;
                    Projectile.ai[1] = -6.9f;
                    break;
                default:
                    if (shining)
                        Projectile.ai[1] += 0.1f;
                    else
                        Projectile.ai[1] -= 0.1f;
                    break;
            }

            byte alpha = (byte)(7 + Projectile.ai[1]);

            Projectile.scale = 2.5f;
            Projectile.DrawWithTransparency(new Rectangle(0, 0, 256, 256), Color.LightYellow, alpha, 8, 2, 0.035f);
            return false;
        }
    }
}