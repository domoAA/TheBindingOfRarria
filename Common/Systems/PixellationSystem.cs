using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Common.Systems;

    // Rewrite this.
public class PixellationSystem : ModSystem
{
        // Credits for this whole class to Naka, and thanks to petrichor: i got my hands on this gem thanks to them
    public enum RenderType
    {
        AlphaBlend,
            // NonPremultiplied,
        Additive
    }

    private static List<Action> DrawActions { get; } = [];
    private static List<Action> DrawActionsAdditive { get; } = [];
    private static List<Action> PrimitiveActions { get; } = [];
    private static RenderTarget2D AlphaBlendTarget { get; set; }
    private static RenderTarget2D AdditiveTarget { get; set; }
    private static RenderTarget2D PrimitiveTarget { get; set; }

    public override void Load()
    {

        if (!Main.dedServ)
        {
            Main.OnResolutionChanged += InitializeRT;
            Main.RunOnMainThread(() =>
            {
                AlphaBlendTarget = new(Main.instance.GraphicsDevice, Main.screenWidth / 2, Main.screenHeight / 2);
                AdditiveTarget = new(Main.instance.GraphicsDevice, Main.screenWidth / 2, Main.screenHeight / 2);
                PrimitiveTarget = new(Main.instance.GraphicsDevice, Main.screenWidth / 2, Main.screenHeight / 2);
            });
        }
        On_Main.DrawProjectiles += On_Main_DrawProjectiles;
        On_Main.CheckMonoliths += DrawToRT;
    }

    public override void Unload()
    {
        if (!Main.dedServ)
            Main.OnResolutionChanged -= InitializeRT;

        On_Main.DrawProjectiles -= On_Main_DrawProjectiles;
        On_Main.CheckMonoliths -= DrawToRT;
    }

    private void DrawToRT(On_Main.orig_CheckMonoliths orig)
    {
        orig.Invoke();

            // has to go here bc order of execution
        var gd = Main.graphics.GraphicsDevice;
        var oldRTs = gd.GetRenderTargets();

        gd.SetRenderTarget(AlphaBlendTarget);
        gd.Clear(Color.Transparent);

        Main.spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend, Main.DefaultSamplerState, default, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

        foreach (var action in DrawActions)
            action.Invoke();

        Main.spriteBatch.End();

        gd.SetRenderTarget(PrimitiveTarget);

        Main.spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend, Main.DefaultSamplerState, default, Main.Rasterizer, null, Matrix.Identity);

        gd.Clear(Color.Transparent);

        foreach (var action in PrimitiveActions)
            action.Invoke();

        Main.spriteBatch.End();

        gd.SetRenderTarget(AdditiveTarget);

        Main.spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive, Main.DefaultSamplerState, default, Main.Rasterizer, null, Matrix.Identity);

        gd.Clear(Color.Transparent);

        foreach (var action in DrawActionsAdditive)
            action.Invoke();

        Main.spriteBatch.End();

        gd.SetRenderTargets(oldRTs);

        DrawActions.Clear();
        DrawActionsAdditive.Clear();
        PrimitiveActions.Clear();
    }
    public static void DrawPixelPrimitive(Action action)
    {
        PrimitiveActions.Add(action);
    }
    private void InitializeRT(Vector2 obj)
    {
        if (Main.dedServ)
            return;

        AlphaBlendTarget?.Dispose();
            // NonPremultipliedTarget?.Dispose();
        AdditiveTarget?.Dispose();
        PrimitiveTarget?.Dispose();

        GraphicsDevice gd = Main.instance.GraphicsDevice;
        int width = Main.screenWidth / 2;
        int height = Main.screenHeight / 2;

        AlphaBlendTarget = new(gd, width, height);
        AdditiveTarget = new(gd, width, height);
        PrimitiveTarget = new(gd, width, height);
    }

    private void On_Main_DrawProjectiles(On_Main.orig_DrawProjectiles orig, Main self)
    {
        orig(self);

        DrawRT();
    }

    /// <summary>
    /// Queues a draw action to the pixelation system.
    /// Remember to halve the scale and draw position!
    /// </summary>
    /// <param name="action"></param>
    public static void QueuePixelationAction(Action action, RenderType type)
    {
        switch (type)
        {
            case RenderType.Additive:
                DrawActionsAdditive.Add(action);
                break;

            case RenderType.AlphaBlend:
                DrawActions.Add(action);
                break;
        }
    }

    /// <summary>
    /// Draws the RT with its pixelated content to 2x scale
    /// </summary>
    private static void DrawRT()
    {
        SpriteBatch sb = Main.spriteBatch;

        if (AlphaBlendTarget == null || AlphaBlendTarget.IsDisposed)
            return;

        sb.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Matrix.Identity);

        sb.Draw(AlphaBlendTarget, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);
        sb.End();

        sb.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Matrix.Identity);

        sb.Draw(PrimitiveTarget, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);
        sb.End();

        if (AdditiveTarget == null || AdditiveTarget.IsDisposed)
            return;

        sb.Begin(SpriteSortMode.Texture, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

        sb.Draw(AdditiveTarget, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);

        sb.End();
    }
}