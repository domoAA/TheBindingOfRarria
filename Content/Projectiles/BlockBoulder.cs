
namespace TheBindingOfRarria.Content.Projectiles;

public class BlockBoulder : ModProjectile
{
    public override void SetDefaults()
    {
        Projectile.tileCollide = false;
        Projectile.penetrate = -1;
        Projectile.ignoreWater = true;
        Projectile.friendly = true;
        Projectile.width = 64;
        Projectile.height = 64;
        Projectile.timeLeft = 120;
        Projectile.netImportant = true;
    }
    public override void AI()
    {
        var owner = Main.player[Projectile.owner];

        if (owner.active) 
        { 
            owner.Center = Projectile.Center;
            owner.mount.Dismount(owner);
            owner.RemoveAllGrapplingHooks();
            owner.velocity *= 0;
        }
        else
            Projectile.Kill();

        Projectile.ReflectProjectiles();
    }
    public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
    {
        base.DrawBehind(index, behindNPCsAndTiles, behindNPCs, behindProjectiles, overPlayers, overWiresUI);
        Main.instance.DrawCacheNPCsOverPlayers.Add(index);
        overPlayers.Add(index);
    }
    public override bool PreDraw(ref Color lightColor)
    {
        Projectile.scale = 2;

        Main.EntitySpriteDraw(Projectile.MyTexture(), Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, Projectile.MyTexture().Size() / 2, Projectile.scale, SpriteEffects.None);

        return false;
    }
}