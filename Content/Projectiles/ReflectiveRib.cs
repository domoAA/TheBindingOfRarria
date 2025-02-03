using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
        }
        public override void AI()
        {
            Projectile.ai[0]++;

            var rotation = (MathHelper.TwoPi / 360 * Projectile.ai[0]);
            Projectile.OrbitingPlayer(1.6f, 400, rotation);
            Projectile.ReflectProjectiles(true, 1f);


            //var pull = Projectile.Center.DirectionTo(owner.Center);//(Projectile.velocity.LengthSquared() * 40);
            //Projectile.velocity = pull.RotatedBy(MathHelper.PiOver2) * 3;
        }
    }
}