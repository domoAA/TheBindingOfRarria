using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Items;
using TheBindingOfRarria.Content.Dusts;

namespace TheBindingOfRarria.Content.Projectiles
{
    public class FireExplotaro : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Default;
            Projectile.width = 160;
            Projectile.height = 160;
            Projectile.friendly = true;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 30;
            for (int i = 9; i > 0; i--)
            {
                bomba[i] = Vector2.One.RotatedBy(MathHelper.PiOver4 * (i + 1) + Main.rand.NextFloat(-MathHelper.Pi / 10, MathHelper.Pi / 10));
            }
        }
        public bool exploded = false;
        public override void AI()
        {
            if (Projectile.ai[0] < 4)
                Projectile.ai[0] += 0.3f;
            else
                Projectile.Kill();

            if (exploded)
                return;

            exploded = true;
            var color = Color.OrangeRed;

            Projectile.Center.SpawnDust(bomba, ModContent.DustType<PixellatedDustE98>(), 7, 0.45f * Main.rand.NextFloat(1.12f, 2.1f), color, 5, -0.05f, MathHelper.PiOver4);

            var sound = SoundID.Item14;
            sound.Pitch += 0.7f;
            sound.Volume *= 0.5f;
            SoundEngine.PlaySound(sound, Projectile.Center);
        }
        public Vector2[] bomba = new Vector2 [10];
        public override bool PreDraw(ref Color lightColor)
        {
            //var num = (int)Projectile.ai[0];
            //var texture = Projectile.MyTexture();
            //var frame = texture.Frame(1, 5, 0, num, 0, -2);
            //Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + new Vector2(0, frame.Height + 2) * num, frame, lightColor, 0, frame.Center(), Projectile.scale * Main.GameZoomTarget, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
            
            return false;
        }
    }
}