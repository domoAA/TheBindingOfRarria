using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections;

namespace TheBindingOfRarria
{
    // Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
    public class TheBindingOfRarria : Mod
    {
        public static Asset<Texture2D> PoisonTexture;
        public static Asset<Texture2D> FireTexture;
        public static Asset<Texture2D> Circle;
        public static Asset<Texture2D> CenserExtra;
        public override void Load()
        {
            PoisonTexture = ModContent.Request<Texture2D>("TheBindingOfRarria/Content/Buffs/PoisonSign");
            FireTexture = ModContent.Request<Texture2D>("TheBindingOfRarria/Content/Buffs/FireSign");
            Circle = ModContent.Request<Texture2D>("TheBindingOfRarria/Content/Projectiles/CircleOfLight");
            CenserExtra = ModContent.Request<Texture2D>("TheBindingOfRarria/Content/Items/CenserExtra");
        }
    }
    public static class PlayerExtensions
    {
        public static bool ReflectProjectiles(this Player player, Rectangle rect, DamageClass damageClass, float chance)
        {
            var target = Array.Find(Main.projectile, proj => proj.velocity != Vector2.Zero && proj.active && proj.hostile && proj.Colliding(proj.getRect(), rect));
            if (target != null)
            {
                if (new Random().NextDouble() >= chance)
                    return false;

                target.velocity = -target.velocity;
                target.hostile = false;
                target.friendly = true;
                target.reflected = true;
                target.DamageType = damageClass;
                return true;
            }
            return false;
        }
    }
    public static class ProjectileExtensions
    {
        public static Dictionary<Projectile, bool> TrulyReflected = [];
        public static void CleanDictionary(Dictionary<Projectile, bool> dict)
        {
            foreach (var member in dict)
            {
                if (!member.Key.active)
                    dict.Remove(member.Key);
            }
        }
        public static void ReflectProjectiles(this Projectile projectile)
        {
            var target = Array.Find(Main.projectile, proj => proj.velocity != Vector2.Zero && proj.type != projectile.type && proj.active && proj.hostile && proj.Colliding(proj.getRect(), projectile.getRect()));
            if (target != null)
            {
                CleanDictionary(TrulyReflected);
                if (!TrulyReflected.ContainsKey(target))
                    TrulyReflected.Add(target, false);

                if (TrulyReflected[target])
                    return;

                target.velocity = -target.velocity;
            }
        }
        public static void ReflectProjectiles(this Projectile projectile, bool friendly, float chance)
        {
            var target = Array.Find(Main.projectile, proj => proj.velocity != Vector2.Zero && proj.type != projectile.type && proj.active && proj.hostile && proj.Colliding(proj.getRect(), projectile.getRect()));
            if (target != null)
            {
                CleanDictionary(TrulyReflected);
                if (!TrulyReflected.ContainsKey(target))
                    TrulyReflected.Add(target, false);

                if (new Random().NextDouble() >= chance || TrulyReflected[target])
                    return;

                target.velocity = -target.velocity;
                TrulyReflected[target] = true;

                if (!friendly)
                    return;

                target.hostile = false;
                target.friendly = true;
                target.reflected = true;
            }
        }
        public static void DrawWithTransparency(this Projectile projectile, Color color, byte alpha)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            Main.instance.GraphicsDevice.BlendState = BlendState.Additive;
            var texture = Terraria.GameContent.TextureAssets.Projectile[projectile.type].Value;
            color.A += alpha;
            Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, new Rectangle(0, 0, 256, 256), color, 0, texture.Size() / 2, projectile.scale / 2, SpriteEffects.None, 0);

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
            var projectile = Array.Find(Main.projectile, projectile => projectile.active && projectile.velocity != Vector2.Zero && projectile.type != proj.type && projectile.hostile && projectile.active && proj.Colliding(proj.getRect(), projectile.getRect()));
            if (projectile != null)
                projectile.velocity += projectile.Center.DirectionFrom(proj.Center);
        }
        public static void OrbitingPlayer(this Projectile projectile, float visualRotation, float r, float rotation)
        {
            projectile.rotation = rotation - MathHelper.PiOver2 * visualRotation;

            var desiredPosition = rotation.ToRotationVector2() * r;

            projectile.Center = Main.player[projectile.owner].Center + desiredPosition;
        }
    }
}
