using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace TheBindingOfRarria.Content.Projectiles;
public class PharmaceuticBullet : ModProjectile
{
    public override string Texture => ContentPath + "Projectiles/" + Name;

    public override void SetDefaults()
    {
        Projectile.width = 2; 
        Projectile.height = 2;
        Projectile.aiStyle = 1;
        Projectile.friendly = true; 
        Projectile.DamageType = DamageClass.Ranged;
        Projectile.alpha = 255;
        Projectile.light = 0.2f;
        Projectile.ignoreWater = true; 
        Projectile.extraUpdates = 2;
        Projectile.scale = 1.33f;

        AIType = ProjectileID.Bullet;
    }

    public override bool PreDraw(ref Color lightColor)
    {
        Texture2D texture = TextureAssets.Projectile[Type].Value;

        Vector2 drawOrigin = new(texture.Width * 0.5f, texture.Height * 0.5f);

        Vector2 drawPos = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);
        Color color = Projectile.GetAlpha(lightColor);
        Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation - PiOver2, drawOrigin, Projectile.scale, SpriteEffects.None, 0);


        return false;
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        var player = Main.player[Projectile.owner];

        if (player.HasBuff(BuffID.PotionSickness))
        {
            player.buffTime[Array.FindIndex(player.buffType, e => e == BuffID.PotionSickness)] = Math.Max(1, player.buffTime[Array.FindIndex(player.buffType, e => e == BuffID.PotionSickness)] - 8);
        }
    }

    public override void OnKill(int timeLeft)
    {
        Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
        SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
    }
}