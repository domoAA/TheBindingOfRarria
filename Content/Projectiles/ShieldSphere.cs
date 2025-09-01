using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Data;
using System.Diagnostics.Metrics;
using Terraria.ID;
using TheBindingOfRarria.Common.Helpers;
using TheBindingOfRarria.Common.Registries;
using TheBindingOfRarria.Common.Systems;
using TheBindingOfRarria.Content.Dusts;
using TheBindingOfRarria.Content.Items;

namespace TheBindingOfRarria.Content.Projectiles;

public class ShieldSphere : ModProjectile
{
    public override string Texture => ContentPath + "Items/ShieldTalisman";

    public override void SetDefaults()
    {
        Projectile.tileCollide = false;
        Projectile.ignoreWater = true;
        Projectile.width = 50;
        Projectile.height = 50;
        Projectile.scale = 1;
        Projectile.alpha = 255;
        Projectile.timeLeft = 50;
    }

    public override bool PreDraw(ref Color lightColor)
    {
        Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
        Vector2 drawPos = Projectile.Center - Main.screenPosition + new Vector2(0, Main.player[Projectile.owner].gfxOffY);


        Main.spriteBatch.End(out var snapshot);
        Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointWrap, DepthStencilState.Default, Main.Rasterizer, null, snapshot.matrix);

        var scale = Main.LocalPlayer.height / (texture.Height);
        var color = Color.White with { A = (byte)(255f - 5f * Projectile.ai[0]) };
        //color *= color.A / 255f;

        Main.spriteBatch.Draw(texture, drawPos, texture.Bounds, color, 0, texture.Size() / 2, scale, Projectile.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);

        Main.spriteBatch.End();
        Main.spriteBatch.Begin(snapshot);
        


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
        if (Projectile.ai[0] == 0)
            Projectile.spriteDirection = Main.player[Projectile.owner].direction;

        Projectile.ai[0]++;
    }
}