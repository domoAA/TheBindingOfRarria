using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using static TheBindingOfRarria.Common.PixellationSystem;

namespace TheBindingOfRarria.Common
{
    public class VertexStrip
    {
        public delegate Color StripColorFunction(float progressOnStrip);

        public delegate float StripHalfWidthFunction(float progressOnStrip);

        private struct CustomVertexInfo : IVertexType
        {
            public Vector2 Position;
            public Color Color;
            public Vector2 TexCoord;
            private static VertexDeclaration _vertexDeclaration = new VertexDeclaration(new VertexElement(0, VertexElementFormat.Vector2, VertexElementUsage.Position, 0), new VertexElement(8, VertexElementFormat.Color, VertexElementUsage.Color, 0), new VertexElement(12, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0));

            public VertexDeclaration VertexDeclaration => _vertexDeclaration;

            public CustomVertexInfo(Vector2 position, Color color, Vector2 texCoord)
            {
                Position = position;
                Color = color;
                TexCoord = texCoord;
            }
        }

        private CustomVertexInfo[] _vertices = new CustomVertexInfo[1];
        private int _vertexAmountCurrentlyMaintained;
        private short[] _indices = new short[1];
        private int _indicesAmountCurrentlyMaintained;
        private List<Vector2> _temporaryPositionsCache = new List<Vector2>();
        private List<float> _temporaryRotationsCache = new List<float>();

        public void PrepareStrip(Vector2[] positions, float[] rotations, StripColorFunction colorFunction, StripHalfWidthFunction widthFunction, Vector2 offsetForAllPositions = default(Vector2), int? expectedVertexPairsAmount = null, bool includeBacksides = false)
        {
            int num = positions.Length;
            int num2 = (_vertexAmountCurrentlyMaintained = num * 2);
            if (_vertices.Length < num2)
                Array.Resize(ref _vertices, num2);

            int num3 = num;
            if (expectedVertexPairsAmount.HasValue)
                num3 = expectedVertexPairsAmount.Value;

            for (int i = 0; i < num; i++)
            {
                if (positions[i] == Vector2.Zero)
                {
                    num = i - 1;
                    _vertexAmountCurrentlyMaintained = num * 2;
                    break;
                }

                Vector2 pos = positions[i] + offsetForAllPositions;
                float rot = MathHelper.WrapAngle(rotations[i]);
                int indexOnVertexArray = i * 2;
                float progressOnStrip = (float)i / (float)(num3 - 1);
                AddVertex(colorFunction, widthFunction, pos, rot, indexOnVertexArray, progressOnStrip);
            }

            PrepareIndices(num, includeBacksides);
        }

        public void PrepareStripWithProceduralPadding(Vector2[] positions, float[] rotations, StripColorFunction colorFunction, StripHalfWidthFunction widthFunction, Vector2 offsetForAllPositions = default(Vector2), bool includeBacksides = false, bool tryStoppingOddBug = true)
        {
            int num = positions.Length;
            _temporaryPositionsCache.Clear();
            _temporaryRotationsCache.Clear();
            for (int i = 0; i < num && !(positions[i] == Vector2.Zero); i++)
            {
                Vector2 vector = positions[i];
                float num2 = MathHelper.WrapAngle(rotations[i]);
                _temporaryPositionsCache.Add(vector);
                _temporaryRotationsCache.Add(num2);
                if (i + 1 >= num || !(positions[i + 1] != Vector2.Zero))
                    continue;

                Vector2 vector2 = positions[i + 1];
                float num3 = MathHelper.WrapAngle(rotations[i + 1]);
                int num4 = (int)(Math.Abs(MathHelper.WrapAngle(num3 - num2)) / ((float)Math.PI / 12f));
                if (num4 != 0)
                {
                    float num5 = vector.Distance(vector2);
                    Vector2 value = vector + num2.ToRotationVector2() * num5;
                    Vector2 value2 = vector2 + num3.ToRotationVector2() * (0f - num5);
                    int num6 = num4 + 2;
                    float num7 = 1f / (float)num6;
                    Vector2 target = vector;
                    for (float num8 = num7; num8 < 1f; num8 += num7)
                    {
                        Vector2 vector3 = Vector2.CatmullRom(value, vector, vector2, value2, num8);
                        float item = MathHelper.WrapAngle(vector3.DirectionTo(target).ToRotation());
                        _temporaryPositionsCache.Add(vector3);
                        _temporaryRotationsCache.Add(item);
                        target = vector3;
                    }
                }
            }

            int count = _temporaryPositionsCache.Count;
            Vector2 zero = Vector2.Zero;
            for (int j = 0; j < count && (!tryStoppingOddBug || !(_temporaryPositionsCache[j] == zero)); j++)
            {
                Vector2 pos = _temporaryPositionsCache[j] + offsetForAllPositions;
                float rot = _temporaryRotationsCache[j];
                int indexOnVertexArray = j * 2;
                float progressOnStrip = (float)j / (float)(count - 1);
                AddVertex(colorFunction, widthFunction, pos, rot, indexOnVertexArray, progressOnStrip);
            }

            _vertexAmountCurrentlyMaintained = count * 2;
            PrepareIndices(count, includeBacksides);
        }

