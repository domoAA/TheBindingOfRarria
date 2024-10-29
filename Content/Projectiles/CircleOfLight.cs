using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using System;

namespace TheBindingOfRarria.Content.Projectiles
{
    public class CircleOfLight : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.width = 60;
            Projectile.height = 60;
        }
        public override void AI()
        {
            Projectile.timeLeft = 3;
            Projectile.ReflectProjectiles(true, 0.3f);
            Projectile.CenteredOnPlayer();
            Projectile.width = (int)(100 * Projectile.scale);
            Projectile.height = (int)(100 * Projectile.scale);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.ai[1] = Projectile.ai[1]++ % 255;
            byte alpha = 150; //(byte)Projectile.ai[1];
            Projectile.DrawWithTransparency(Color.LightYellow, alpha);
            return false;
        }
    }
}