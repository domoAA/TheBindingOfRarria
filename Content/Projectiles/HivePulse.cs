using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Projectiles
{
    public class HivePulse : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.timeLeft = 120;
            Projectile.netImportant = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.scale = 0.1f;
            Projectile.hostile = true;
        }
        public override void AI()
        {
            Projectile.width = (int)(110 * Projectile.scale);
            Projectile.height = (int)(110 * Projectile.scale);

            Projectile.ai[0] += 0.01f;
            Projectile.scale += Projectile.ai[0];

            for (int i = 3; i > 0; i--)
            {
                Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, Terraria.ID.DustID.Honey).noGravity = true;
            }
        }
        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            modifiers.Cancel();
            target.Heal(Projectile.damage);
            base.ModifyHitPlayer(target, ref modifiers);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.DrawWithTransparency(Color.LightGoldenrodYellow, 200);
            return false;
        }
    }
}