using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Projectiles;

public class CircleOfLight : ModProjectile
{
    public override string Texture => ContentPath + "Projectiles/" + Name;

    private bool? shining = null;

    public override void SetDefaults()
    {
        Projectile.tileCollide = false;
        Projectile.penetrate = -1;
        Projectile.ignoreWater = true;
        Projectile.width = 60;
        Projectile.height = 60;
        Projectile.friendly = true;
        Projectile.netImportant = true;
        Projectile.light = 0.4f;
    }

    public override void AI()
    {
        Projectile.width = (int)(110 * Projectile.scale);
        Projectile.height = (int)(110 * Projectile.scale);

        Projectile.CenteredOnPlayer();
        Projectile.ReflectProjectiles(0.12f);

        if (Projectile.ai[0] != 0)
            shining = true;

        Projectile.ai[0] = 0;

        Projectile.ai[1] += shining == true ? 1f : shining == false ? -0.5f : 0;

        if (Projectile.ai[1] > 13)
            shining = false;
        else if (Projectile.ai[1] <= 0)
            shining = null;

        Projectile.netUpdate = true;
    }

    public override bool PreDraw(ref Color lightColor)
    {
        float scale = Projectile.scale * 0.5f;

        Texture2D texture = TextureAssets.Projectile[Type].Value;
        Color color = Color.LightYellow * ((7 + Projectile.ai[1]) * (1f / 255f));

        for (int i = 0; i < 6; i++)
        {
            scale -= 0.015f;
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, color with { A = 0 }, Projectile.rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0);
        }

        return false;
    }
}