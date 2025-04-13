using Terraria;
using Terraria.ModLoader;
using TheBindingOfRarria.Common.Helpers;
using TheBindingOfRarria.Content.Dusts;

namespace TheBindingOfRarria.Content.Projectiles;

public class CEFist : ModProjectile
{
    public override string Texture => ContentPath + "Projectiles/" + Name;

    public override void SetDefaults()
    {
        Projectile.ignoreWater = true;
        Projectile.tileCollide = false;
        Projectile.friendly = true;
        Projectile.penetrate = 1;
        Projectile.DamageType = DamageClass.Magic;
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = -1;
        Projectile.width = 30;
        Projectile.height = 30;
        Projectile.timeLeft = 132;
    }

    private NPC Target
    {
        get => Projectile.ai[0] == 0 ? null : Main.npc[(int)Projectile.ai[0]];
        set => Projectile.ai[0] = value == null ? 0 : value.whoAmI;
    }

    private Vector2 Offset = new(0, 0);

    public override void AI()
    {
        if (Target == null || !Target.active)
            Projectile.Kill();
        else if (Projectile.timeLeft > 12)
        {
            Offset = new Vector2(Projectile.ai[1], Projectile.ai[2]);
            Projectile.Center = Target.Center + Offset;
            Projectile.netUpdate = true;
        }
    }

    public override bool ShouldUpdatePosition() => false;

    public override bool? CanHitNPC(NPC target)
    {
        if (Projectile.timeLeft > 12 || target.whoAmI != Target.whoAmI)
            return false;

        Projectile.Center.SpawnDust(ModContent.DustType<PixellatedDustE98>(), 1.6f, 0.36f, Color.LightSeaGreen, 7, 25, 0.7f, Projectile.velocity.ToRotation() + PiOver2);
        return base.CanHitNPC(target);
    }

    public override bool PreDraw(ref Color lightColor) => false;
}