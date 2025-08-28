using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Common.Helpers;
using TheBindingOfRarria.Common.Registries;

namespace TheBindingOfRarria.Content.Projectiles;

public class PotMinion : ModProjectile
{
    public override string Texture => ContentPath + "Projectiles/" + Name;

    public override void SetStaticDefaults()
    {
        ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;

        Main.projPet[Projectile.type] = true;

        ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
    }

    public override void SetDefaults()
    {
        Projectile.timeLeft = 3600;
        Projectile.width = 24;
        Projectile.height = 24;
        Projectile.ignoreWater = true;
        Projectile.tileCollide = false;
        Projectile.friendly = true;
        Projectile.minion = true;
        Projectile.DamageType = DamageClass.Summon;
        Projectile.minionSlots = 0;
        Projectile.penetrate = -1;
    }

    public enum State
    {
        Idle,
        Teleporting,
        Attacking
    }

    public int Target = -1;

    public State state = State.Idle;

    public override void AI()
    {
        var owner = Main.player[Projectile.owner];

        Projectile.velocity = Projectile.Center.DirectionTo(owner.Center - new Vector2(0, 100).RotatedBy(Main.GlobalTimeWrappedHourly + Projectile.whoAmI));

        if (Projectile.ai[1] > 0)
            Projectile.ai[1]--;

        if (Projectile.ai[1] != 0)
            return;

        if (state == State.Idle && Projectile.ai[0] == 0 && Target == -1)
        {
            if (Projectile.Center.DistanceSQ(owner.Center) > 600 * 600)
            {
                state = State.Teleporting;
                Projectile.ai[0] = -20 - (Projectile.whoAmI % 5);
                Target = 0;
                return;
            }

            Projectile.Minion_FindTargetInRange(800, ref Target, false);

            if (Target != -1 && Main.npc[Target].GetGlobalNPC<PotMinionTargettedNPC>().TargettedBy != -1)
                Target = -1;
        }
        else
        {
            var enemy = Main.npc[Target];
            if (Projectile.ai[0] >= 0 && (!enemy?.active == true || (enemy?.GetGlobalNPC<PotMinionTargettedNPC>().TargettedBy > -1 && enemy?.GetGlobalNPC<PotMinionTargettedNPC>().TargettedBy != Projectile.identity)))
            {
                Projectile.ai[0] = 0;
                state = State.Idle;
                Target = -1;
                Projectile.ai[1] = 40;
                Projectile.rotation = 0;
                return;
            }

            if (state != State.Attacking && Projectile.ai[0] < 20 + (Projectile.whoAmI % 5))
            {
                if (Projectile.ai[0] >= 0)
                    enemy.GetGlobalNPC<PotMinionTargettedNPC>().TargettedBy = Projectile.identity;
                else
                    enemy.GetGlobalNPC<PotMinionTargettedNPC>().TargettedBy = -1;

                state = State.Teleporting;
                Projectile.ai[0]++;

                if (Projectile.ai[0] == 0)
                {
                    Projectile.Center = owner.Center - new Vector2(0, 100).RotatedBy(float.Sin(Main.GlobalTimeWrappedHourly));
                    state = State.Idle;
                    Target = -1;
                    enemy.GetGlobalNPC<PotMinionTargettedNPC>().TargettedBy = -1;
                    Projectile.ai[1] = 40;
                    Projectile.rotation = 0;
                }

                return;
            }
            else state = State.Attacking;

            Projectile.Center = enemy.Center - new Vector2(0, 150);

            Projectile.rotation = Pi;

            if (Projectile.ai[0] > -20 - (Projectile.whoAmI % 5))
            {
                Projectile.ai[0]--;
                return;
            }

            state = State.Teleporting;


            if (owner.whoAmI == Main.myPlayer)
                Projectile.NewProjectile(Projectile.GetProjectileSource_FromThis(), Projectile.Center, new Vector2(0, 5), ProjectileID.PewMaticHornShot, 10, 2, ai1: Main.rand.Next(Main.projFrames[ProjectileID.PewMaticHornShot]));
        }
    }

    public override bool? CanCutTiles()
    {
        return false;
    }

    public override bool PreDraw(ref Color lightColor)
    {
        if (state == State.Teleporting && float.Abs(Projectile.ai[0]) < 10)
        {
            Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Fireworks).noGravity = true;
        }

        return base.PreDraw(ref lightColor);
    }
}

public class PotMinionTargettedNPC : GlobalNPC
{
    public override bool InstancePerEntity => true;

    public int TargettedBy = -1;
}