using Terraria.Audio;
using TheBindingOfRarria.Common.Helpers;
using TheBindingOfRarria.Common.Systems;
using TheBindingOfRarria.Content.Dusts;

namespace TheBindingOfRarria.Content.Projectiles;

public class BallistaBolt : ModProjectile
{
    public override string Texture => Helper.GetVanillaProjectileTexture(ProjectileID.DD2BallistraProj);

    public override void SetDefaults()
    {
        Projectile.friendly = true;
        Projectile.penetrate = 3;
        Projectile.usesIDStaticNPCImmunity = true;
        Projectile.idStaticNPCHitCooldown = 20;
        Projectile.width = 5;
        Projectile.height = 5;
        Projectile.timeLeft = 900;
        Projectile.DamageType = DamageClass.Ranged;
        Projectile.aiStyle = ProjAIStyleID.Arrow;
        Projectile.scale = 1.33f;
        Projectile.extraUpdates = 1;
    }

    public override void AI()
    {
        if (Projectile.ai[2] != 0)
            Projectile.rotation = Projectile.ai[2];
    }

    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        Projectile.velocity = Vector2.Zero;
        Projectile.ai[2] = Projectile.rotation;
        return false;
    }

    public override bool? CanHitNPC(NPC target)
    {
        if (Projectile.velocity.LengthSquared() < 1)
            return false;

        return null;
    }

    public override bool PreDraw(ref Color lightColor)
    {
        var texture = TextureAssets.Projectile[Type].Value;

        var offset = (Projectile.rotation - PiOver2).ToRotationVector2() * texture.Width / 5 * Projectile.scale;

        Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + offset, null, lightColor, Projectile.rotation, texture.Size() / 2, Projectile.scale, SpriteEffects.None, 0);

        return false;
    }
}