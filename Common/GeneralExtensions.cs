using System;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using System.Collections.Generic;
using System.Linq;
using Terraria.ID;

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
    }
    public static class NPCExtensions
    {
        public class SlowedGlobalNPC : GlobalNPC
        {
            public override bool InstancePerEntity => true;
            public (bool, int) Slowed = (false, 0);
            public int counter = 0;
            public override bool PreAI(NPC npc)
            {
                if (Slowed.Item1 == true)
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
                if (Slowed.Item1)
                    npc.velocity *= 0.97f;
            }
            public override void DrawEffects(NPC npc, ref Color drawColor)
            {
                if (Slowed.Item1 == true)
                    drawColor.A = 100;

                Slowed.Item2--;
                if (Slowed.Item2 <= 0)
                    Slowed.Item1 = false;

                base.DrawEffects(npc, ref drawColor);
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
        public static bool ReflectProjectiles(this Player player, DamageClass damageClass, float chance)
        {
            var target = Array.Find(Main.projectile, proj => proj.velocity != Vector2.Zero && proj.active && proj.hostile && proj.Colliding(proj.getRect(), player.getRect()));
            if (target != null)
            {
                if (Main.rand.NextFloat() >= chance)
                    return false;

                target.velocity = -target.velocity;
                target.hostile = false;
                target.friendly = true;
                target.reflected = true;
                target.DamageType = damageClass;
                target.netUpdate = true;
                return true;
            }
            return false;
        }
        public static bool OwnsProjectile(this Player player, int type)
        {
            return player.ownedProjectileCounts[type] > 0;
            return Main.ActiveProjectiles.Find(proj => proj.type == type && proj.active && proj.owner == player.whoAmI) != null;
        }
        public static void SpawnProjectileIfNotSpawned(this Player player, int type)
        {
            Projectile proj = null;
            if (Main.myPlayer != player.whoAmI)
                return;
            if (!player.OwnsProjectile(type))
                proj = Main.projectile[Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, Vector2.Zero, type, 0, 0, player.whoAmI)];
            else
                proj = Main.ActiveProjectiles.Find(proj => proj.type == type && proj.active && proj.owner == player.whoAmI);
            if (proj != null)
            {
                proj.timeLeft = 3;
                proj.netUpdate = true;
            }
        }
        public static void SpawnProjectileIfNotSpawned(this Player player, int type, Vector2 position)
        {
            Projectile proj = null;
            if (Main.myPlayer != player.whoAmI)
                return;
            if (!player.OwnsProjectile(type))
                Projectile.NewProjectile(player.GetSource_FromThis(), position, Vector2.Zero, type, 0, 0, player.whoAmI);
            else
                proj = Main.ActiveProjectiles.Find(proj => proj.type == type && proj.active && proj.owner == player.whoAmI);
            if (proj != null)
            {
                proj.timeLeft = 3;
                proj.netUpdate = true;
            }
        }
        public static void SpawnProjectileIfNotSpawned(this Player player, int type, IEntitySource source)
        {
            Projectile proj = null;
            if (Main.myPlayer != player.whoAmI)
                return;
            if (!player.OwnsProjectile(type))
                Projectile.NewProjectile(source, player.Center, Vector2.Zero, type, 0, 0, player.whoAmI);
            else
                proj = Main.ActiveProjectiles.Find(proj => proj.type == type && proj.active && proj.owner == player.whoAmI);
            if (proj != null)
            {
                proj.timeLeft = 3;
                proj.netUpdate = true;
            }
        }
        public static void SpawnProjectileIfNotSpawned(this Player player, int type, Vector2 position, IEntitySource source)
        {
            Projectile proj = null;
            if (Main.myPlayer != player.whoAmI)
                return;
            if (!player.OwnsProjectile(type))
                Projectile.NewProjectile(source, position, Vector2.Zero, type, 0, 0, player.whoAmI);
            else
                proj = Main.ActiveProjectiles.Find(proj => proj.type == type && proj.active && proj.owner == player.whoAmI);
            if (proj != null)
            {
                proj.timeLeft = 3;
                proj.netUpdate = true;
            }
        }
    }
}