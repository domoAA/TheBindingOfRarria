using Microsoft.Xna.Framework;
using System.Collections.Generic;
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
            Projectile.ai[0] = 0.1f;
            Projectile.hostile = true;
        }
        public override void AI()
        {
            Projectile.width = (int)(112 * Projectile.scale);
            Projectile.height = (int)(112 * Projectile.scale);

            Projectile.ai[0] += 0.05f;
            Projectile.scale = Projectile.ai[0];

            Projectile.Center = new Vector2(Projectile.ai[1], Projectile.ai[2]);

            Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, Terraria.ID.DustID.Honey).noGravity = true;
        }
        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            modifiers.Cancel();
            if (target.GetModPlayer<HiveHealedPlayer>().HealedByHives.Contains(Projectile.identity)) {
                base.ModifyHitPlayer(target, ref modifiers);
                return; }

            target.Heal(Projectile.damage);
            target.GetModPlayer<HiveHealedPlayer>().HealedByHives.Add(Projectile.identity);
            base.ModifyHitPlayer(target, ref modifiers);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.DrawWithTransparency(new Rectangle (0, 0, 256, 256), Color.Goldenrod, 1, 9, 1, 0.03f);
            return false;
        }
    }
    public class HiveHealedPlayer : ModPlayer
    {
        public List<int> HealedByHives = [];
    }
}