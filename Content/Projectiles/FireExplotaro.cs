using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Items;

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
            Projectile.width = 150;
            Projectile.height = 150;
            Projectile.friendly = true;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 30;
        }
        public override void AI()
        {
            if (Projectile.ai[0] < 4)
                Projectile.ai[0] += 0.3f;
            else
                Projectile.Kill();

            if (Projectile.ai[0] != 0.3f)
                return;

            var color = Color.OrangeRed;
            for (int i = bomba.Length - 1; i > 0; i--)
            {
                var rot = Main.rand.NextFloat(-MathHelper.Pi / 8, MathHelper.Pi / 8);
                Dust.NewDustPerfect(Projectile.Center + bomba[i], ModContent.DustType<PixelatedDustParticle>(), bomba[i] * 6.5f, 255, color, 0.3f * Main.rand.NextFloat(1f, 2.5f));
                Dust.NewDustPerfect(Projectile.Center + bomba[i].RotatedBy(rot), ModContent.DustType<PixelatedDustParticle>(), bomba[i].RotatedBy(rot) * 5.8f, 255, color, 0.15f * Main.rand.NextFloat(1.12f, 2.35f));
                rot = Main.rand.NextFloat(-MathHelper.Pi / 8, MathHelper.Pi / 8);
                Dust.NewDustPerfect(Projectile.Center + bomba[i].RotatedBy(rot), ModContent.DustType<PixelatedDustParticle>(), bomba[i].RotatedBy(rot) * 5.6f, 255, color, 0.26f * Main.rand.NextFloat(0.9f, 2.25f));
                rot = Main.rand.NextFloat(-MathHelper.Pi / 7, MathHelper.Pi / 8);
                Dust.NewDustPerfect(Projectile.Center + bomba[i].RotatedBy(rot), ModContent.DustType<PixelatedDustParticle>(), bomba[i].RotatedBy(rot) * 6.6f, 255, color, 0.2f * Main.rand.NextFloat(0.95f, 2.45f));
                rot = Main.rand.NextFloat(-MathHelper.Pi / 6, MathHelper.Pi / 8);
                Dust.NewDustPerfect(Projectile.Center + bomba[i].RotatedBy(rot), ModContent.DustType<PixelatedDustParticle>(), bomba[i].RotatedBy(rot) * 5.2f, 255, color, 0.32f * Main.rand.NextFloat(0.8f, 2.3f));
            }
            var sound = SoundID.Item14;
            sound.Pitch += 0.7f;
            sound.Volume *= 0.5f;
            SoundEngine.PlaySound(sound, Projectile.Center);
        }
        public Vector2[] bomba =
        {
            Vector2.One.RotatedBy(MathHelper.PiOver4 + Main.rand.NextFloat(-MathHelper.Pi / 10, MathHelper.Pi / 10)),
            Vector2.One.RotatedBy(MathHelper.PiOver4 * 2 + Main.rand.NextFloat(-MathHelper.Pi / 10, MathHelper.Pi / 10)),
            Vector2.One.RotatedBy(MathHelper.PiOver4 * 3 + Main.rand.NextFloat(-MathHelper.Pi / 10, MathHelper.Pi / 10)),
            Vector2.One.RotatedBy(MathHelper.PiOver4 * 4 + Main.rand.NextFloat(-MathHelper.Pi / 10, MathHelper.Pi / 10)),
            Vector2.One.RotatedBy(MathHelper.PiOver4 * 5 + Main.rand.NextFloat(-MathHelper.Pi / 10, MathHelper.Pi / 10)),
            Vector2.One.RotatedBy(MathHelper.PiOver4 * 6 + Main.rand.NextFloat(-MathHelper.Pi / 10, MathHelper.Pi / 10)),
            Vector2.One.RotatedBy(MathHelper.PiOver4 * 7 + Main.rand.NextFloat(-MathHelper.Pi / 10, MathHelper.Pi / 10)),
            Vector2.One.RotatedBy(MathHelper.PiOver4 * 8 + Main.rand.NextFloat(-MathHelper.Pi / 10, MathHelper.Pi / 10)),
            Vector2.One.RotatedBy(MathHelper.PiOver4 * 9 + Main.rand.NextFloat(-MathHelper.Pi / 10, MathHelper.Pi / 10)),
            Vector2.One.RotatedBy(MathHelper.PiOver4 * 10 + Main.rand.NextFloat(-MathHelper.Pi / 10, MathHelper.Pi / 10))
        };
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