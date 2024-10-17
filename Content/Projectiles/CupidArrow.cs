using Terraria;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Items;

namespace TheBindingOfRarria.Content.Projectiles
{
    public class CupidTransformProj : GlobalProjectile
    {
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
        {
            return entity.arrow;
        }
        public override bool InstancePerEntity => true;
        public bool IsCupidated = false;
        public override void AI(Projectile projectile)
        {
            if (Main.player[projectile.owner].GetModPlayer<CupidArrowPlayer>().HasTheThing && !IsCupidated && projectile.penetrate != -1)
            {
                IsCupidated = true;
                projectile.penetrate++;
                projectile.damage = (int)(projectile.damage * 0.66f);
                projectile.usesLocalNPCImmunity = true;
                projectile.localNPCHitCooldown = 20;
            }
        }
    }
}