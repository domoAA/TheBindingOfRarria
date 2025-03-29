// MISSING: projectile picture?
// Putting Bass picture...

namespace TheBindingOfRarria.Content.Projectiles
{
    public class FlippityFloppity : ModProjectile
    {
        List<int> fishID = new();
        Texture2D fish;

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.scale = .6f;
            Projectile.timeLeft = 300;
            Projectile.friendly = true;
            Projectile.penetrate = -1;

            // Fish texture List
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
            Projectile.ai[1] += .4f;


            // Caps X velocity
            if (Projectile.velocity.X * Projectile.direction > 3f)
                Projectile.velocity.X = Projectile.direction * 3f;

            // // Caps Y velocity
            if (Projectile.velocity.Y < -3f)
                Projectile.velocity.Y = -3f;
            if (Projectile.velocity.Y > 10f)
                Projectile.velocity.Y = 10f;

            // The flop...
            if (Projectile.direction == 1)
                Projectile.rotation = (float)Math.Sin(Projectile.ai[1]) / 2;
            if (Projectile.direction == -1)
                Projectile.rotation = (float)Math.Sin(Projectile.ai[1]) / 2 + Pi;

            Projectile.velocity.Y += 0.2f;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.timeLeft == 0)
                Projectile.Kill();

            if (Math.Abs(Projectile.velocity.X - oldVelocity.X) > float.Epsilon)
                Projectile.velocity.X = -oldVelocity.X;

            if (Math.Abs(Projectile.velocity.Y - oldVelocity.Y) > float.Epsilon)
            {
                Projectile.velocity.Y = -oldVelocity.Y;
                if (Main.rand.NextBool(3))
                    Projectile.velocity.X = -oldVelocity.X;
            }

            return false;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCDeath28, Projectile.position);
            for (int i = 0; i < 5; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.GreenBlood);
                dust.noGravity = true;
                dust.velocity *= 1.5f;
                dust.scale *= 1.5f;
            }
            base.OnKill(timeLeft);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.EntitySpriteDraw(fish, Projectile.Center - Main.screenPosition, fish.Bounds, lightColor, Projectile.rotation + (Projectile.direction * PiOver4), fish.Size() / 2, 1, (SpriteEffects)(1 - Projectile.direction));
            return false;
        }
    }
}