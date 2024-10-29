using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using Terraria;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Projectiles
{
    public class GlobalProjectileReflectionBlacklist : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public bool Reflected = false;
    }
    public static class ProjectileExtensions
    {
        public static bool ReflectCheck(this Projectile projectile, Projectile target, Predicate<Projectile> predicate)
        {
            return predicate(target);
        }
        public static bool ReflectCheck(this Projectile projectile, Projectile target)
        {
            var predicate = new Predicate<Projectile>(proj => proj.velocity != Vector2.Zero && !proj.GetGlobalProjectile<GlobalProjectileReflectionBlacklist>().Reflected && proj.type != projectile.type && proj.hostile && projectile.Colliding(proj.getRect(), projectile.getRect()));

            return predicate(target);
        }
        public static void ReflectProjectiles(this Projectile projectile)
        {
            Projectile target = null;
            foreach (var proj in Main.ActiveProjectiles)
            {
                if (projectile.ReflectCheck(proj))
                    target = proj;

                if (target != null)
                {
                    target.velocity = -target.velocity;

                    target.GetGlobalProjectile<GlobalProjectileReflectionBlacklist>().Reflected = true;
                }
            }
        }
        public static void ReflectProjectiles(this Projectile projectile, bool friendly, float chance)
        {
            Projectile target = null;
            foreach (var proj in Main.ActiveProjectiles)
            {
                if (projectile.ReflectCheck(proj))
                    target = proj;

                if (target != null)
                {
                    target.GetGlobalProjectile<GlobalProjectileReflectionBlacklist>().Reflected = true;
                    if (new Random().NextDouble() >= chance)
                        return;

                    target.velocity = -target.velocity;

                    if (!friendly)
                        return;

                    if (projectile.type == ModContent.ProjectileType<CircleOfLight>())
                        projectile.ai[0] = 1;

                    target.hostile = false;
                    target.friendly = true;
                    target.reflected = true;
                }
            }
        }
        public static void DrawWithTransparency(this Projectile projectile, Color color, byte alpha)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            Main.instance.GraphicsDevice.BlendState = BlendState.Additive;
            var texture = Terraria.GameContent.TextureAssets.Projectile[projectile.type].Value;
            color.A += alpha;
            var scale = projectile.scale * Main.GameZoomTarget;
            Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, new Rectangle(0, 0, 256, 256), color, 0, texture.Size() / 2, projectile.scale / 2, SpriteEffects.None, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            Main.instance.GraphicsDevice.BlendState = BlendState.AlphaBlend;
        }
        public static void DrawWithTransparency(this Projectile projectile, Color color, byte alpha, int layers, byte layerAlphaStep, float layerScaleStep)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            Main.instance.GraphicsDevice.BlendState = BlendState.Additive;
            var texture = Terraria.GameContent.TextureAssets.Projectile[projectile.type].Value;
            color.A += alpha;
            var scale = projectile.scale * Main.GameZoomTarget;
            for (int i = 0; i < layers; i++)
            {
                color.A += layerAlphaStep;
                scale -= layerScaleStep;

                Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, new Rectangle(0, 0, 256, 256), color, 0, texture.Size() / 2, scale / 2, SpriteEffects.None, 0);
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            Main.instance.GraphicsDevice.BlendState = BlendState.AlphaBlend;
        }
        public static void CenteredOnPlayer(this Projectile projectile)
        {
            Vector2 position = projectile.Center;

            if (Main.player[projectile.owner].active)
                position = Main.player[projectile.owner].Center;

            projectile.Center = position;
        }
        public static void ProjectileRepelling(this Projectile proj)
        {
            foreach (var projectile in Main.ActiveProjectiles)
            {
                if (proj.ReflectCheck(projectile, p => p != null && p.active && p.velocity != Vector2.Zero && p.type != proj.type && p.hostile && p.Colliding(p.getRect(), proj.getRect())))
                    projectile.velocity += projectile.Center.DirectionFrom(proj.Center);
            }
        }
        public static void OrbitingPlayer(this Projectile projectile, float visualRotation, float r, float rotation)
        {
            projectile.rotation = rotation - MathHelper.PiOver2 * visualRotation;

            var desiredPosition = rotation.ToRotationVector2() * r;

            projectile.Center = Main.player[projectile.owner].Center + desiredPosition;
        }
    }
}