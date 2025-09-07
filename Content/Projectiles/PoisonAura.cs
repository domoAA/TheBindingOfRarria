using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheBindingOfRarria.Common;
using TheBindingOfRarria.Common.Graphics.Particles;
using TheBindingOfRarria.Common.Helpers;
using TheBindingOfRarria.Common.Registries;
using static System.Net.Mime.MediaTypeNames;

namespace TheBindingOfRarria.Content.Projectiles;

internal class PoisonAura : ModProjectile
{
    internal List<Vector2>[] Positions = [[], []];
    internal int Owner = -1;

    internal ref float Time => ref Projectile.ai[0];
    private Vector2 Inner => Projectile.Center - new Vector2(10, 0);

    public override string Texture => Helper.GetVanillaProjectileTexture(4);

    public override void SetDefaults()
    {
        Projectile.width = Projectile.height = 1;
        Projectile.timeLeft = 180;
        Projectile.scale = Projectile.Opacity = 0;
        Projectile.friendly = true;
        Projectile.hostile = false;
        Projectile.tileCollide = false;
        Projectile.penetrate = -1;

        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = 25;
    }

    public override void AI()
    {
        if (Time == 0)
        {
            int positionCount = 128;

            //for (int i = 0; i <= positionCount; i++)
            //{
            //    float rotation = Helper.EvenCircle(i, positionCount);

            //    Positions[0].Add(rotation.ToRotationVector2() * 50f); 
            //    Positions[1].Add(rotation.ToRotationVector2() * 100f);
            //}

            for (int i = (int)(-positionCount / 2f); i <= (positionCount / 2f); i++)
            {
                Vector2 sigmagyat = Vector2.UnitX;
                Vector2 _posMain = Inner - sigmagyat * 10f;
                Vector2 _posInner = Inner - sigmagyat * 20f;
                Vector2 rotation = sigmagyat.RotatedBy(Helper.EvenCircle(i, positionCount)) * 50f;

                Positions[0].Add(_posMain + rotation - Inner);
                Positions[1].Add(_posInner + rotation - Inner);
            }

            Time = 1;
        }

        else
        {
            Time++;

            Projectile.Opacity = Lerp(0, 1, Clamp(Time / 25f, 0, 1));
            Projectile.scale = Lerp(0, 1, Clamp(Time / 15f, 0, 1));

            Projectile.Center = Main.LocalPlayer.Center + new Vector2(25, 0);

            ParticleManager.SpawnParticle(Main.LocalPlayer.Center + Main.rand.NextVector2CircularEdge(25f, 25f), -Vector2.UnitY, 
                30, Color.Lerp(Color.Lime, Color.LimeGreen, 0.33f), 
                new ParticleFadeData(Main.rand.NextFloat(0.5f, 1f), 0, 1, 0), 
                new Vector2(Main.rand.NextFloat(0.8f, 2f), Main.rand.NextFloat(0.1f, 0.133f) * 0.6f) * 0.4f, 
                ParticleBehaviour.Default, ParticleDrawType.Additive, ParticleTextureType.FadedGlowyBall, 
                [0.98f]
            );

        }
    }

    public override bool PreDraw(ref Color lightColor)
    {
        if (Time == 0)
            return false;

        Main.spriteBatch.End();
        Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

        List<VertexInfo> vertices = [];
        List<VertexInfo> vertices2 = [];
        List<VertexInfo> vertices3 = [];

        //float t = Main.GlobalTimeWrappedHourly * 1.8f;

        //for (int i = 0; i < Positions[0].Count; i++) //explosion ahh effect
        //{
        //    float factor = i / (float)Positions[0].Count;
        //    float opacity = Projectile.Opacity * 0.5f;
        //    float t = (float)Main.timeForVisualEffects * -0.2f;

        //    Color color = Color.Lerp(Color.Green, Color.Lime, factor) * opacity;

        //    vertices.Add(new VertexInfo(Projectile.Center + Positions[0][i] * Main.rand.NextFloat(0, 1f) - Main.screenPosition, new Vector3(1f, 0f + t, opacity), color));
        //    vertices.Add(new VertexInfo(Projectile.Center + Positions[1][i] * Main.rand.NextFloat(0.5f, 1f) - Main.screenPosition, new Vector3(0f, 1f + t, opacity), color));
        //}

        Vector2 up = new(0f, 0.01f);

        for (int i = 0; i < Positions[0].Count; i++)
        {
            float factor = i / (float)Positions[0].Count;
            float a = 0.35f * Projectile.Opacity;
            float t = (float)Main.timeForVisualEffects * -0.063f;

            Color color = Color.Green * Projectile.Opacity * a;

            vertices.Add(new VertexInfo(Inner + up + 1000f * Projectile.scale * 0.002f * Positions[0][i] - Main.screenPosition, new Vector3(factor + t, 0f, 1f), color));
            vertices.Add(new VertexInfo(Inner - up + 100f * Projectile.scale * 0.002f * Positions[1][i] - Main.screenPosition, new Vector3(factor + t, 1f, 0f), color));
        }

        for (int i = 0; i < Positions[0].Count; i++)
        {
            float factor = i / (float)Positions[0].Count;
            float a = 0.5f * Projectile.Opacity;
            float t = (float)Main.timeForVisualEffects * 0.063f;

            Color color = Color.Green * Projectile.Opacity * a;

            vertices2.Add(new VertexInfo(Inner + up + 2f * Projectile.scale * 1f * Positions[0][i] - Main.screenPosition, new Vector3(factor + t, 0f, 1f), color));
            vertices2.Add(new VertexInfo(Inner - up + 1.1f * Projectile.scale * 1f * Positions[1][i] - Main.screenPosition, new Vector3(factor + t, 1f, 0f), color));
        }

        for (int i = 0; i < Positions[0].Count; i++)
        {
            float factor = i / (float)Positions[0].Count;
            float a = 0.35f * Projectile.Opacity;
            float t = (float)Main.timeForVisualEffects * -0.1f;

            Color color = Color.Green * Projectile.Opacity * a;

            vertices3.Add(new VertexInfo(Inner + up + 2f * Projectile.scale * 1f * Positions[0][i] - Main.screenPosition, new Vector3(factor + t, 0f, 1f), color));
            vertices3.Add(new VertexInfo(Inner - up + 1.1f * Projectile.scale * 1f * Positions[1][i] - Main.screenPosition, new Vector3(factor + t, 1f, 0f), color));
        }

        if (vertices.Count > 2)
        {
            Main.graphics.GraphicsDevice.Textures[0] = Textures.TrailTexture2.Value;
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices.ToArray(), 0, vertices.Count - 2);
        }

        if (vertices2.Count > 2)
        {
            Main.graphics.GraphicsDevice.Textures[0] = Textures.TrailTexture2.Value;
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices2.ToArray(), 0, vertices2.Count - 2);
        }

        if (vertices3.Count > 2)
        {
            Main.graphics.GraphicsDevice.Textures[0] = Textures.TrailTexture.Value;
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices3.ToArray(), 0, vertices3.Count - 2);
        }

        Main.spriteBatch.End();
        Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

        return false;
    }
}
