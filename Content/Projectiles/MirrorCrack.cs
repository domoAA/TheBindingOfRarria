using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Projectiles
{
    public class MirrorCrack : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.width = 30;
            Projectile.height = 80;
            Projectile.damage = 0;
            Projectile.netImportant = true;
            Projectile.timeLeft = 120;
            Projectile.scale = 2;
        }
        public override void AI()
        {
            if (Projectile.timeLeft > 100)
                Projectile.ReflectProjectiles(true, 1);

        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.ai[1]++;
            byte dimming = (byte)(128 - (byte)Projectile.ai[1]);
            var effect = Projectile.ai[2] == 0 ? Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipVertically : Microsoft.Xna.Framework.Graphics.SpriteEffects.None;
            Projectile.DrawWithTransparency(lightColor, dimming, effect);
            return false;
        }
    }
}