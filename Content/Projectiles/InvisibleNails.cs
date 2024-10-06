using Terraria.ModLoader;
using Terraria;

namespace TheBindingOfRarria.Content.Projectiles
{
    public class InvisibleNails : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Default;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.width = 20;
            Projectile.height = 42;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.alpha = 255;
            Projectile.timeLeft = 60;
            Projectile.friendly = true;
        }
        public override void AI()
        {
            if (Projectile.ai[1] != 0)
            {
                Projectile.timeLeft += (int)Projectile.ai[0];
                Projectile.ai[0] = 0;
            }
            Projectile.Center = Main.player[Projectile.owner].Center;
        }
    }
}