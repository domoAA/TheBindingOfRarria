using System;
using System.Collections.Generic;

namespace TheBindingOfRarria.Content.Projectiles;

public class HandBallistaProj : ModProjectile
{
    public override string Texture => ContentPath + "Items/HandBallista";

    public override void SetStaticDefaults() => Main.projFrames[Type] = 4;

    public override void SetDefaults()
    {
        Projectile.penetrate = -1;
        Projectile.ignoreWater = true;
        Projectile.tileCollide = false;
        Projectile.width = 100;
        Projectile.height = 100;
        Projectile.damage = 0;
        Projectile.netImportant = true;
    }

    public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
    {
        overPlayers.Add(index);
    }

    public override bool ShouldUpdatePosition()
    {
        return false;
    }

    public override void AI()
    {
        Projectile.ai[0] += 0.1f;

        if (Projectile.ai[0] > 5)
            Projectile.Kill();
    }

    public override bool PreDraw(ref Color lightColor)
    {
        Texture2D texture = TextureAssets.Projectile[Type].Value;
        Projectile.spriteDirection = float.Sign(Projectile.velocity.X);

        int frame = Math.Max(0, (int)Projectile.ai[0] % 4);
        Rectangle rect = texture.Frame(1, 4, 0, frame, 0, -2);

        Color color = Color.White;

        Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, rect, color, Projectile.velocity.ToRotation() - (Projectile.spriteDirection - 1) / 2 * PI, Projectile.Size * 0.5f, Projectile.scale, Projectile.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0); ;

        return false;
    }
}