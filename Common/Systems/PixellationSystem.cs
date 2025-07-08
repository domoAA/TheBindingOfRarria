using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Common.Systems;
public class PixellationSystem : ModSystem
{
    // credits for the base for this system to naka, also thanks to zen, stormytuna and some others I forgor about for helping me change this thing to fit my needs

    public enum RenderType
    {
        AlphaBlend,
        Additive
    }
    private static Queue<Action> DrawActions { get; } = new();
    private static Queue<Action> DrawActionsAdditive { get; } = new();
    private static Queue<Action> PrimitiveActions { get; } = new();
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

    private void DrawToRT(On_Main.orig_CheckMonoliths orig)
    {
        orig.Invoke();
        // has to go here bc order of execution
        var gd = Main.graphics.GraphicsDevice;
        var oldRTs = gd.GetRenderTargets();


        gd.SetRenderTarget(AlphaBlendTarget);
        gd.Clear(Color.Transparent);
        Main.spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Matrix.Identity);

        foreach (var action in DrawActions)
        {
            action.Invoke();
        }
        Main.spriteBatch.End();


        gd.SetRenderTarget(PrimitiveTarget);
        Main.spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend, Main.DefaultSamplerState, default, default, null, Matrix.Identity);
        gd.Clear(Color.Transparent);

        foreach (var action in PrimitiveActions)
        {
            action.Invoke();
        }
        Main.spriteBatch.End();
        gd.SetRenderTarget(AdditiveTarget);
        Main.spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive, Main.DefaultSamplerState, default, default, null, Matrix.Identity);
        gd.Clear(Color.Transparent);

        foreach (var action in DrawActionsAdditive)
        {
            action.Invoke();
        }
        Main.spriteBatch.End();


        gd.SetRenderTargets(oldRTs);
        DrawActions.Clear();
        DrawActionsAdditive.Clear();
        PrimitiveActions.Clear();
    }
    public static void DrawPixelPrimitive(Action action)
    {
        PrimitiveActions.Enqueue(action);
    }
    public override void Unload()
    {
        On_Main.DrawProjectiles -= On_Main_DrawProjectiles;
    }
    private void InitializeRT(Vector2 obj)
    {
        if (Main.dedServ)
        {
            return;
        }
        AlphaBlendTarget?.Dispose();
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
        orig.Invoke(self);
        DrawRT();
    }
    /// <summary>
    /// Queues a draw action to the pixelation system.
    /// Remember to NOT halve the scale and draw position!
    /// </summary>
    /// <param name="action"></param>
    public static void QueuePixelationAction(Action action, RenderType type)
    {
        switch (type)
        {
            case RenderType.Additive:
                DrawActionsAdditive.Enqueue(action);
                break;
            case RenderType.AlphaBlend:
                DrawActions.Enqueue(action);
                break;
        }
    }
    /// <summary>
    /// Draws the RT with its pixelated content to 2x scale
    /// </summary>
    private static void DrawRT()
    {
        if (AlphaBlendTarget == null || AlphaBlendTarget.IsDisposed)
        {
            return;
        }
        Main.spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
        Main.spriteBatch.Draw(AlphaBlendTarget, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);
        Main.spriteBatch.End();

        Main.spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
        Main.spriteBatch.Draw(PrimitiveTarget, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);
        Main.spriteBatch.End();

        if (AdditiveTarget == null || AdditiveTarget.IsDisposed)
        {
            return;
        }

        Main.spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
        Main.spriteBatch.Draw(AdditiveTarget, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);
        Main.spriteBatch.End();
    }
}
