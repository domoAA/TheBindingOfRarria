using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace TheBindingOfRarria.Content.Projectiles
{
    public class RepellingPulse : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.timeLeft = 3;
            Projectile.friendly = true;
        }
        public enum State
        {
            Contracting,
            Expanding
        }
        public State state = State.Expanding;
        public override void AI()
        {
            if (!Projectile.hostile)
                Projectile.CenteredOnPlayer();

            var rand = Main.rand.NextFloat() * 1.5f + 0.1f;
            Projectile.ai[0] += state == State.Expanding ? 0.01f * rand : -0.01f * rand;
            if (Projectile.ai[0] > 1)
                state = State.Contracting;
            else if (Projectile.ai[0] < 0.5f)
                state = State.Expanding;

            Projectile.scale = Projectile.ai[0];
            Projectile.width = (int)(100 * Projectile.scale);
            Projectile.height = (int)(100 * Projectile.scale);
            Projectile.ProjectileRepelling();

            Lighting.AddLight(Projectile.Center, Color.DeepSkyBlue.ToVector3() * Projectile.scale / 2);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.DrawWithTransparency(Color.DarkBlue, 100);
            Lighting.AddLight(Projectile.Center, Color.DeepSkyBlue.ToVector3() * Projectile.ai[0] / 3);
            return false;
        }
    }
}