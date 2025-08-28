using Terraria;
using Terraria.ModLoader;
using TheBindingOfRarria.Common.Helpers;

namespace TheBindingOfRarria.Content.Projectiles;

public class ReflectiveRib : ModProjectile
{
    public override string Texture => ContentPath + "Projectiles/" + Name;

    public override void SetDefaults()
    {
        Projectile.width = 26;
        Projectile.height = 30;
        Projectile.friendly = true;
        Projectile.DamageType = DamageClass.Default;
        Projectile.ignoreWater = true;
        Projectile.tileCollide = false;
        Projectile.penetrate = -1;
    }

    public override void AI()
    {
        Projectile.ai[0]++;

        float rotation = TwoPi / 360 * Projectile.ai[0];
        Projectile.OrbitingPlayer(1.6f, 40, rotation);
        Projectile.ReflectProjectiles();

    }
}