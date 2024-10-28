using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using System;

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
        }
        public enum State
        {
            Contracting,
            Expanding
        }
        public State state = State.Expanding;
        public override void AI()
        {
            Projectile.timeLeft++;
            Projectile.Center = Main.player[Projectile.owner].Center;

            var rand = Main.rand.NextFloat() * 1.5f + 0.1f;
            Projectile.ai[0] += state == State.Expanding ? 0.01f * rand : -0.01f * rand;
            if (Projectile.ai[0] > 1)
                state = State.Contracting;
            else if (Projectile.ai[0] < 0.5f)
                state = State.Expanding;

            Projectile.scale = Projectile.ai[0];
            Projectile.width = (int)(100 * Projectile.scale);
            Projectile.height = (int)(100 * Projectile.scale);
            ProjectileRepelling();
        }
        public void ProjectileRepelling()
        {
            var proj = Array.Find(Main.projectile, projectile => projectile.active && projectile.velocity != Vector2.Zero && projectile.type != Projectile.type && projectile.hostile && projectile.active && Projectile.Colliding(Projectile.getRect(), projectile.getRect()));
            if (proj != null)
                proj.velocity += proj.Center.DirectionFrom(Projectile.Center);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            Main.instance.GraphicsDevice.BlendState = BlendState.Additive;
            var texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            var color = Color.DeepSkyBlue;
            color.A += 150;
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, 256, 256), color, 0, texture.Size() / 2, Projectile.scale / 2, SpriteEffects.None, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            Main.instance.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            return false;
        }
    }
}