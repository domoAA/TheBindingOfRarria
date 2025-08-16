
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Common.Helpers;
using TheBindingOfRarria.Common.Registries;
using TheBindingOfRarria.Common.Systems;
using TheBindingOfRarria.Content.Dusts;

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

        var pulse = float.Sin(Projectile.ai[0] * Pi / 60f) * 20;

        texture.DrawWithTransparency(drawPos, scale * 2, Color.Green, (byte)Math.Min(130 + pulse, 660 - Projectile.ai[0]));



        texture = Textures.Crypt.Value;

        var frame = (int)float.Floor(Projectile.ai[0] % 60 / 15);
        var rect = texture.Frame(1, 4, 0, frame);

        texture.DrawWithTransparency(drawPos + new Vector2(0, (rect.Height + 1) * 1.5f) * scale, rect, scale * 2, Color.MediumSpringGreen, (byte)Math.Min(30 + pulse, 620 - Projectile.ai[0]));

        return false;
    }

    public override bool ShouldUpdatePosition()
    {
        return false;
    }
    public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
    {
        Main.instance.DrawCacheNPCsOverPlayers.Add(index);
        overPlayers.Add(index);
    }

    public override void AI()
    {
        Projectile.ai[0]++;

        if (Projectile.ai[0] < 20) 
        {
            Projectile.scale = 3 - (2 - Projectile.ai[0] / 10);
            return; 
        }

        if (Projectile.ai[0] % 3 == 0)
            Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<GlorpyHealingPlus>(), 0, 0, 100, Color.MediumSpringGreen with { A = 150 }, 1.75f);


        float radius = Projectile.width / 2;

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