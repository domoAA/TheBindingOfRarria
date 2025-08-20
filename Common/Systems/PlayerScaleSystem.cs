using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics;
using Terraria.Graphics.Renderers;
using Terraria.ModLoader;
using TheBindingOfRarria.Common.Helpers;
using TheBindingOfRarria.Common.Registries;
using static TheBindingOfRarria.Common.Systems.ResizedPlayerUtils;
//using static TheBindingOfRarria.Common.Systems.ResizedPlayerUtils;

namespace TheBindingOfRarria.Common.Systems;

// Thanks davidfdev for allowing me to use his implementation for inspiration, though I did change it some
// the original that Dave, and by extension myself, based our implementations on: https://github.com/NotLe0n/Creativetools/blob/1.4.4/src/Tools/Modify/ModifyPlayer.cs
public static class ResizedPlayerUtils
{
    #region Static Methods
    public static void ResetPlayerSize(this Player player)
    {
        var p = player.GetModPlayer<ResizedPlayer>();
        player.position = player.BottomLeft;

        player.width = (int)p.OldSize.X;
        player.height = (int)p.OldSize.Y;

        player.BottomLeft = player.position;

    }

    public static void ApplyPlayerSize(this Player player, float scale)
    {
        // https://github.com/NotLe0n/Creativetools/blob/1.4.4/src/Tools/Modify/ModifyPlayer.cs
        var p = player.GetModPlayer<ResizedPlayer>();
        player.position = player.BottomLeft;

        p.OldSize.X = player.width;
        p.OldSize.Y = player.height;

        player.width = (int)(player.width * scale);
        player.height = (int)(player.height * scale);

        player.BottomLeft = player.position;

    }

    /// <summary>
    ///     Set the player's custom scale.
    /// </summary>
    public static void SetScale(this Player player, float scale)
    {
        var resizedPlayer = player.GetModPlayer<ResizedPlayer>();
        if (Math.Abs(scale - resizedPlayer.Scale) < float.Epsilon)
        {
            // No change
            return;
        }

        //player.ResetPlayerSize();
        //player.ApplyPlayerSize(scale);

        resizedPlayer.Scale = Math.Max(scale, 0.05f);
    }

    /// <summary>
    ///     Reset the player's custom scale.
    /// </summary>
    public static void ResetScale(this Player player)
    {
        SetScale(player, 1);
    }

    #endregion


    public sealed class ResizedPlayer : ModPlayer
    {
        #region Fields

        public float Scale = 1;

        public Vector2 OldSize = new(Player.defaultWidth, Player.defaultHeight);

        public bool IsScaled => Scale is < 1 or > 1;

        #endregion

        #region Methods

        public override void Initialize()
        {
            Scale = 1;
            OldSize = new(Player.defaultWidth, Player.defaultHeight);
        }

        public override void PreUpdateMovement()
        {
            if (IsScaled)
            {
                Player.ApplyPlayerSize(Scale);
            }
        }

        public override void PreUpdate()
        {
            if (IsScaled)
            {
                ResetPlayerSize(Player);
                Player.ResetScale();
            }
        }

        public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
        {
            if (IsScaled)
            {
                drawInfo.ItemLocation += new Vector2(0, drawInfo.drawPlayer.height * (Scale - 1f));
            }
        }
        #endregion
    }
}

public class PlayerScaleItem : GlobalItem
{
    public override bool InstancePerEntity => true;

    public override void UseItemHitbox(Item item, Player player, ref Rectangle hitbox, ref bool noHitbox)
    {
        var p = player.GetModPlayer<ResizedPlayer>();
        if (p.IsScaled)
        {
            var addX = (int)(hitbox.Width * (p.Scale - 0.75f));
            var addY = (int)(hitbox.Height * (p.Scale - 0.75f));

            hitbox.Width += addX;
            hitbox.Height += addY;

            if (player.direction == -1)
                hitbox.X -= addX / 2 + (int)(player.width - p.OldSize.X) / 2;

            hitbox.Y -= addY / 2;
        }
    }
}

public class PlayerRenderTarget : ModSystem
{
    public static RenderTarget2D Target;

    private static RenderTarget2D ScaleTarget;

