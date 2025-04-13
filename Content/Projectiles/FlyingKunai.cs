using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Common.Systems;

namespace TheBindingOfRarria.Content.Projectiles;

public class FlyingKunai : ModProjectile
{
    public override string Texture => ContentPath + "Projectiles/" + Name;

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
        Projectile.rotation = Projectile.velocity.ToRotation() + PiOver2;

        if (Projectile.oldPos.Last() != Vector2.Zero)
            Projectile.tileCollide = true;
    }

    public override bool PreDraw(ref Color lightColor)
    {
        Texture2D texture = TextureAssets.Projectile[ProjectileID.ThrowingKnife].Value;

        Color color = lightColor;

        Color darkColor = color.MultiplyRGB(Color.DarkGray);
        darkColor.A = 150;

        Color brightColor = color.MultiplyRGB(Color.Gray);
        brightColor.A = 150;

        Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, darkColor, Projectile.rotation, texture.Size() * 0.5f, Projectile.scale, SpriteEffects.None);

        PixellationSystem.QueuePixelationAction(() =>
        {
            for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Type] - 1; i++)
            {
                if (Projectile.oldPos[i + 1] != Vector2.Zero)
                {
                    Vector2 position = Projectile.oldPos[i] + (Projectile.Size * 0.5f) - Main.screenPosition;

                        // Remove when you use a proper matrix.
                    position *= 0.5f;

                    float trailSize = ProjectileID.Sets.TrailCacheLength[Type];

                    float ratio = i / trailSize;
                    float colorRatio = (i - 1) / trailSize;

                    float rotation = Projectile.oldPos[i].DirectionFrom(Projectile.oldPos[i + 1]).ToRotation() + PiOver2;

                    Vector2 origin = new(7, 0);
                    Vector2 scale = new Vector2((1f - ratio) * Projectile.scale, Projectile.oldPos[i].Distance(Projectile.oldPos[i + 1]) * 0.5f) * 0.5f;

                    Vector2 darkScale = scale;
                    darkScale.X *= 0.75f;


                    Main.EntitySpriteDraw(texture, position, new Rectangle(0, 8, 14, 2), darkColor * (1 - ratio), rotation, origin, darkScale, SpriteEffects.None);
                    Main.EntitySpriteDraw(texture, position, new Rectangle(0, 8, 14, 2), brightColor * (0.5f - ratio), rotation, origin, scale, SpriteEffects.None);
                }
            }
        }, PixellationSystem.RenderType.Additive);


        Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, brightColor, Projectile.rotation, texture.Size() * 0.5f, Projectile.scale, SpriteEffects.None);

        return false;
    }
}