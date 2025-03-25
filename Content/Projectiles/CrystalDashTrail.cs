
using System;

namespace TheBindingOfRarria.Content.Projectiles
{
    public class CrystalDashTrail : ModProjectile
    {
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
                Main.player[Projectile.owner].GetModPlayer<CrystalDashPlayer>().Dashing = false;
                Main.player[Projectile.owner].GetModPlayer<CrystalDashPlayer>().RandRot.Clear();
                for (int i = 0; i < 10; i++)
                {
                    Main.player[Projectile.owner].GetModPlayer<CrystalDashPlayer>().RandRot.Add(Main.rand.NextFloat(-PiOver4 / 4, PiOver4 / 4));
                }
            }


            var center = Projectile.Center + offset;
            //middle
            center.SpawnDust(ModContent.DustType<PixellatedDustE98>(), 1, 1f, Color.White with { A = 200 }, 2, 46, PiOver2 * 0.7f, PiOver4 * 1.5f + Pi * Math.Max(0, -Projectile.direction), 3, -0.1f);

            //front
            center.SpawnDust(ModContent.DustType<PixellatedDustE98>(), 2, 1.16f, Color.White with { A = 150 }, 5, 36, Pi * 0.7f, PiOver4 + Pi * Math.Max(0, -Projectile.direction) - PiOver2 * 0.4f, 2, -0.1f);

            //sides
            center.SpawnDust(ModContent.DustType<PixellatedDustE98>(), 2, 0.8f, Color.White with { A = 150 }, 2, 36, PiOver4 / 2, PiOver4 / 2 * (Projectile.direction + 1));
            center.SpawnDust(ModContent.DustType<PixellatedDustE98>(), 2, 0.8f, Color.White with { A = 150 }, 3, 36, PiOver4 / 2, Pi - PiOver4 * Projectile.direction + PiOver4 / 2 * (Projectile.direction + 1));

            //tail
            center.SpawnDust(ModContent.DustType<PixellatedDustE98>(), 9, 1.2f, Color.White with { A = 170 }, 2, 86, PiOver4 / 3, PiOver2 + PiOver4 / 2 * 1.3f + Pi * Math.Max(0, -Projectile.direction));

            Projectile.netUpdate = true;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
    }
}