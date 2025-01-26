using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace TheBindingOfRarria.Content.Projectiles
{
    public class SaturnusRing : ModProjectile
    {
        public bool? shining = null;
        public float chance = 0.16f;
        public override void SetDefaults()
        {
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.width = 116;
            Projectile.height = 116;
            Projectile.scale = 2.5f;
        }
        public override void AI()
        {
            Projectile.CenteredOnPlayer();

            foreach (var proj in Main.ActiveProjectiles)
            {
                if (!CheckOrbit(proj))
                    continue;

                if (proj.GetGlobalProjectile<OrbitalDebrisProjectile>().FollowingARing != false || proj.GetGlobalProjectile<OrbitalDebrisProjectile>().Orbiting == true)
                    continue;

                if (Main.rand.NextFloat() < chance)
                {
                    proj.hostile = false;
                    proj.friendly = true;
                    proj.owner = Projectile.owner;
                    proj.GetGlobalProjectile<OrbitalDebrisProjectile>().FollowingARing = true;
                    proj.GetGlobalProjectile<OrbitalDebrisProjectile>().Orbiting = true;
                    proj.GetGlobalProjectile<OrbitalDebrisProjectile>().SaturnusRing = Projectile;
                }
                else
                    proj.GetGlobalProjectile<OrbitalDebrisProjectile>().FollowingARing = null;
            }
            Projectile.netUpdate = true;
        }
        public bool CheckOrbit(Projectile projectile)
        {
            if (Projectile.Colliding(Projectile.getRect(), projectile.getRect()))
                return true;

            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.ai[0] != 0)
                shining = true;

            Projectile.ai[0] = 0;

            Projectile.ai[1] += shining == true ? 1f : shining == false ? -0.5f : 0;

            if (Projectile.ai[1] > 20)
                shining = false;
            else if (Projectile.ai[1] <= 0)
                shining = null;

            byte alpha = (byte)(15 + Projectile.ai[1]);

            Projectile.DrawWithTransparency(Color.LightGray, alpha);
            return false;
        }
    }
}