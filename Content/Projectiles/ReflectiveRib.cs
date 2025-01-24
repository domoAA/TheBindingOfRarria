using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Items;

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
            Projectile.timeLeft = 60;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
        }
        public override void AI()
        {
            Projectile.ai[0]++;

            var rotation = (MathHelper.TwoPi / 360 * Projectile.ai[0]);
            Projectile.OrbitingPlayer(1.6f, 40, rotation);
            Projectile.ReflectProjectiles(true, 1f);

            //var pull = Projectile.Center.DirectionTo(owner.Center);//(Projectile.velocity.LengthSquared() * 40);
            //Projectile.velocity = pull.RotatedBy(MathHelper.PiOver2) * 3;
        }
    }
}