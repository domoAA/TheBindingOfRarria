using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TheBindingOfRarria.Content.Projectiles
{
    public class GlobalProjectileReflectionBlacklist : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
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
        public (TheBindingOfRarria.State, int) Slowed = (TheBindingOfRarria.State.Default, 0);
        public override void PostAI(Projectile projectile)
        {
            if (Slowed.Item1 == TheBindingOfRarria.State.Slow)
                projectile.velocity *= 0.97f;
            else if (Slowed.Item1 == TheBindingOfRarria.State.Fast)
                projectile.velocity *= 1.03f;
            projectile.netUpdate = true;
        }
        public override bool PreDraw(Projectile projectile, ref Color lightColor)
        {
            if (Slowed.Item1 == TheBindingOfRarria.State.Slow)
                lightColor.A = 130;

            Slowed.Item2--;
            if (Slowed.Item2 <= 0)
                Slowed.Item1 = TheBindingOfRarria.State.Default;

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
            var predicate = new Predicate<Projectile>(proj => proj.velocity.LengthSquared() > 1 && !proj.reflected && proj.type != projectile.type && proj.hostile && projectile.Colliding(proj.getRect(), projectile.getRect()));

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

                    target.reflected = true;
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
                    var reflected = Main.rand.NextFloat() < chance;

                    if (Main.netMode == NetmodeID.MultiplayerClient){
                        ModPacket packet = ModContent.GetInstance<TheBindingOfRarria>().GetPacket();
                        packet.Write((int)TheBindingOfRarria.PacketTypes.ProjectileReflection);
                        packet.Write(reflected);
                        packet.Write(target.identity);
                        packet.Write(friendly);
                        packet.Send(); }
                    else if (reflected)
                    {
                        target.GetReflected(friendly, false);
                        target.reflected = true;
                    }


                    if (!reflected)
                        return;

                    if (projectile.type == ModContent.ProjectileType<CircleOfLight>())
                        projectile.ai[0] = 1;
                }
            }
        }
        public static void GetReflected(this Projectile projectile, bool friendly, bool Directly = false)
        {
            if (!Directly)
            {
                projectile.velocity = -projectile.velocity;
                if (friendly)
                {
                    projectile.hostile = false;
                    projectile.friendly = true;
                    projectile.reflected = true;
                }
            }
            else
            {
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    ModPacket packet = ModContent.GetInstance<TheBindingOfRarria>().GetPacket();
                    packet.Write((int)TheBindingOfRarria.PacketTypes.ProjectileReflection);
                    packet.Write(true);
                    packet.Write(projectile.identity);
                    packet.Write(friendly);
                    packet.Send();
                }
                else
                {
                    projectile.GetReflected(friendly, false);
                    projectile.reflected = true;
                }
            }
        }
        public static void GetSlowed(this Projectile projectile, TheBindingOfRarria.State state, int duration)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                ModPacket packet = ModContent.GetInstance<TheBindingOfRarria>().GetPacket();
                packet.Write((int)TheBindingOfRarria.PacketTypes.EntitySlow);
                packet.Write((int)state);
                packet.Write(duration);
                packet.Write(true);
                packet.Write(projectile.identity);
                packet.Send();
            }
            else
            {
                projectile.GetGlobalProjectile<SlowedGlobalProjectile>().Slowed = (state, duration);
            }
        }
        public static void DrawPixellated(this Projectile projectile, Color color, byte alpha, SpriteEffects effects, PixellationSystem.RenderType renderType)
        {
            var texture = Terraria.GameContent.TextureAssets.Projectile[projectile.type].Value;
            var scale = projectile.scale * Main.GameZoomTarget;
            color.A += alpha;

            PixellationSystem.QueuePixelationAction(() => {
                Main.EntitySpriteDraw(texture, (projectile.Center - Main.screenPosition) / 2, texture.Bounds, color, projectile.rotation, texture.Size() / 2, scale / 2, effects, 0);
            }, renderType);
        }
        public static void DrawPixellated(this Projectile projectile)
        {
            var texture = Terraria.GameContent.TextureAssets.Projectile[projectile.type].Value;
            var scale = projectile.scale * Main.GameZoomTarget;

            PixellationSystem.QueuePixelationAction(() => {
                Main.EntitySpriteDraw(texture, (projectile.Center - Main.screenPosition) / 2, texture.Bounds, Color.White, projectile.rotation, texture.Size() / 2, scale / 2, SpriteEffects.None, 0);
            }, PixellationSystem.RenderType.AlphaBlend);
        }
        public static void DrawWithTransparency(this Projectile projectile, Color color, byte alpha)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            Main.instance.GraphicsDevice.BlendState = BlendState.Additive;
            var texture = Terraria.GameContent.TextureAssets.Projectile[projectile.type].Value;
            var scale = projectile.scale * Main.GameZoomTarget;
            color.A += alpha;
            Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, texture.Bounds, color, projectile.rotation, texture.Size() / 2, scale / 2, SpriteEffects.None, 0);

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
            //Main.spriteBatch.End();
            //Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            //Main.instance.GraphicsDevice.BlendState = BlendState.Additive;

            color.A += alpha;
            scale *= Main.GameZoomTarget;

            var rotation = projectile.rotation;
            var texture = textureEnd;
            var position = (projectile.Center + projectile.Center.DirectionFrom(Main.player[projectile.owner].Center) * 30 * Main.GameZoomTarget - Main.screenPosition) / 2;

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

                position += new Vector2(projectile.height * 15f, 0).RotatedBy(rotation + MathHelper.PiOver2) / 2;
                rotation += MathHelper.Pi;

                if (parts == 1)
                {
                    texture = textureBody;
                    position += new Vector2(projectile.height * 15f, 0).RotatedBy(rotation + MathHelper.PiOver2) * 0.5f / 2;
                    scale.Y = (new Vector2(projectile.height * 15f, 0).RotatedBy(rotation + MathHelper.PiOver2).Length() / 2 - textureEnd.Height * Main.GameZoomTarget) / textureBody.Height;
                    scaleStep.Y = 0;
                    layers--;
                }
            }

            //Main.spriteBatch.End();
            //Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            //Main.instance.GraphicsDevice.BlendState = BlendState.AlphaBlend;
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
                var predicate = proj.hostile ? new Predicate<Projectile>(p => p != null && p.aiStyle != ProjAIStyleID.Spear && p.active && p.type != proj.type && p.velocity.LengthSquared() > 1 && p.Colliding(p.getRect(), proj.getRect())) : new Predicate<Projectile>(p => p != null && p.active && p.hostile && p.type != proj.type && p.velocity.LengthSquared() > 1 && p.Colliding(p.getRect(), proj.getRect()));
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