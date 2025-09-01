using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Common.Helpers;
using TheBindingOfRarria.Content.Dusts;

namespace TheBindingOfRarria.Content.Projectiles;

public class Cleave : ModProjectile
{
    public override string Texture => ContentPath + "Projectiles/" + Name;

    public override void SetStaticDefaults()
    {
        Projectile.tileCollide = false;
        Projectile.ignoreWater = true;
        Projectile.penetrate = -1;
        Projectile.friendly = true;
        Projectile.DamageType = DamageClass.Melee;
        Projectile.height = 48;
        Projectile.width = 56;
        Projectile.maxPenetrate = -1;
        Projectile.timeLeft = 6;
        Projectile.usesIDStaticNPCImmunity = true;
        Projectile.idStaticNPCHitCooldown = 15;
    }

    private bool cleaved = false;

    public override bool ShouldUpdatePosition() => false;
    
    public override bool? CanHitNPC(NPC target) => false;
    
    public override void AI()
    {
        if (Projectile.ai[0] < 4)
            Projectile.ai[0] += 0.3f;
        else
            Projectile.Kill();

        Vector2 a = new(Projectile.ai[1], Projectile.ai[2]);
        Projectile.rotation = Projectile.Center.DirectionFrom(a).ToRotation() + PiOver2;

        if (!cleaved) {
            Projectile.scale = float.Sqrt(Projectile.Center.Distance(a)) / 12;
            cleaved = true;
            Color color = Color.DarkGray;

            a.SpawnDust(ModContent.DustType<PixellatedDustE98>(), 8f * Projectile.scale, 0.7f * Projectile.scale, color, 7, 35, 0.9f, a.DirectionFrom(Projectile.Center).ToRotation() + PiOver2, 2, -0.05f);

            SoundStyle sound = SoundID.Item14;
            sound.Pitch -= 0.4f;
            sound.Volume *= 0.5f;
            SoundEngine.PlaySound(sound, a);
        }

        Vector2 b = (Projectile.Center - Main.screenPosition + Projectile.Center.DirectionFrom(a).RotatedBy(PiOver2) * 66 * Projectile.scale - new Vector2(Main.screenWidth / 2, Main.screenHeight / 2)) * Main.GameZoomTarget + new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
        Vector2 c = (Projectile.Center - Main.screenPosition + Projectile.Center.DirectionFrom(a).RotatedBy(-PiOver2) * 66 * Projectile.scale - new Vector2(Main.screenWidth / 2, Main.screenHeight / 2)) * Main.GameZoomTarget + new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);

        foreach (var target in Main.ActiveNPCs)
        {
            if (target.friendly || !target.CanBeChasedBy() && target.immortal || target.immune[Projectile.owner] > 0 || target.aiStyle == NPCAIStyleID.DD2MysteriousPortal)
                continue;

            for (int i = 4; i > 0; i--)
            {
                Vector2 pos = i switch
                {
                    0 => new(target.position.X + target.width, target.position.Y + target.height),
                    1 => new(target.position.X + target.width, target.position.Y),
                    2 => new(target.position.X, target.position.Y + target.height),
                    _ => target.position,
                };

                if (LightCone.IsPointInTriangle(pos - Main.screenPosition, a - Main.screenPosition, b, c))
                {
                    var info = target.CalculateHitInfo(Projectile.damage, Projectile.direction, Main.rand.Next(101) < Projectile.CritChance, 5, Projectile.DamageType, true, Main.player[Projectile.owner].luck);
                    target.StrikeNPC(info);
                    NetMessage.SendStrikeNPC(target, info);
                    target.immune[Projectile.owner] += 15;
                    break;
                }
            }
        }
    }
    public override bool PreDraw(ref Color lightColor) => false;
}