        private void PrepareIndices(int vertexPaidsAdded, bool includeBacksides)
        {
            int num = vertexPaidsAdded - 1;
            int num2 = 6 + includeBacksides.ToInt() * 6;
            int num3 = (_indicesAmountCurrentlyMaintained = num * num2);
            if (_indices.Length < num3)
                Array.Resize(ref _indices, num3);

            for (short num4 = 0; num4 < num; num4 = (short)(num4 + 1))
            {
                short num5 = (short)(num4 * num2);
                int num6 = num4 * 2;
                _indices[num5] = (short)num6;
                _indices[num5 + 1] = (short)(num6 + 1);
                _indices[num5 + 2] = (short)(num6 + 2);
                _indices[num5 + 3] = (short)(num6 + 2);
                _indices[num5 + 4] = (short)(num6 + 1);
                _indices[num5 + 5] = (short)(num6 + 3);
                if (includeBacksides)
                {
                    _indices[num5 + 6] = (short)(num6 + 2);
                    _indices[num5 + 7] = (short)(num6 + 1);
                    _indices[num5 + 8] = (short)num6;
                    _indices[num5 + 9] = (short)(num6 + 2);
                    _indices[num5 + 10] = (short)(num6 + 3);
                    _indices[num5 + 11] = (short)(num6 + 1);
                }
            }
        }

        private void AddVertex(StripColorFunction colorFunction, StripHalfWidthFunction widthFunction, Vector2 pos, float rot, int indexOnVertexArray, float progressOnStrip)
        {
            while (indexOnVertexArray + 1 >= _vertices.Length)
            {
                Array.Resize(ref _vertices, _vertices.Length * 2);
            }

            Color color = colorFunction(progressOnStrip);
            float num = widthFunction(progressOnStrip);
            Vector2 vector = MathHelper.WrapAngle(rot - (float)Math.PI / 2f).ToRotationVector2() * num;
            _vertices[indexOnVertexArray].Position = (pos + vector) / 2;
            _vertices[indexOnVertexArray + 1].Position = (pos - vector) / 2;
            _vertices[indexOnVertexArray].TexCoord = new Vector2(progressOnStrip, 1f);
            _vertices[indexOnVertexArray + 1].TexCoord = new Vector2(progressOnStrip, 0f);
            _vertices[indexOnVertexArray].Color = color;
            _vertices[indexOnVertexArray + 1].Color = color;
        }

