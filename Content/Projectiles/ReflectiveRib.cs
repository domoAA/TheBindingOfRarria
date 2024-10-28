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
            var owner = Main.player[Projectile.owner];

            if (owner.GetModPlayer<SlipperyRibblerPlayer>().IsTheRibbler)
                Projectile.timeLeft = 2;
            Projectile.ai[0]++;

            var rotation = (MathHelper.TwoPi / 360 * Projectile.ai[0]);
            Projectile.rotation = rotation - MathHelper.PiOver2 * 1.6f;
            var desiredPosition = rotation.ToRotationVector2() * 40;
            Projectile.Center = Main.player[Projectile.owner].Center + desiredPosition;
            ReflectingProjectiles();

            //var pull = Projectile.Center.DirectionTo(owner.Center);//(Projectile.velocity.LengthSquared() * 40);
            //Projectile.velocity = pull.RotatedBy(MathHelper.PiOver2) * 3;
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