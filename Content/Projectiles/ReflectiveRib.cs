
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
        public Vector2[] stripe =
        {
            Vector2.Zero,
            Vector2.Zero,
            Vector2.Zero,
            Vector2.Zero,
            Vector2.Zero,
            Vector2.Zero,
            Vector2.Zero
        };
        public override void AI()
        {
            Projectile.ai[0]++;

            var rotation = (MathHelper.TwoPi / 360 * Projectile.ai[0]);
            Projectile.OrbitingPlayer(1.6f, 40, rotation);
            Projectile.ReflectProjectiles();

            if (Projectile.ai[0] % 5 == 6)
            {
                var owner = Main.player[Projectile.owner];
                for (int i = stripe.Length - 1; i > 1; i--)
                {
                    stripe[i] = Projectile.Center + Projectile.Center.DirectionTo(owner.Center) * Projectile.Center.Distance(owner.Center) * (i - 1) / stripe.Length + Projectile.Center.DirectionTo(owner.Center).RotatedBy(Main.rand.NextFloat(-MathHelper.Pi / 10, MathHelper.Pi / 10));
                }
                stripe[0] = Projectile.Center;
            }

            //var pull = Projectile.Center.DirectionTo(owner.Center);//(Projectile.velocity.LengthSquared() * 40);
            //Projectile.velocity = pull.RotatedBy(MathHelper.PiOver2) * 3;
        }
    }
}