        public void DrawTrail()
        {
            if (_vertexAmountCurrentlyMaintained >= 3)
                Main.instance.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, _vertices, 0, _vertexAmountCurrentlyMaintained, _indices, 0, _indicesAmountCurrentlyMaintained / 3);
        }
    }
        public class UhhPrimsPixellationTest : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
        {
            return entity.type == ProjectileID.RainbowRodBullet;
        }
        public override bool PreDraw(Projectile projectile, ref Color lightColor)
        {
            var texture = Terraria.GameContent.TextureAssets.Projectile[projectile.type].Value;
            var color = lightColor;
            var scale = projectile.scale / Math.Max(2, projectile.velocity.Length());

            Vector2 vector73 = new Vector2(60f, 30f);
            Vector2 vector36 = new Vector2(220f, -60f);
            var vector74 = texture.Size() / 2 + vector36 * Vector2.One;
            Color color81 = projectile.GetAlpha(lightColor);
            Vector2 spinningpoint6 = new Vector2(2f * scale + (float)Math.Cos(Main.GlobalTimeWrappedHourly * ((float)Math.PI * 2f)) * 0.4f, 0f).RotatedBy(projectile.rotation + Main.GlobalTimeWrappedHourly * ((float)Math.PI * 2f));
            
            QueuePixelationAction(() => {
                for (float num323 = 0f; num323 < 1f; num323 += 1f / 6f)
                {
                    Color color89 = Main.hslToRgb(num323, 1f, 0.5f) * 0.3f;
                    color89.A = 0;
                    Main.EntitySpriteDraw(texture, vector73 + spinningpoint6.RotatedBy(num323 * ((float)Math.PI * 2f)), null, color89, projectile.rotation, texture.Size() / 2, scale, SpriteEffects.None);
                }
                Main.EntitySpriteDraw(texture, vector73, null, color81, projectile.rotation, texture.Size() / 2, vector74, SpriteEffects.None);
            }, RenderType.Additive);



            DrawPixelPrimitive(() => {
                Draw(projectile);
            });
            return false;
        }
        private static VertexStrip _vertexStrip = new VertexStrip();

        public void Draw(Projectile proj)
        {
            MiscShaderData miscShaderData = GameShaders.Misc["RainbowRod"];
            miscShaderData.UseSaturation(-2.8f);
            miscShaderData.UseOpacity(4f);
            miscShaderData.Apply();
            _vertexStrip.PrepareStripWithProceduralPadding(proj.oldPos, proj.oldRot, StripColors, StripWidth, -Main.screenPosition + proj.Size / 2f);
            _vertexStrip.DrawTrail();
            Main.pixelShader.CurrentTechnique.Passes[0].Apply();
        }

        private Color StripColors(float progressOnStrip)
        {
            Color value = Main.hslToRgb((progressOnStrip * 1.6f - Main.GlobalTimeWrappedHourly) % 1f, 1f, 0.5f);
            Color result = Color.Lerp(Color.White, value, Utils.GetLerpValue(-0.2f, 0.5f, progressOnStrip, clamped: true)) * (1f - Utils.GetLerpValue(0f, 0.98f, progressOnStrip));
            result.A = 0;
            return result;
        }

        private float StripWidth(float progressOnStrip)
        {
            float num = 1f;
            float lerpValue = Utils.GetLerpValue(0f, 0.2f, progressOnStrip, clamped: true);
            num *= 1f - (1f - lerpValue) * (1f - lerpValue);
            return MathHelper.Lerp(0f, 32f, num);
        }
    }
    public class PixellationSystem : ModSystem
    {
        // credits for this whole class to Naka, and thanks to petrichor: i got my hands on this gem thanks to them

        public enum RenderType
        {
            AlphaBlend,
            //NonPremultiplied,
            Additive
        }
        private static List<Action> DrawActions { get; } = new();
        //private static List<Action> DrawActionsNonPremultiplied { get; } = new();
        private static List<Action> DrawActionsAdditive { get; } = new();
        private static List<Action> PrimitiveActions { get; } = new();
        private static RenderTarget2D AlphaBlendTarget { get; set; }
        private static RenderTarget2D AdditiveTarget { get; set; }
        private static RenderTarget2D PrimitiveTarget { get; set; }
        //private static RenderTarget2D NonPremultipliedTarget { get; set; }
        public override void Load()
        {

            if (!Main.dedServ)
            {
                Main.OnResolutionChanged += InitializeRT;
                Main.RunOnMainThread(() => {
                    AlphaBlendTarget = new(Main.instance.GraphicsDevice, Main.screenWidth / 2, Main.screenHeight / 2);
                    AdditiveTarget = new(Main.instance.GraphicsDevice, Main.screenWidth / 2, Main.screenHeight / 2);
                    PrimitiveTarget = new(Main.instance.GraphicsDevice, Main.screenWidth / 2, Main.screenHeight / 2);
                });
            }
            On_Main.DrawProjectiles += On_Main_DrawProjectiles;
            On_Main.CheckMonoliths += DrawToRT;
        }

        private void DrawToRT(On_Main.orig_CheckMonoliths orig)
        {
            orig.Invoke();
            // has to go here bc order of execution
            var gd = Main.graphics.GraphicsDevice;
            var oldRTs = gd.GetRenderTargets();
            gd.SetRenderTarget(AlphaBlendTarget);
            gd.Clear(Color.Transparent);
            Main.spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend, Main.DefaultSamplerState, default, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            foreach (var action in DrawActions)
            {
                action.Invoke();
            }
            Main.spriteBatch.End();
            gd.SetRenderTarget(PrimitiveTarget);
            Main.spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend, Main.DefaultSamplerState, default, Main.Rasterizer, null, Matrix.Identity);
            gd.Clear(Color.Transparent);
            foreach (var action in PrimitiveActions)
            {
                action.Invoke();
            }
            Main.spriteBatch.End();
            gd.SetRenderTarget(AdditiveTarget);
            Main.spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive, Main.DefaultSamplerState, default, Main.Rasterizer, null, Matrix.Identity);
            gd.Clear(Color.Transparent);
            foreach (var action in DrawActionsAdditive)
            {
                action.Invoke();
            }
            Main.spriteBatch.End();
            gd.SetRenderTargets(oldRTs);
            DrawActions.Clear();
            DrawActionsAdditive.Clear();
            PrimitiveActions.Clear();
        }
        public static void DrawPixelPrimitive(Action action)
        {
            PrimitiveActions.Add(action);
        }
        public override void Unload()
        {
            On_Main.DrawProjectiles -= On_Main_DrawProjectiles;
        }
        private void InitializeRT(Vector2 obj)
        {
            if (Main.dedServ)
            {
                return;
            }
            AlphaBlendTarget?.Dispose();
            //NonPremultipliedTarget?.Dispose();
            AdditiveTarget?.Dispose();
            PrimitiveTarget?.Dispose();
            GraphicsDevice gd = Main.instance.GraphicsDevice;
            int width = Main.screenWidth / 2;
            int height = Main.screenHeight / 2;
            AlphaBlendTarget = new(gd, width, height);
            AdditiveTarget = new(gd, width, height);
            PrimitiveTarget = new(gd, width, height);
        }
        private void On_Main_DrawProjectiles(On_Main.orig_DrawProjectiles orig, Main self)
        {
            orig.Invoke(self);
            DrawRT();
        }
        /// <summary>
        /// Queues a draw action to the pixelation system.
        /// Remember to halve the scale and draw position!
        /// </summary>
        /// <param name="action"></param>
        public static void QueuePixelationAction(Action action, RenderType type)
        {
            switch (type)
            {
                case RenderType.Additive:
                    DrawActionsAdditive.Add(action);
                    break;
                case RenderType.AlphaBlend:
                    DrawActions.Add(action);
                    break;
            }
        }
        /// <summary>
        /// Draws the RT with its pixelated content to 2x scale
        /// </summary>
        private static void DrawRT()
        {
            if (AlphaBlendTarget == null || AlphaBlendTarget.IsDisposed)
            {
                return;
            }
            Main.spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Matrix.Identity);
            Main.spriteBatch.Draw(AlphaBlendTarget, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Matrix.Identity);
            Main.spriteBatch.Draw(PrimitiveTarget, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);
            Main.spriteBatch.End();
            if (AdditiveTarget == null || AdditiveTarget.IsDisposed)
            {
                return;
            }
            Main.spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            Main.spriteBatch.Draw(AdditiveTarget, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);
            Main.spriteBatch.End();
        }
    }
}