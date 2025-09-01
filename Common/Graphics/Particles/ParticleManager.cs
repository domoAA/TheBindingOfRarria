using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Steamworks;
using System.Collections.Generic;
using TheBindingOfRarria.Common.Registries;
using TheBindingOfRarria.Common.Systems;

namespace TheBindingOfRarria.Common.Graphics.Particles;

public class ParticleManager : ModSystem
{
    internal static int MaxParticles = 25000;

    private static ParticleData[] Particles = new ParticleData[MaxParticles];
    private static int activeParticles = 0;

    #region management

    public static void SpawnParticle(Vector2 position, Vector2 velocity, int time, Color drawColor, ParticleFadeData fadeData, Vector2 scaleSquish, ParticleBehaviour type, ParticleDrawType drawType, ParticleTextureType texture, params float[] ai)
    {
        if (Main.dedServ || activeParticles > MaxParticles)
            return;

        ParticleData particle = new()
        {
            Position = position,
            Velocity = velocity,
            Squish = scaleSquish,
            fadeData = fadeData,
            ParticleBehaviour = type,
            ParticleDrawType = drawType,
            ParticleTextureType = texture,
            MaxTime = time,
            LifeTimer = 0,
            DrawColor = drawColor,
            ai = ai
        };

        Particles[activeParticles++] = particle;
    }

    public static void UpdateParticles()
    {
        for (int i = 0; i < activeParticles; i++)
        {
            ref ParticleData particle = ref Particles[i];

            float lifeRatio = particle.LifeTimer / (float)particle.MaxTime;
            particle.LifeTimer++;

            switch (particle.ParticleBehaviour)
            {
                case ParticleBehaviour.Default:
                    {
                        particle.Position += particle.Velocity * particle.ai[0];
                        particle.Rotation = particle.Velocity.ToRotation();
                        particle.Scale = Lerp(particle.fadeData.scaleStart, particle.fadeData.scaleEnd, lifeRatio);
                        particle.Opacity = Lerp(particle.fadeData.opacityStart, particle.fadeData.opacityEnd, lifeRatio);
                    }
                    break;
            }

            if (!particle.Active)
            {
                Particles[i] = Particles[--activeParticles];
                i--;
            }
        }
    }

    #endregion

    #region impl

    public override void Load()
    {
        if (!Main.dedServ)
            On_Main.DrawDust += DrawParticles;

        Particles = new ParticleData[MaxParticles];
    }

    public override void Unload()
    {
        if (!Main.dedServ)
            On_Main.DrawDust -= DrawParticles;

        Particles = null;
    }

    public override void PostUpdateDusts() => UpdateParticles();

    private void DrawParticles(On_Main.orig_DrawDust orig, Main self)
    {
        orig(self);

        List<ParticleData> additiveParticles = [];
        List<ParticleData> alphablendParticles = [];
        List<ParticleData> additiveParticlesPixelated = [];
        List<ParticleData> alphablendParticlesPixelated = [];

        for (int i = 0; i < activeParticles; i++)
        {
            ref ParticleData particle = ref Particles[i];

            if (!particle.Active)
                continue;

            switch (particle.ParticleDrawType)
            {
                case ParticleDrawType.Additive: { additiveParticles.Add(particle); } break;
                case ParticleDrawType.AlphaBlend: { alphablendParticles.Add(particle); } break;
                case ParticleDrawType.PixelatedAdditive: { alphablendParticlesPixelated.Add(particle); } break;
                case ParticleDrawType.PixelatedAlphaBlend: { alphablendParticlesPixelated.Add(particle); } break;
            }
        }

        if (additiveParticles.Count != 0)
        {
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            DrawAdditiveParticles([..additiveParticles], Main.spriteBatch);
            Main.spriteBatch.End();
        }

        if (alphablendParticles.Count != 0)
        {
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            DrawAlphablendParticles([..alphablendParticles], Main.spriteBatch);
            Main.spriteBatch.End();
        }

        if (additiveParticlesPixelated.Count != 0)
        {
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            PixellationSystem.QueuePixellationAction(() => DrawAdditiveParticles([..additiveParticlesPixelated], Main.spriteBatch), PixellationSystem.RenderType.Additive, PixellationSystem.RenderLayer.Projectiles);
            Main.spriteBatch.End();
        }

        if (alphablendParticlesPixelated.Count != 0)
        {
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            PixellationSystem.QueuePixellationAction(() => DrawAlphablendParticles([.. alphablendParticlesPixelated], Main.spriteBatch), PixellationSystem.RenderType.AlphaBlend, PixellationSystem.RenderLayer.Projectiles);
            Main.spriteBatch.End();
        }
    }

    #endregion

    #region individual handlers

    public static void DrawAdditiveParticles(ParticleData[] particles, SpriteBatch sb)
    {
        for (int i = 0; i < particles.Length; i++)
        {
            ref ParticleData particle = ref particles[i];

            Texture2D texture = Textures.Particles[(int)particle.ParticleTextureType].Value;

            sb.Draw(texture, particle.Position - Main.screenPosition, null,
                 particle.DrawColor * particle.Opacity, particle.Rotation, 
                 texture.Size() / 2f, particle.Scale * particle.Squish, SpriteEffects.None, 0f
            );
        }
    }

    public static void DrawAlphablendParticles(ParticleData[] particles, SpriteBatch sb)
    {
        for (int i = 0; i < particles.Length; i++)
        {
            ref ParticleData particle = ref particles[i];

            Texture2D texture = Textures.Particles[(int)particle.ParticleTextureType].Value;

            sb.Draw(texture, particle.Position - Main.screenPosition, null,
                 particle.DrawColor with { A = 0 } * particle.Opacity, particle.Rotation,
                 texture.Size() / 2f, particle.Scale * particle.Squish, SpriteEffects.None, 0f
            );
        }
    }

    #endregion
}
