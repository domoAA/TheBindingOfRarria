using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Common.Registries;

namespace TheBindingOfRarria.Content.Projectiles;

public class FishBullet : ModProjectile
{
    private const int FishFrames = 66;

    public override void SetDefaults()
    {
        Projectile.width = 40;
        Projectile.height = 40;
        Projectile.scale = .6f;
        Projectile.friendly = true;
        Projectile.extraUpdates = 1;
        Projectile.timeLeft = 600;

        Projectile.DamageType = DamageClass.Ranged;

        Projectile.frame = Main.rand.Next(FishFrames);
    }

        // Archaic.
    public override void AI()
    {
        Projectile.ai[0] += 1f;
        Projectile.rotation = Projectile.velocity.ToRotation();

        if (Projectile.ai[0] > 60)
        {
            Projectile.ai[0] = 60;
            Projectile.velocity.Y += 0.05f;
        }

        if (Projectile.timeLeft == 0)
            Projectile.Kill();
    }

    public override void OnKill(int timeLeft)
    {
        SoundEngine.PlaySound(SoundID.Dig, Projectile.position);
        for (int i = 0; i < 5; i++)
        {
            Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Silver);
            dust.noGravity = true;
            dust.velocity *= 1.5f;
            dust.scale *= 0.9f;
        }
    }

        // God help me kain.
    public override bool PreDraw(ref Color lightColor)
    {
        Texture2D texture = Textures.Fish.Value;

        Rectangle frame = texture.Frame(1, FishFrames, 0, Projectile.frame);

        SpriteEffects flip = (SpriteEffects)(1 - Projectile.direction);

        Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, frame, lightColor, Projectile.rotation + (Projectile.direction * PiOver4), frame.Size() * 0.5f, 1, flip, 0);

        return false;
    }
}