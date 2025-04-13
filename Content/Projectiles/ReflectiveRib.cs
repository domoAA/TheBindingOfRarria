using Terraria;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Projectiles;

public class ReflectiveRib : ModProjectile
{
    public override string Texture => ContentPath + "Projectiles/" + Name;

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

    public Vector2[] stripe = new Vector2[7];

            // For anyone wondering, the old code looked like this.
                // In my entire life I have never seen a more horrid line of code ever, take this as a warning Kain.

        // public Vector2[] stripe =
        // {
        //     Vector2.Zero,
        //     Vector2.Zero,
        //     Vector2.Zero,
        //     Vector2.Zero,
        //     Vector2.Zero,
        //     Vector2.Zero,
        //     Vector2.Zero
        // };

    public override void AI()
    {
        Projectile.ai[0]++;

        float rotation = TwoPi / 360 * Projectile.ai[0];
        Projectile.OrbitingPlayer(1.6f, 40, rotation);
        Projectile.ReflectProjectiles();

        if (Projectile.ai[0] % 5 == 6)
        {
            Player owner = Main.player[Projectile.owner];

            for (int i = stripe.Length - 1; i > 1; i--)
                stripe[i] = Projectile.Center + Projectile.Center.DirectionTo(owner.Center) * Projectile.Center.Distance(owner.Center) * (i - 1) / stripe.Length + Projectile.Center.DirectionTo(owner.Center).RotatedBy(Main.rand.NextFloat(-Pi / 10, Pi / 10));

            stripe[0] = Projectile.Center;
        }
    }
}