    public override void Load()
    {
        if (Main.dedServ)
            return;

        Main.OnResolutionChanged += InitializeRT;
        Main.RunOnMainThread(() =>
        {
            Target = new(Main.instance.GraphicsDevice, Main.screenWidth, Main.screenHeight, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
            ScaleTarget = new(Main.instance.GraphicsDevice, Main.screenWidth, Main.screenHeight, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
        });

        On_LegacyPlayerRenderer.DrawPlayerInternal += On_LegacyPlayerRenderer_DrawPlayerInternal;
    }

    private void On_LegacyPlayerRenderer_DrawPlayerInternal(On_LegacyPlayerRenderer.orig_DrawPlayerInternal orig, LegacyPlayerRenderer self, Camera camera, Player drawPlayer, Vector2 position, float rotation, Vector2 rotationOrigin, float shadow, float alpha, float scale, bool headOnly)
    {
        if (Main.graphics.graphicsDevice.PresentationParameters.RenderTargetUsage != RenderTargetUsage.PreserveContents)
            Main.graphics.graphicsDevice.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;

        var oldTargets = Main.graphics.graphicsDevice.GetRenderTargets();
        foreach (var target in oldTargets)
        {
            if (target.RenderTarget is RenderTarget2D rt)
                rt.RenderTargetUsage = RenderTargetUsage.PreserveContents;
        }


        if (!headOnly && !Main.gameMenu && drawPlayer.TryGetModPlayer<ResizedPlayer>(out var p) == true && p.IsScaled)
        {
            var Scale = drawPlayer.GetModPlayer<ResizedPlayer>().Scale;

            var difference = new Vector2(drawPlayer.width - p.OldSize.X, drawPlayer.height - p.OldSize.Y);

            if (shadow != 0 && shadow is 0.5f or 0.7f or 0.9f && (Math.Abs(position.Y - drawPlayer.position.Y) > 6f || Math.Abs(position.X - drawPlayer.position.X) > 6f))
            {
                position.Y -= difference.Y;
                var data = self._drawData;
            }

            position += difference * (Main.GameZoomTarget - 1f) / 4;

            DrawPlayerTarget(drawPlayer.whoAmI, () => orig(self, camera, drawPlayer, position, rotation, rotationOrigin, shadow, alpha, scale, headOnly));
        }

        else orig(self, camera, drawPlayer, position, rotation, rotationOrigin, shadow, alpha, scale, headOnly);
    }

    private static void InitializeRT(Vector2 obj)
    {
        if (Main.dedServ)
        {
            return;
        }
        Target?.Dispose();
        ScaleTarget?.Dispose();

        GraphicsDevice gd = Main.instance.GraphicsDevice;
        int width = Main.screenWidth;
        int height = Main.screenHeight;

        ScaleTarget = new(gd, width, height, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
        Target = new(gd, width, height, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
    }

    private void DrawPlayerTarget(int who, Action action)
    {
        RenderTargetBinding[] oldtargets2 = Main.graphics.GraphicsDevice.GetRenderTargets();


        var player = Main.player[who];

        Main.graphics.GraphicsDevice.SetRenderTarget(ScaleTarget);
        Main.graphics.GraphicsDevice.Clear(Color.Transparent);


        var scale = player.GetModPlayer<ResizedPlayer>().Scale;
        player.ResetPlayerSize();

        action.Invoke();


        Main.graphics.GraphicsDevice.SetRenderTarget(Target);
        Main.graphics.GraphicsDevice.Clear(Color.Transparent);

        var beginned = Main.spriteBatch.beginCalled;
        Helper.SpritebatchParameters parameters = new();
        if (beginned)
            Main.spriteBatch.End(out parameters);

        Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Matrix.Identity);

        player.ApplyPlayerSize(scale);
        var center = player.position - Main.screenPosition;
        player.ResetPlayerSize();

        Main.spriteBatch.Draw(ScaleTarget, center - new Vector2(0, player.gfxOffY * scale), null, Color.White, 0, center, scale, SpriteEffects.None, 0);

        Main.spriteBatch.End();
        if (beginned)
            Main.spriteBatch.Begin(parameters);

        player.ApplyPlayerSize(scale);


        Main.graphics.GraphicsDevice.SetRenderTargets(oldtargets2);


        beginned = Main.spriteBatch.beginCalled;
        if (beginned)
            Main.spriteBatch.End(out parameters);
            
        Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, Main.Rasterizer);

        Main.spriteBatch.Draw(Target, new Vector2(0, 0), Color.White);

        Main.spriteBatch.End();

        if (beginned)
            Main.spriteBatch.Begin(parameters);
    }
}