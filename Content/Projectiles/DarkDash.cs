using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Items;

namespace TheBindingOfRarria.Content.Projectiles;

public class DarkDash : ModProjectile
{
    public override string Texture => ContentPath + "Projectiles/" + Name;

    public override void SetStaticDefaults()
    {
        ProjectileID.Sets.TrailCacheLength[Type] = 5;
        ProjectileID.Sets.TrailingMode[Type] = 0;
    }

    public override void SetDefaults()
    {
        Projectile.ignoreWater = true;
        Projectile.tileCollide = false;
        Projectile.penetrate = -1;
        Projectile.width = 64;
        Projectile.height = 64;
        Projectile.timeLeft = DarkDashPlayer.DashDuration;
    }

    public override void AI()
    {
        var owner = Main.player[Projectile.owner];


        Projectile.oldPos[0] = owner.position;

        if (Projectile.oldPos.LastOrDefault().Distance(owner.position) < (8 * 5))
        {
            var player = Main.player[Projectile.owner].GetModPlayer<DarkDashPlayer>();

            player.counter = -DarkDashPlayer.DashCooldown;
        }

        for (int i = Projectile.timeLeft % 2; i > -1; i--)
        {
            Vector2 offset = new Vector2(Main.rand.NextFloat(-10f + i * 5, 10f - i * 5), Main.rand.NextFloat(-30f + i * 20, 30f - i * 20));
            Dust dust = Dust.NewDustPerfect(owner.Center + offset, DustID.Asphalt, Vector2.Zero, 0, Color.Black, 3f);
            dust.noGravity = true;
            dust.velocity = Vector2.Zero;
        }
    }

    public override bool PreDraw(ref Color lightColor) => false;
}