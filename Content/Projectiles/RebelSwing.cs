using TheBindingOfRarria.Common.Helpers;

namespace TheBindingOfRarria.Content.Projectiles;

public class RebelSwing : ModProjectile
{
    public override string Texture => ContentPath + "Projectiles/CEFist";

    public override void SetDefaults()
    {
        Projectile.width = 90;
        Projectile.height = 150;
        Projectile.friendly = false;
        Projectile.hostile = true;
        Projectile.DamageType = DamageClass.Default;
        Projectile.ignoreWater = true;
        Projectile.tileCollide = false;
        Projectile.penetrate = -1;
        Projectile.timeLeft = 10;
    }

    public override void AI()
    {
        for (int x = 0; x < Projectile.width / 16f; x++)
        {
            var pos = new Point(Projectile.BottomLeft.ToTileCoordinates().X + x, Projectile.BottomLeft.ToTileCoordinates().Y - 1);
            if (!WorldGen.SolidOrSlopedTile(pos.X, pos.Y))
            {
                WorldGen.PlaceTile(pos.X, pos.Y, TileID.CrackedBlueDungeonBrick, forced:true);
            }
        }
    }
}