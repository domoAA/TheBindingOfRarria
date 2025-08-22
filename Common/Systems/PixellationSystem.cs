using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using TheBindingOfRarria.Common.Helpers;

namespace TheBindingOfRarria.Common.Systems;
public class PixellationSystem : ModSystem
{
    // credits for the base for this system to naka, also thanks to zen, stormytuna and some others I forgor about for helping me change this thing to fit my needs

    public enum RenderType
    {
        Additive,
        AlphaBlend
    }

    private static RenderTarget2D Target { get; set; }

    private static Queue<(Action action, RenderType type)> Actions { get; set; } = new();

    public override void Load()
    {

        if (!Main.dedServ)
        {
            Main.OnResolutionChanged += InitializeRT;
            Main.RunOnMainThread(() =>
            {
                Target = new(Main.instance.GraphicsDevice, Main.screenWidth, Main.screenHeight, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
            });
        }

        On_Main.DrawInfernoRings += DrawPixellated;
    }

    private void InitializeRT(Vector2 obj)
    {
        if (Main.dedServ)
        {
            return;
        }
        Target?.Dispose();

        GraphicsDevice gd = Main.instance.GraphicsDevice;
        int width = Main.screenWidth / 2;
        int height = Main.screenHeight / 2;

        Target = new(gd, width, height, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
    }

    public static void QueuePixellationAction(Action action, RenderType type)
    {
        Actions.Enqueue((action, type));
    }

    /// <summary>
    /// Invokes the passed draw action on the rt and draws the rt with 2x scale
    /// </summary>
    private static void DrawPixellated(On_Main.orig_DrawInfernoRings orig, Main self)
    {
        orig(self);

        if (Actions is null || Actions.Count <= 0)
            return;

        var gd = Main.graphics.GraphicsDevice;

        if (gd.PresentationParameters.RenderTargetUsage != RenderTargetUsage.PreserveContents)
            gd.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;

        var oldTargets = gd.GetRenderTargets();
        foreach (var target in oldTargets)
        {
            if (target.RenderTarget is RenderTarget2D rt)
                rt.RenderTargetUsage = RenderTargetUsage.PreserveContents;
        }

        gd.SetRenderTarget(Target);
        gd.Clear(Color.Transparent);

        Helper.SpritebatchParameters parameters = new();
        var beginned = Main.spriteBatch.beginCalled;
        if (beginned)
            Main.spriteBatch.End(out parameters);


        for (int i = 0; i < Actions.Count; i++)
        {
            var (action, type) = Actions.Dequeue();

            Main.spriteBatch.Begin(SpriteSortMode.Deferred, type == RenderType.Additive ? BlendState.Additive : BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, Main.Rasterizer, null, Matrix.Identity);
 
            action.Invoke();

            Main.spriteBatch.End();
        }

        if (beginned)
            Main.spriteBatch.Begin(parameters);
    


        Main.graphics.GraphicsDevice.SetRenderTargets(oldTargets);

        beginned = Main.spriteBatch.beginCalled;
        if (beginned)
            Main.spriteBatch.End(out parameters);
        
        Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

        Main.spriteBatch.Draw(Target, new Vector2(0), null, Color.White, 0, new Vector2(0), 2, SpriteEffects.None, 0);

        Main.spriteBatch.End();

        if (beginned)
            Main.spriteBatch.Begin(parameters);
    }
}
