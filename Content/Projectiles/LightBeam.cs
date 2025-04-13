using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Common.Registries;
using TheBindingOfRarria.Common.Systems;

namespace TheBindingOfRarria.Content.Projectiles;

public class LightBeam : ModProjectile
{
    public override string Texture => ContentPath + "Projectiles/" + Name;

    private int CD = 0;

    public override void SetDefaults()
    {
        Projectile.tileCollide = false;
        Projectile.penetrate = -1;
        Projectile.ignoreWater = true;
        Projectile.width = 24;
        Projectile.height = 24;
        Projectile.timeLeft = 500;
        Projectile.friendly = true;
        Projectile.netImportant = true;
        Projectile.ai[2] = 0;

        ProjectileID.Sets.DrawScreenCheckFluff[Type] = 1000;
    }

    public override bool ShouldUpdatePosition() => false;

    public override void AI()
    {
        if (Projectile.timeLeft < 100)
            Projectile.Kill();

        else if (Projectile.timeLeft < 240)
            Projectile.timeLeft--;

        Projectile.scale = 2f;
        Projectile.width = (int)(24 * Projectile.scale);
        Projectile.height = (int)(24 * Projectile.scale);

        if (Projectile.timeLeft == 330)
        {
            SoundStyle sound = SoundID.DD2_WitherBeastAuraPulse;
            sound.Volume = 185f;
            sound.Pitch = -0.5f;
            SoundEngine.PlaySound(sound);
        }

        CD--;
        if (CD < 0)
            CD = 10;

        Projectile.netUpdate = true;
    }

    public override bool? CanHitNPC(NPC target)
    {
        if (CD > 0 || !Projectile.friendly || Projectile.timeLeft > 350)
            return false;

        LaserHit(target);

        return true;
    }

    public void LaserHit(NPC target)
    {
        float _ = float.NaN;
        if (Collision.CheckAABBvLineCollision(target.getRect().TopLeft(), target.getRect().Size(), Projectile.Center, Projectile.Center + Projectile.velocity, 24 * Projectile.scale, ref _))
            SyncedManualStrike(target);
    }

    public static void SyncedManualStrike(NPC target)
    {
        NPC.HitInfo info = new()
        {
            Damage = 30 - target.defense / 2,
            Knockback = 3,
            InstantKill = false,
            HideCombatText = false
        };

        target.StrikeNPC(info);

        NetMessage.SendStrikeNPC(target, info);
    }

    public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
    {
        Main.instance.DrawCacheNPCsOverPlayers.Add(index);
        overPlayers.Add(index);
    }

    public override bool PreDraw(ref Color lightColor)
    {
        var time = 400 - Projectile.timeLeft;
        float power = 0.0035f * (260 - (float.Pow(time - 150, 2) / 60));

        Vector2 position = Projectile.Center - Main.screenPosition;

        if (Projectile.timeLeft <= 360)
        {
            byte alpha = 30;

            PixellationSystem.QueuePixelationAction(() =>
            {
                Vector2 beamPosition = Projectile.Center - Main.screenPosition + Projectile.velocity.SafeNormalize(Vector2.Zero) * 7;

                float rotation = Projectile.velocity.ToRotation() + PiOver2 * 3;

                DrawLightBeam(Main.spriteBatch, Textures.BeamEnd.Value, Textures.BeamBody.Value, beamPosition, Color.LightYellow, alpha, 1, rotation, Projectile.velocity, new Vector2(power, 1), new Vector2(0.04f, 0f), 13);
            }, PixellationSystem.RenderType.Additive);
        }

        Texture2D texture = TextureAssets.Projectile[ModContent.ProjectileType<Extra98Bomb>()].Value;

        float scale = Projectile.scale * 0.5f * (float.Sin(power) * float.Sqrt(Math.Abs(float.Sin(power))) + 1f);

        for (int i = 0; i < 4; i++)
        {
            scale -= 0.02f;
            Main.spriteBatch.Draw(texture, position, null, Color.LightYellow with { A = 0 }, power * float.Sin(power), texture.Size() * 0.5f, scale, SpriteEffects.None, 0);
        }

        return false;
    }

        // ???????????????????????????????
    private static void DrawLightBeam(SpriteBatch spriteBatch, Texture2D textureEnd, Texture2D textureBody, Vector2 position, Color color, byte alpha, byte alphaStep, float rotation, Vector2 direction, Vector2 scale, Vector2 scaleStep, int layers)
    {
        color.A += alpha;

        // float rotation = projectile.velocity.ToRotation() + PiOver2 * 3;

        Texture2D texture = textureEnd;

        // Vector2 position = projectile.Center  - Main.screenPosition + projectile.velocity.SafeNormalize(Vector2.Zero) * 7;

        for (int parts = 2; parts > -1; parts--)
        {
            for (int i = layers; i > 0; i--)
            {
                color.A += alphaStep;
                scale -= scaleStep;

                spriteBatch.Draw(texture, position * 0.5f, null, color, rotation, texture.Size() * 0.5f, scale * 0.5f, SpriteEffects.None, 0);
            }

            scale += scaleStep * layers;
            color.A = alpha;

            position += direction;
            rotation += Pi;

            if (parts == 1)
            {
                texture = textureBody;
                position -= direction * 1.5f;
                scale.Y = (direction.Length() - textureEnd.Height) / textureBody.Height;
                scaleStep.Y = 0;
                layers--;
            }
        }
    }
}