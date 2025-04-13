using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using TheBindingOfRarria.Common.Systems;

namespace TheBindingOfRarria.Content.Projectiles;

public class HeavyBlowThing : ModProjectile
{
    public override string Texture => ContentPath + "Projectiles/" + Name;

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
        float time = (20 - Projectile.timeLeft) * 0.1f;
        Projectile.scale = 0.2f + (Projectile.ai[0] * 0.2f + float.Pow(float.Sin((time + 0.25f) * PiOver2), 2));
        
        Projectile.rotation = Projectile.velocity.ToRotation() + PiOver2;
    }

    public override bool ShouldUpdatePosition() => false;

    public override bool PreDraw(ref Color lightColor)
    {
        Texture2D texture = TextureAssets.Projectile[Type].Value;

        float scale = Projectile.scale / 9;

        Color color = Color.White;
        color.A = (byte)Math.Min(228, Projectile.scale * 200);

        PixellationSystem.QueuePixelationAction(() => {
            Main.EntitySpriteDraw(texture, Projectile.Center + Projectile.velocity * 4 - Main.screenPosition, null, color, Projectile.rotation + PiOver2, texture.Size() * 0.5f, scale, SpriteEffects.None, 0);
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, color, Projectile.rotation, texture.Size() * 0.5f, scale * 1.5f, SpriteEffects.None, 0);
        }, PixellationSystem.RenderType.Additive);

        return false;
    }
}