using System;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using System.Collections.Generic;
using System.Linq;
using Terraria.ID;
using Terraria.GameContent;
using TheBindingOfRarria.Content.Projectiles;
using MonoMod.Cil;
using TheBindingOfRarria.Content.Items;

namespace TheBindingOfRarria.Content
{
    public static class GeneralExtensions
    {
        public static T Find<T>(this ActiveEntityIterator<T> iterator, Func<T, bool> predicate) where T : Entity
        {
            foreach (var entity in iterator)
            {
                if (predicate(entity))
                    return entity;
            }
            return null;
        }
        public static void DrawPixellated(this Texture2D texture, Vector2 position, float scale, float rotation, Color color, PixellationSystem.RenderType renderType)
        {
            //scale *= Main.GameZoomTarget;

            PixellationSystem.QueuePixelationAction(() =>
            {
                Main.EntitySpriteDraw(texture, position, texture.Bounds, color, rotation, texture.Size() / 2, scale / 2, SpriteEffects.None, 0);
            }, renderType);
        }
        public static void DrawPixellated(this Texture2D texture, Vector2 position, Vector2 scale, float rotation, Color color, PixellationSystem.RenderType renderType)
        {
            //scale *= Main.GameZoomTarget;

            PixellationSystem.QueuePixelationAction(() =>
            {
                Main.EntitySpriteDraw(texture, position, texture.Bounds, color, rotation, texture.Size() / 2, scale / 2, SpriteEffects.None, 0);
            }, renderType);
        }
        public static void DrawWithTransparency(this Texture2D texture, Vector2 center, Rectangle? rect, Color color, byte alpha, byte alphaStep, float scale, float scaleStep, int layers)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            Main.instance.GraphicsDevice.BlendState = BlendState.Additive;
            color.A += alpha;
            scale *= Main.GameZoomTarget;
            for (int i = 0; i < layers; i++)
            {
                color.A += alphaStep;
                scale -= scaleStep;

                Main.spriteBatch.Draw(texture, center, rect, color, 0, texture.Size() / 2, scale / 2, SpriteEffects.None, 0);
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            Main.instance.GraphicsDevice.BlendState = BlendState.AlphaBlend;
        }
        public static void DrawWithTransparency(this Texture2D texture, Vector2 center, float scale, Color color, byte alpha)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            Main.instance.GraphicsDevice.BlendState = BlendState.Additive;
            color.A += alpha;
            scale *= Main.GameZoomTarget;
            Main.spriteBatch.Draw(texture, center, texture.Bounds, color, 0, texture.Size() / 2, scale / 2, SpriteEffects.None, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            Main.instance.GraphicsDevice.BlendState = BlendState.AlphaBlend;
        }
        public static void DrawWithTransparency(this Texture2D texture, Vector2 center, float scale, float rotation, Color color, byte alpha)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            Main.instance.GraphicsDevice.BlendState = BlendState.Additive;
            color.A += alpha;
            scale *= Main.GameZoomTarget;
            Main.spriteBatch.Draw(texture, center, texture.Bounds, color, rotation, texture.Size() / 2, scale / 2, SpriteEffects.None, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            Main.instance.GraphicsDevice.BlendState = BlendState.AlphaBlend;
        }
        public static void DrawWithTransparency(this Texture2D texture, Vector2 center, float scale, float rotation, Color color, byte alpha, SpriteEffects effect, bool apply)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            Main.instance.GraphicsDevice.BlendState = BlendState.Additive;
            color.A += alpha;
            scale *= Main.GameZoomTarget;
            effect = apply ? effect : SpriteEffects.None;
            Main.spriteBatch.Draw(texture, center, texture.Bounds, color, rotation, texture.Size() / 2, scale / 2, effect, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            Main.instance.GraphicsDevice.BlendState = BlendState.AlphaBlend;
        }
        public static void SpawnDust(this Vector2 center, Vector2 totalRect, int type, float speed, float scale, Color color, int amount, float distance = 1, float rectRotation = 0)
        {
            for (int i = amount; i > 0; i--)
            {
                var direction = totalRect.RotatedBy(rectRotation) * Vector2.One.RotatedBy(MathHelper.TwoPi / amount * i + Main.rand.NextFloat(-MathHelper.Pi / 10, MathHelper.Pi / 10));
                Dust.NewDustPerfect(center + direction * distance, type, direction * speed, 0, color, scale);
            }
        }
        public static void SpawnDust(this Vector2 center, int type, float speed, float scale, Color color, int amount, float distance = 1, float totalSize = 1, float rotation = -MathHelper.PiOver4 * 3, int layers = 1, float scaleStep = 0f)
        {
            for (int x = layers; x > 0; x--)
            {
                scale += scaleStep;
                for (int i = amount; i > 0; i--)
                {
                    var rot = Main.rand.NextFloat(-totalSize / 2, totalSize / 2);
                    var direction = Vector2.One.RotatedBy(totalSize + rot + rotation);
                    Dust.NewDustPerfect(center + direction * distance, type, direction * speed, 0, color, scale * Main.rand.NextFloat(0.8f, 1.3f));
                }
            }
        }
        public static void SpawnDust(this Vector2 center, Vector2[] directions, int type, float speed, float scale, Color color, int layers = 1, float scaleStep = 0, float random = 0, float distance = 1)
        {
            for (int i = layers; i > 0; i--)
            {
                scale += scaleStep;
                foreach (var direction in directions)
                {
                    var rotation = Main.rand.NextFloat(-random / 2, random / 2);
                    Dust.NewDustPerfect(
                        center + direction.RotatedBy(rotation) * distance,
                        type,
                        direction.RotatedBy(rotation) * speed, 
                        255, 
                        color, 
                        scale * float.Abs(rotation) + 0.3f
                    );
                }
            }
        }
    }
    public static class NPCExtensions
    {
        public class SlowedGlobalNPC : GlobalNPC
        {
            public override bool InstancePerEntity => true;
            public (TheBindingOfRarria.State, int) Slowed = (TheBindingOfRarria.State.Default, 0);
            public int counter = 0;
            public override bool PreAI(NPC npc)
            {
                if (Slowed.Item1 == TheBindingOfRarria.State.Slow)
                    counter++;

                if (counter >= 3)
                {
                    counter = 0;
                    return false;
                }
                return base.PreAI(npc);
            }
            public override void PostAI(NPC npc)
            {
                if (Slowed.Item1 == TheBindingOfRarria.State.Slow)
                    npc.velocity *= 0.97f;
                else if (Slowed.Item1 == TheBindingOfRarria.State.Fast)
                    npc.velocity *= 1.03f;
            }
            public override void DrawEffects(NPC npc, ref Color drawColor)
            {
                if (Slowed.Item1 == TheBindingOfRarria.State.Slow)
                    drawColor.A = 100;

                Slowed.Item2--;
                if (Slowed.Item2 <= 0)
                    Slowed.Item1 = TheBindingOfRarria.State.Default;

                base.DrawEffects(npc, ref drawColor);
            }
        }
        public static void GetSlowed(this NPC npc, TheBindingOfRarria.State state, int duration)
        {
            if (npc.GetGlobalNPC<SlowedGlobalNPC>().Slowed.Item1 == state)
                return;

            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                ModPacket packet = ModContent.GetInstance<TheBindingOfRarria>().GetPacket();
                packet.Write((int)TheBindingOfRarria.PacketTypes.EntitySlow);
                packet.Write((int)state);
                packet.Write(duration);
                packet.Write(false);
                packet.Write(npc.whoAmI);
                packet.Send();
            }
            else
            {
                npc.GetGlobalNPC<SlowedGlobalNPC>().Slowed = (state, duration);
            }
        }
    }
    public static class ItemExtensions
    {
        public static bool HasTag(this Item item, int[] tag)
        {
            foreach (var member in tag)
                if (member == item.type)
                    return true;

            return false;
        }
    }
    public static class PlayerExtensions
    {
        public static bool OwnsProjectile(this Player player, int type)
        {
            return player.ownedProjectileCounts[type] > 0;
            // return Main.ActiveProjectiles.Find(proj => proj.type == type && proj.active && proj.owner == player.whoAmI) != null;
        }
        public static void SpawnProjectileIfNotSpawned(this Player player, int type, IEntitySource source, Vector2 position)
        {
            Projectile proj = null;
            if (!player.OwnsProjectile(type) && Main.myPlayer == player.whoAmI)
                Projectile.NewProjectile(source, position, Vector2.Zero, type, 0, 0, player.whoAmI);
            else
                proj = Main.ActiveProjectiles.Find(proj => proj.type == type && proj.active && proj.owner == player.whoAmI);
            if (proj != null)
            {
                proj.timeLeft = 5;
                proj.netUpdate = true;
            }
        }
        public static void SpawnProjectileIfNotSpawned(this Player player, int type, IEntitySource source)
        {
            Projectile proj = null;
            if (!player.OwnsProjectile(type) && Main.myPlayer == player.whoAmI)
                Projectile.NewProjectile(source, player.Center, Vector2.Zero, type, 0, 0, player.whoAmI);
            else
                proj = Main.ActiveProjectiles.Find(proj => proj.type == type && proj.active && proj.owner == player.whoAmI);
            if (proj != null)
            {
                proj.timeLeft = 5;
                proj.netUpdate = true;
            }
        }
        public static void SpawnProjectileIfNotSpawned(this Player player, int type, Vector2 position, IEntitySource source)
        {
            Projectile proj = null;
            if (!player.OwnsProjectile(type) && Main.myPlayer == player.whoAmI)
                Projectile.NewProjectile(source, position, Vector2.Zero, type, 0, 0, player.whoAmI);
            else
                proj = Main.ActiveProjectiles.Find(proj => proj.type == type && proj.active && proj.owner == player.whoAmI);
            if (proj != null)
            {
                proj.timeLeft = 5;
                proj.netUpdate = true;
            }
        }
    }
    public class FixDustBugSystem : ModSystem
    {
        // credits to Lion (or Zen?? idk who even made it tbh)
        public override void Load()
        {
            IL_Dust.NewDust += FixStupidBugBecauseTerrariaDevsAreIncompetent;
        }

        public override void Unload()
        {
            IL_Dust.NewDust -= FixStupidBugBecauseTerrariaDevsAreIncompetent;
        }

        private void FixStupidBugBecauseTerrariaDevsAreIncompetent(ILContext il)
        {
            ILCursor c = new(il);

            c.GotoNext(MoveType.After,
                i => i.MatchLdloca(1),
                i => i.MatchLdloc2(),
                i => i.MatchCall<Rectangle>("Intersects"));

            c.EmitLdarg3();
            c.EmitDelegate((bool Intersects, int Type) =>
            {
                if (Type == ModContent.DustType<PixelatedDustParticle>())
                    return true;
                return Intersects;
            });
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
            DrawRT();
            orig.Invoke(self);
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