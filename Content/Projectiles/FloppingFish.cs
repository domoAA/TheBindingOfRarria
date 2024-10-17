using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Projectiles
{
    public class FloppingFish : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
            Projectile.width = 40;
            Projectile.height = 24;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.timeLeft = 240;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            Projectile.friendly = true;
        }
        public override void AI()
        {
            Projectile.velocity *= 0.99f;
            Projectile.velocity.Y += 0.1f;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {

            Projectile.velocity = new Vector2(-2, -2);
            Projectile.velocity = Projectile.velocity.RotateRandom(MathHelper.PiOver2);
            return false;
        }
    }
}