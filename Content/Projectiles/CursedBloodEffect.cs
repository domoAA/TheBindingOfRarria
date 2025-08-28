using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using TheBindingOfRarria.Common.Helpers;

namespace TheBindingOfRarria.Content.Projectiles;

public class CursedBloodEffect : ModProjectile
{
    public override string Texture => ContentPath + "Projectiles/" + Name;

    public override void SetStaticDefaults() => Main.projFrames[Type] = 4;

    public override void SetDefaults()
    {
        Projectile.penetrate = -1;
        Projectile.ignoreWater = true;
        Projectile.tileCollide = false;
        Projectile.width = 64;
        Projectile.height = 64;
        Projectile.damage = 0;
        Projectile.netImportant = true;
        Projectile.timeLeft = 360;
    }

    public override void AI() => Projectile.CenteredOnPlayer();

    public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
    {
            // also why twice like what ?
        Main.instance.DrawCacheNPCsOverPlayers.Add(index);
        overPlayers.Add(index);
    }

    public override bool PreDraw(ref Color lightColor)
    {
        Texture2D texture = TextureAssets.Projectile[Type].Value;

        int frame = Math.Max(0, 4 - Projectile.timeLeft % 20 / 4);
        Rectangle rect = texture.Frame(1, 5, 0, frame, 0, -2);

        Color color = Color.White;

            // Why not use the origin arg ????
        Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition - (Projectile.Size * 0.5f), rect, color);
        
        return false;
    }
}