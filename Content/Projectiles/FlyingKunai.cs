
namespace TheBindingOfRarria.Content.Projectiles
{
    public class FlyingKunai : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Type] = 2;
            ProjectileID.Sets.TrailCacheLength[Type] = 10;
        }
        public override void SetDefaults()
        {
            Projectile.ArmorPenetration = 10;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Default;
            Projectile.extraUpdates = 1;
            Projectile.tileCollide = false;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            if (Projectile.oldPos.Last() != Vector2.Zero)
                Projectile.tileCollide = true;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            var texture = Terraria.GameContent.TextureAssets.Projectile[ProjectileID.ThrowingKnife].Value;
            var color = lightColor;
            var darkColor = color.MultiplyRGB(Color.DarkGray);
            darkColor.A = 200;
            var brightColor = color.MultiplyRGB(Color.GhostWhite);
            brightColor.A = 200;
            var p = Projectile;

            Main.EntitySpriteDraw(texture, p.Center - Main.screenPosition, null, darkColor, p.rotation, texture.Size() / 2, p.scale, SpriteEffects.None);

            QueuePixelationAction(() =>
            {
                for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[p.type] - 1; i++)
                {
                    if (p.oldPos[i + 1] != Vector2.Zero)
                    {
                        Main.EntitySpriteDraw(texture, (p.oldPos[i] + (p.Size / 2) - Main.screenPosition) / 2, new Rectangle(0, 8, 14, 2), darkColor * (1 - ((i - 1) / (float)ProjectileID.Sets.TrailCacheLength[p.type])), p.oldPos[i].DirectionFrom(p.oldPos[i + 1]).ToRotation() + MathHelper.PiOver2, new Vector2(7, 0), new Vector2((1f - (i / (float)ProjectileID.Sets.TrailCacheLength[p.type])) * p.scale * 0.75f, p.oldPos[i].Distance(p.oldPos[i + 1]) / 2) / 2, SpriteEffects.None);
                        Main.EntitySpriteDraw(texture, (p.oldPos[i] + (p.Size / 2) - Main.screenPosition) / 2, new Rectangle(0, 8, 14, 2), brightColor * (0.5f - ((i - 1) / (float)ProjectileID.Sets.TrailCacheLength[p.type])), p.oldPos[i].DirectionFrom(p.oldPos[i + 1]).ToRotation() + MathHelper.PiOver2, new Vector2(7, 0), new Vector2((1f - (i / (float)ProjectileID.Sets.TrailCacheLength[p.type])) * p.scale, p.oldPos[i].Distance(p.oldPos[i + 1]) / 2) / 2, SpriteEffects.None);
                    }
                }
            }, RenderType.Additive);


            Main.EntitySpriteDraw(texture, p.Center - Main.screenPosition, null, brightColor, p.rotation, texture.Size() / 2, p.scale, SpriteEffects.None);

            return false;
        }
    }
}