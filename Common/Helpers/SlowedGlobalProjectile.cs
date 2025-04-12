using Terraria;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Projectiles;

public class SlowedGlobalProjectile : GlobalProjectile
{
    public override bool InstancePerEntity => true;
    public (TheBindingOfRarria.State, int) Slowed = (TheBindingOfRarria.State.Default, 0);

    public override void AI(Projectile projectile)
    {
        if (Slowed.Item1 != TheBindingOfRarria.State.Default)
        {
            if (Slowed.Item1 == TheBindingOfRarria.State.Slow)
                projectile.velocity *= 0.97f;

            else if (Slowed.Item1 == TheBindingOfRarria.State.Fast)
                projectile.velocity *= 1.03f;

            projectile.netUpdate = true;
        }
    }
    public override bool PreDraw(Projectile projectile, ref Color lightColor)
    {
        if (Slowed.Item1 == TheBindingOfRarria.State.Slow)
            lightColor.A = 130;

        Slowed.Item2--;
        if (Slowed.Item2 <= 0)
            Slowed.Item1 = TheBindingOfRarria.State.Default;

        return base.PreDraw(projectile, ref lightColor);
    }
}
