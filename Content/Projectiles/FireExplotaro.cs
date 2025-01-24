using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Projectiles
{
    public class FireExplotaro : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Default;
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.friendly = true;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 30;
        }
        public override void AI()
        {
            if (Projectile.ai[0] < 4)
                Projectile.ai[0] += 0.3f;
            else
                Projectile.Kill();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            var num = (int)Projectile.ai[0];
            var texture = Projectile.MyTexture();
            var frame = texture.Frame(1, 5, 0, num, 0, -2);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + new Vector2(0, frame.Height + 2) * num, frame, lightColor, 0, frame.Center(), Projectile.scale * Main.GameZoomTarget, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
            return false;
        }
    }
}