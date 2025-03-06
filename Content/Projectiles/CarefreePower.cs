using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Items;
using TheBindingOfRarria.Content.Dusts;

namespace TheBindingOfRarria.Content.Projectiles
{
    public class CarefreePower : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.width = 128;
            Projectile.height = 128;
            Projectile.timeLeft = 120;
            Projectile.netImportant = true;

            for (int i = 19; i > 0; i--)
            {
                edges[i] = Vector2.One.RotatedBy(MathHelper.PiOver4 * (i + 1) + Main.rand.NextFloat(-MathHelper.Pi / 20, MathHelper.Pi / 20));
            }
        }
        public bool exploded = false;
        public Vector2[] edges = new Vector2[20];
        public override void AI()
        {
            if (Projectile.ai[0] < 5)
                Projectile.ai[0] += 0.3f;
            else
                Projectile.Kill();

            if (exploded)
                return;

            exploded = true;
            var color = Color.Red;

            Projectile.Center.SpawnDust(edges, ModContent.DustType<PixellatedDustE98>(), 1, 0.95f * Main.rand.NextFloat(1.12f, 2.2f), color, 5, -0.09f, MathHelper.PiOver4, 96);

            var sound = SoundID.Item74;
            sound.Pitch += 0.7f;
            sound.Volume *= 0.5f;
            SoundEngine.PlaySound(sound, Projectile.Center);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.scale = 0.75f;
            Projectile.DrawWithTransparency(Color.Red, (byte)(40 * Projectile.ai[0]));
            return false;
        }
    }
}