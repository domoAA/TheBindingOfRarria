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

public class MagicSphere : ModProjectile
{
    public override string Texture => ContentPath + "Projectiles/" + Name;

    public override void SetDefaults()
    {
        Projectile.tileCollide = false;
        Projectile.ignoreWater = true;
        Projectile.width = 50;
        Projectile.height = 50;
        Projectile.scale = 1;
        Projectile.alpha = 255;
    }

    public override bool PreDraw(ref Color lightColor)
    {
        Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
        Vector2 drawPos = Projectile.Center - Main.screenPosition + new Vector2(0, Main.player[Projectile.owner].gfxOffY);


        var effect = Effects.ShpereShader?.Value;


        Main.spriteBatch.End(out var snapshot);
        Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, Main.Rasterizer, effect, snapshot.matrix);

        var scale = Main.LocalPlayer.height / (texture.Height * 0.5f);
        
        effect?.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly);
        effect?.Parameters["uSize"].SetValue(texture.Size() * scale);
        effect?.Parameters["uColor"].SetValue((Color.RoyalBlue with { A = (byte)(250f * Projectile.ai[0]) }).ToVector4());
        effect?.CurrentTechnique.Passes[0].Apply();
        

        Main.spriteBatch.Draw(texture, drawPos, texture.Bounds, Color.Red, 0, texture.Size() / 2, scale, SpriteEffects.None, 0);

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
        overPlayers.Add(index);
    }

    public override void AI()
    {
        Projectile.CenteredOnPlayer();

        var owner = Main.player[Projectile.owner];
        Projectile.ai[0] = owner.GetModPlayer<ManaShieldPLayer>().power;
    }
}