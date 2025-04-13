using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Projectiles;

public class RepellingPulse : ModProjectile
{
    public override string Texture => ContentPath + "Projectiles/" + Name;

    public override void SetDefaults()
    {
        Projectile.tileCollide = false;
        Projectile.penetrate = -1;
        Projectile.ignoreWater = true;
        Projectile.width = 50;
        Projectile.height = 50;
        Projectile.friendly = true;
    }

    private enum State
    {
        Contracting,
        Expanding
    }

    private State state = State.Expanding;

    public override void AI()
    {
        if (!Projectile.hostile)
            Projectile.CenteredOnPlayer();

        var rand = Main.rand.NextFloat() * 1.5f + 0.1f;
        Projectile.ai[0] += state == State.Expanding ? 0.01f * rand : -0.01f * rand;
        if (Projectile.ai[0] > 1)
            state = State.Contracting;
        else if (Projectile.ai[0] < 0.5f)
            state = State.Expanding;

        Projectile.scale = Projectile.ai[0];
        Projectile.width = (int)(100 * Projectile.scale);
        Projectile.height = (int)(100 * Projectile.scale);
        Projectile.ProjectileRepelling();

        Lighting.AddLight(Projectile.Center, Color.DeepSkyBlue.ToVector3() * Projectile.scale * 0.5f);
        Projectile.netUpdate = true;
    }

    public override bool PreDraw(ref Color lightColor)
    {
        Texture2D texture = TextureAssets.Projectile[Type].Value;

        Color color = Projectile.friendly ? Color.DeepSkyBlue : Color.DarkBlue;

        color *= 150f / 255f;
        Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, color with { A = 0 }, Projectile.rotation, texture.Size() * 0.5f, Projectile.scale * 0.5f, SpriteEffects.None, 0);

        Lighting.AddLight(Projectile.Center, Color.DeepSkyBlue.ToVector3() * Projectile.ai[0] / 3);

        return false;
    }
}