using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Graphics.Shaders;
using System.Diagnostics;

namespace TheBindingOfRarria.Content.Projectiles
{
    public class GlobalProjectileReflectionBlacklist : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public bool Reflected = false;
        public override bool PreDraw(Projectile projectile, ref Color lightColor)
        {
            if (projectile.reflected)
                lightColor.A = 70;

            return base.PreDraw(projectile, ref lightColor);
        }
    }
    public class SlowedGlobalProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public (bool, int) Slowed = (false, 0);
        public override void PostAI(Projectile projectile)
        {
            if (Slowed.Item1)
                projectile.velocity *= 0.97f;
            projectile.netUpdate = true;
        }
        public override bool PreDraw(Projectile projectile, ref Color lightColor)
        {
            if (Slowed.Item1 == true)
                lightColor.A = 130;

            Slowed.Item2--;
            if (Slowed.Item2 <= 0)
                Slowed.Item1 = false;

            return base.PreDraw(projectile, ref lightColor);
        }
    }
    public static class ProjectileExtensions
    {
        public static Texture2D MyTexture(this Projectile projectile) 
        { 
            return Terraria.GameContent.TextureAssets.Projectile[projectile.type].Value;
        }
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
            foreach (var proj in Main.ActiveProjectiles)
            {
                Projectile target = null;
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
            foreach (var proj in Main.ActiveProjectiles)
            {
                Projectile target = null;
                if (projectile.ReflectCheck(proj))
                    target = proj;

                if (target != null && Main.myPlayer == projectile.owner)
                {
                    target.GetGlobalProjectile<GlobalProjectileReflectionBlacklist>().Reflected = true;

                    var reflected = Main.rand.NextFloat() < chance;
                    Console.WriteLine(reflected); // 
                    ModPacket packet = ModContent.GetInstance<TheBindingOfRarria>().GetPacket();
                    packet.Write("ProjectileReflection");
                    packet.Write(reflected);
                    packet.Write(target.identity);
                    packet.Write(friendly);
                    packet.Send();

                    if (!reflected)
                        return;

                    if (projectile.type == ModContent.ProjectileType<CircleOfLight>())
                        projectile.ai[0] = 1;
                    target.GetReflected(friendly);
                }
            }
        }
        public static void GetReflected(this Projectile projectile, bool friendly)
        {
            Console.WriteLine("GetReflected"); // 
            projectile.velocity = -projectile.velocity;
            projectile.GetGlobalProjectile<GlobalProjectileReflectionBlacklist>().Reflected = true;
            if (friendly)
            {
                projectile.hostile = false;
                projectile.friendly = true;
                projectile.reflected = true;
            }
            projectile.netUpdate = true;
        }
        public static void DrawElectricity(this Projectile projectile, Vector4[] points, float width, Color color)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
            Main.instance.GraphicsDevice.BlendState = BlendState.Additive;

            var texture = Terraria.GameContent.TextureAssets.Projectile[projectile.type].Value;
            var scale = projectile.scale * Main.GameZoomTarget;

            GameShaders.Misc["electricity"].Shader.Parameters["points"].SetValue(points);
            GameShaders.Misc["electricity"].Shader.Parameters["width"].SetValue(width);
            GameShaders.Misc["electricity"].Shader.Parameters["dimensions"].SetValue(texture.Size());
            GameShaders.Misc["electricity"].UseColor(color);
            GameShaders.Misc["electricity"].Shader.CurrentTechnique.Passes[0].Apply();

            Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, texture.Bounds, color, 0, texture.Size() / 2, scale / 2, SpriteEffects.None, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            Main.instance.GraphicsDevice.BlendState = BlendState.AlphaBlend;
        }
        public static void DrawWithTransparency(this Projectile projectile, Color color, byte alpha)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            Main.instance.GraphicsDevice.BlendState = BlendState.Additive;
            var texture = Terraria.GameContent.TextureAssets.Projectile[projectile.type].Value;
            var scale = projectile.scale * Main.GameZoomTarget;
            color.A += alpha;
            Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition - new Vector2(Main.screenWidth / 2, Main.screenHeight / 2) * Main.GameZoomTarget + new Vector2(Main.screenWidth / 2, Main.screenHeight / 2), texture.Bounds, color, 0, texture.Size() / 2, scale / 2, SpriteEffects.None, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            Main.instance.GraphicsDevice.BlendState = BlendState.AlphaBlend;
        }
        public static void DrawWithTransparency(this Projectile projectile, Rectangle rect, Color color, byte alpha, int layers, byte layerAlphaStep, float layerScaleStep)
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

                Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, rect, color, projectile.rotation, texture.Size() / 2, scale / 2, SpriteEffects.None, 0);
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            Main.instance.GraphicsDevice.BlendState = BlendState.AlphaBlend;
        }
        public static void DrawWithTransparency(this Projectile projectile, Vector2 drawOffset, Rectangle rect, Color color, byte alpha, int layers, byte layerAlphaStep, float layerScaleStep)
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

                Main.spriteBatch.Draw(texture, (projectile.Center + drawOffset - Main.screenPosition - new Vector2(Main.screenWidth / 2, Main.screenHeight / 2)) * Main.GameZoomTarget + new Vector2(Main.screenWidth / 2, Main.screenHeight / 2), rect, color, projectile.rotation, texture.Size() / 2, scale / 2, SpriteEffects.None, 0);
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            Main.instance.GraphicsDevice.BlendState = BlendState.AlphaBlend;
        }
        public static void DrawLightBeam(this Projectile projectile, Texture2D textureEnd, Texture2D textureBody, Color color, byte alpha, byte alphaStep, Vector2 scale, Vector2 scaleStep, int layers)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            Main.instance.GraphicsDevice.BlendState = BlendState.Additive;

            color.A += alpha;
            scale *= Main.GameZoomTarget;

            var rotation = projectile.rotation;
            var texture = textureEnd;
            var position = (projectile.Center - Main.screenPosition - new Vector2(Main.screenWidth / 2, Main.screenHeight / 2)) * Main.GameZoomTarget;
            position += new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);

            for (int parts = 2; parts > -1; parts--)
            {
                var rect = new Rectangle(0, 0, texture.Width, texture.Height);

                for (int i = layers; i > 0; i--)
                {
                    color.A += alphaStep;
                    scale -= scaleStep;

                    Main.spriteBatch.Draw(texture, position, rect, color, rotation, texture.Size() / 2, scale, SpriteEffects.None, 0);
                }

                scale += scaleStep * layers;
                color.A = alpha;

                position += new Vector2(projectile.height * 15f * Main.GameZoomTarget, 0).RotatedBy(rotation + MathHelper.PiOver2);
                rotation += MathHelper.Pi;

                if (parts == 1)
                {
                    texture = textureBody;
                    position += new Vector2(projectile.height * 15f * Main.GameZoomTarget, 0).RotatedBy(rotation + MathHelper.PiOver2) * 0.5f;
                    scale.Y = (new Vector2(projectile.height * 15f * Main.GameZoomTarget, 0).RotatedBy(rotation + MathHelper.PiOver2).Length() - textureEnd.Height * Main.GameZoomTarget) / textureBody.Height;
                    scaleStep.Y = 0;
                    layers--;
                }
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            Main.instance.GraphicsDevice.BlendState = BlendState.AlphaBlend;
        }

        /// <summary>
        /// This method sets the center of the projectile it was called upon to the center of its owner.
        /// </summary>
        public static void CenteredOnPlayer(this Projectile projectile)
        {
            Vector2 position = projectile.Center;

            if (Main.player[projectile.owner].active)
                position = Main.player[projectile.owner].Center;

            projectile.Center = position;
        }

        /// <summary>
        /// This method repells projectiles that collide with the hitbox of the projectile it was called upon in the direction opposite of the projectile's center.
        /// </summary>
        public static void ProjectileRepelling(this Projectile proj)
        {
            foreach (var projectile in Main.ActiveProjectiles)
            {
                var predicate = proj.hostile ? new Predicate<Projectile>(p => p != null && p.aiStyle != ProjAIStyleID.Spear && p.active && p.type != proj.type && p.velocity.LengthSquared() > 1 && p.Colliding(p.getRect(), proj.getRect())) : new Predicate<Projectile>(p => p != null && p.active && p.hostile && p.type != proj.type && p.Colliding(p.getRect(), proj.getRect()));
                if (proj.ReflectCheck(projectile, predicate))
                {
                    projectile.velocity += proj.hostile ? projectile.Center.DirectionFrom(proj.Center) * 4 : projectile.Center.DirectionFrom(proj.Center);
                    projectile.netUpdate = true;
                }
            }
        }
        public static void OrbitingPlayer(this Projectile projectile, float visualRotation, float r, float rotation)
        {
            projectile.rotation = rotation - MathHelper.PiOver2 * visualRotation;

            var desiredPosition = rotation.ToRotationVector2() * r;

            projectile.Center = Main.player[projectile.owner].Center + desiredPosition;
            projectile.netUpdate = true;
        }
        public static void FollowingPlayer(this Projectile projectile, float wobble, float period)
        {
            projectile.velocity += projectile.Center.DirectionTo(Main.player[projectile.owner].Center) * (projectile.Center.Distance(Main.player[projectile.owner].Center) / 90 - 1);
            projectile.position.Y += wobble * period;
            projectile.netUpdate = true;
        }
        public static bool Transform(this Projectile projectile, bool condition)
        {
            if (condition)
                return false;

            else if (projectile.damage > 0)
                return true;

            return false;
        }
    }
}