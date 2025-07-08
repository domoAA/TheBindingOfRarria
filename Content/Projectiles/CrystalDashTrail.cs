using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Common.Helpers;
using TheBindingOfRarria.Content.Dusts;
using TheBindingOfRarria.Content.Items;

namespace TheBindingOfRarria.Content.Projectiles;

/*public class CrystalDashTrail : ModProjectile
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
        Projectile.friendly = true;
        Projectile.penetrate = -1;
        Projectile.usesIDStaticNPCImmunity = true;
        Projectile.idStaticNPCHitCooldown = 20;
        Projectile.width = 190;
        Projectile.height = 128;
        Projectile.DamageType = DamageClass.Default;
        Projectile.timeLeft = 20;
    }

    public override void AI()
    {
        Projectile.damage = 10;

        Projectile.direction = Main.player[Projectile.owner].direction;

        Projectile.CenteredOnPlayer();
        Vector2 offset = new(Projectile.width / 2 * Projectile.direction, 0);
        Projectile.Center -= offset;

        if (Projectile.oldPos.LastOrDefault().Distance(Projectile.position) < (8 * 5))
        {
            CrystalDashPlayer player = Main.player[Projectile.owner].GetModPlayer<CrystalDashPlayer>();

            player.Dashing = false;
            for (int i = 0; i < 10; i++)
            {
                player.RandomRotations[i] = Main.rand.NextFloat(-PiOver4 / 4, PiOver4 / 4);
            }
        }


        Vector2 center = Projectile.Center + offset;

        int type = ModContent.DustType<PixellatedDustE98>();

                // Fucking what.
                    // Kain I can't do more then refer you to lolxd.
                    // Que?

            // middle
        center.SpawnDust(type, 1, 1f, Color.White with { A = 200 }, 2, 46, PiOver2 * 0.7f, PiOver4 * 1.5f + Pi * Math.Max(0, -Projectile.direction), 3, -0.1f);

            // front
        center.SpawnDust(type, 2, 1.16f, Color.White with { A = 150 }, 5, 36, Pi * 0.7f, PiOver4 + Pi * Math.Max(0, -Projectile.direction) - PiOver2 * 0.4f, 2, -0.1f);

            // sides
        center.SpawnDust(type, 2, 0.8f, Color.White with { A = 150 }, 2, 36, PiOver4 / 2, PiOver4 / 2 * (Projectile.direction + 1));
        center.SpawnDust(type, 2, 0.8f, Color.White with { A = 150 }, 3, 36, PiOver4 / 2, Pi - PiOver4 * Projectile.direction + PiOver4 / 2 * (Projectile.direction + 1));

            // tail
        center.SpawnDust(type, 9, 1.2f, Color.White with { A = 170 }, 2, 86, PiOver4 / 3, PiOver2 + PiOver4 / 2 * 1.3f + Pi * Math.Max(0, -Projectile.direction));

        Projectile.netUpdate = true;
    }

    public override bool ShouldUpdatePosition() => false;

    public override bool PreDraw(ref Color lightColor) => false;
}*/