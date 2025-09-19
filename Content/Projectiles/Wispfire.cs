using TheBindingOfRarria.Common.Helpers;

namespace TheBindingOfRarria.Content.Projectiles;

public class Wispfire : ModProjectile
{
    public override string Texture => Helper.GetVanillaItemTexture(ItemID.NebulaPickup2);

    public override void SetStaticDefaults() => Main.projFrames[Type] = 4;

    public override void SetDefaults()
    {
        Projectile.tileCollide = false;
        Projectile.penetrate = -1;
        Projectile.ignoreWater = true;
        Projectile.width = 16;
        Projectile.height = 16;
    }

    public override void AI()
    {
        if (Projectile.ai[0] == 0f)
            Projectile.ai[1] = Main.player[0].ownedProjectileCounts[Type] - 1;

        if (Projectile.ai[1] > 7)
            Projectile.Kill();


        Projectile.ai[0]++;

        Projectile.frameCounter = (Projectile.frameCounter + 1) % 5;
        if (Projectile.frameCounter == 0)
            Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Type];

        float rotation = TwoPi / 6 * Main.GlobalTimeWrappedHourly + Pi / 3f + TwoPi / 3 * Projectile.ai[1];
        var r = 40;
        if (Projectile.ai[1] > 2)
        {
            r = 80;
            rotation = TwoPi / 4 * Main.GlobalTimeWrappedHourly + TwoPi / 5 * Projectile.ai[1];
        }

        Projectile.OrbitingPlayer(1.6f, r, rotation);
        Projectile.rotation = 0;
    }

    public override bool PreDraw(ref Color lightColor)
    { 

        return base.PreDraw(ref lightColor);
    }
}