// MISSING: projectile picture?
// Putting Bass picture...

namespace TheBindingOfRarria.Content.Projectiles
{

    public class FishBullet : ModProjectile
    {
        List<int> fishID = new();
        Texture2D fish;
        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.scale = .6f;
            Projectile.friendly = true;
            Projectile.extraUpdates = 1;
            Projectile.timeLeft = 600;

            Projectile.DamageType = DamageClass.Ranged;

            // All fishes
            for (int i = 2297; i <= 2321; i++)
            {
                fishID.Add(i);
            }
            for (int j = 2450; j <= 2488; j++)
            {
                fishID.Add(j);
            }
            fishID.Add(2290);
            fishID.Add(4401);
            fishID.Add(4402);

            if (fish == null)
            {
                int index = Main.rand.Next(fishID);
                fish = TextureAssets.Item[index].Value;
            }

        }

        public override void AI()
        {
            Projectile.ai[0] += 1f;
            Projectile.rotation = Projectile.velocity.ToRotation();

            if (Projectile.ai[0] > 60)
            {
                Projectile.ai[0] = 60;
                Projectile.velocity.Y += 0.05f;
            }

            if (Projectile.timeLeft == 0)
                Projectile.Kill();
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
            for (int i = 0; i < 5; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Silver);
                dust.noGravity = true;
                dust.velocity *= 1.5f;
                dust.scale *= 0.9f;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.EntitySpriteDraw(fish, Projectile.Center - Main.screenPosition, fish.Bounds, lightColor, Projectile.rotation + (Projectile.direction * PiOver4), fish.Size() / 2, 1, (SpriteEffects)(1 - Projectile.direction), 0);
            return false;
        }
    }
}