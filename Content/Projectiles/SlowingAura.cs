using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using TheBindingOfRarria.Common.Helpers;

namespace TheBindingOfRarria.Content.Projectiles;

public class SlowingAura : ModProjectile
{
    public override string Texture => ContentPath + "Projectiles/" + Name;

    private bool shining = true;

    public override void SetDefaults()
    {
        Projectile.tileCollide = false;
        Projectile.penetrate = -1;
        Projectile.ignoreWater = true;
        Projectile.width = 105;
        Projectile.height = 105;
    }

    public override void AI()
    {
        Projectile.width = (int)(105 * Projectile.scale);
        Projectile.height = (int)(105 * Projectile.scale);

        foreach (var proj in Main.ActiveProjectiles)
        {
            if (proj == null || proj.friendly)
                continue;

            if (proj.Colliding(proj.getRect(), Projectile.getRect()))
                proj?.GetSlowed(TheBindingOfRarria.State.Slow, 180);
        }

        foreach (var target in Main.ActiveNPCs)
        {
            if (target == null || target.friendly)
                continue;

            if (Projectile.Colliding(target.getRect(), Projectile.getRect()))
                target?.GetSlowed(TheBindingOfRarria.State.Slow, 60);
        }

        Projectile.CenteredOnPlayer();

        switch (Projectile.ai[1])
        {
            case > 4:
                shining = false;
                Projectile.ai[1] = 3.9f;
                break;
            case < -4:
                shining = true;
                Projectile.ai[1] = -3.9f;
                break;
            default:
                if (shining)
                    Projectile.ai[1] += 0.1f;
                else
                    Projectile.ai[1] -= 0.1f;
                break;
        }
    }

    public override bool PreDraw(ref Color lightColor)
    {
        byte alpha = (byte)(6 + Projectile.ai[1]);

        Projectile.scale = 3f;
            // Projectile.DrawWithTransparency(new Rectangle(0, 0, 256, 256), Color.LightYellow, alpha, 8, 2, 0.025f);

        float scale = Projectile.scale * 0.5f;

        Texture2D texture = TextureAssets.Projectile[Type].Value;

        Color color = Color.LightYellow * (alpha / 255f);

        for (int i = 0; i < 8; i++)
        {
            scale -= 0.025f;

            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, color with { A = 0 }, Projectile.rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0);
        }
        return false;
    }
}