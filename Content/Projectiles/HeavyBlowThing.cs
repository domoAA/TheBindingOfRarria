
namespace TheBindingOfRarria.Content.Projectiles
{
    public class HeavyBlowThing : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 72;
            Projectile.tileCollide = false;
            Projectile.damage = 0;
            Projectile.ignoreWater = true;
            Projectile.light = 0.1f;
            Projectile.timeLeft = 20;
            Projectile.scale = 0.2f;
        }
        public override void AI()
        {
            var time = (20 - Projectile.timeLeft) * 0.1f;
            Projectile.scale = 0.2f + (Projectile.ai[0] * 0.2f + float.Pow(float.Sin((time + 0.25f) * MathHelper.Pi / 2), 2));
            
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            var texture = TextureAssets.Projectile[Type].Value;
            var scale = Projectile.scale / 9;
            var color = Color.White;
            color.A = (byte)(Math.Min(228, Projectile.scale * 200));

            QueuePixelationAction(() => {
                Main.EntitySpriteDraw(texture, (Projectile.Center + Projectile.velocity * 4 - Main.screenPosition) / 2, texture.Bounds, color, Projectile.rotation + MathHelper.PiOver2, texture.Size() / 2, scale * 2, SpriteEffects.None, 0);
                Main.EntitySpriteDraw(texture, (Projectile.Center - Main.screenPosition) / 2, texture.Bounds, color, Projectile.rotation, texture.Size() / 2, scale * 3, SpriteEffects.None, 0);
            }, RenderType.Additive);
            return false;
        }
    }
}