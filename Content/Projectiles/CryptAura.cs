
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using TheBindingOfRarria.Common.Helpers;
using TheBindingOfRarria.Content.Dusts;
using TheBindingOfRarria.Common.Registries;

namespace TheBindingOfRarria.Content.Projectiles;

public class CryptAura : ModProjectile
{
    public override string Texture => ContentPath + "Projectiles/" + Name;

    public override void SetDefaults()
    {
        Projectile.friendly = true;
        Projectile.timeLeft = 600;
        Projectile.tileCollide = false;
        Projectile.ignoreWater = true;
        Projectile.alpha = 100;
        Projectile.width = 52;
        Projectile.height = 52;
        Projectile.scale = 3;
    }

    public override bool PreDraw(ref Color lightColor)
    {
        Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
        Vector2 drawPos = Projectile.Center - Main.screenPosition;
        float scale = Projectile.scale;

        var pulse = float.Sin(Projectile.ai[0] * Pi / 60f) * 40;

        texture.DrawWithTransparency(drawPos, scale * 2, Color.Green, (byte)(210 + pulse));



        texture = Textures.Crypt.Value;

        var frame = (int)float.Floor(Projectile.ai[0] % 60 / 15);
        var rect = texture.Frame(1, 4, 0, frame);

        texture.DrawWithTransparency(drawPos + new Vector2(0, rect.Height * 4.5f), rect, scale * 2, Color.MediumSpringGreen, (byte)(30 + pulse / 2));

        return false;
    }

    public override bool ShouldUpdatePosition()
    {
        return false;
    }

    public override void AI()
    {
        float radius = 80f;

        if (Projectile.ai[0] % 3 == 0)
            Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<GlorpyHealingPlus>(), 0, 0, 100, Color.MediumSpringGreen with { A = 150 }, 1.75f);
        

        Projectile.ai[0]++;
        if (Projectile.ai[0] % 60 == 0)
        {
            float healRadiusSquared = radius * radius;
            foreach (Player player in Main.player)
            {
                if (player.active && !player.dead && Vector2.DistanceSquared(Projectile.Center, player.Center) <= healRadiusSquared)
                {
                    player.Heal(5);
                }
            }
        }
    }
}