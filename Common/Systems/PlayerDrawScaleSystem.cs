using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using ReLogic.Peripherals.RGB;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.Graphics;
using Terraria.Graphics.Renderers;
using Terraria.ModLoader;
using Terraria.Utilities;
using TheBindingOfRarria.Common.Helpers;
using static TheBindingOfRarria.Common.Systems.PixellationSystem;
//using static TheBindingOfRarria.Common.Systems.ResizedPlayerUtils;

namespace TheBindingOfRarria.Common.Systems;

// Thanks davidfdev for allowing me to use his implementation for inspiration, though I did change it some
// the original that Dave, and by extension myself, based our implementations on: https://github.com/NotLe0n/Creativetools/blob/1.4.4/src/Tools/Modify/ModifyPlayer.cs
/*public static class ResizedPlayerUtils
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

        public override void HideDrawLayers(PlayerDrawSet drawInfo)
        {
            if (!PlayerRenderTarget.canUseTarget || !IsScaled)
            {
                return;
            }

            foreach (PlayerDrawLayer layer in PlayerDrawLayerLoader.Layers)
            {
                layer.Hide();
            }
        }

        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            if (!PlayerRenderTarget.canUseTarget || !IsScaled)
            {
                return;
            }

            drawInfo.drawPlayer.ResetPlayerSize();

            Main.spriteBatch.Draw(PlayerRenderTarget.Target, PlayerRenderTarget.Target.Size() / 2, null, Color.White, 0, PlayerRenderTarget.Target.Size() / 2, 1, SpriteEffects.None, 0);

            drawInfo.drawPlayer.ApplyPlayerSize(Scale);
        }
        #endregion
    }
}*/
/*
	Based on https://github.com/ProjectStarlight/StarlightRiver/blob/fb35df83489a4d840271e946ba38448037fe7cc6/Content/CustomHooks/Visuals.PlayerTarget.cs
    Mostly rewritten though
 */

/*public class PlayerRenderTarget : ModSystem
{
    public static RenderTarget2D Target;

    private static RenderTarget2D ScaleTarget;

    private static Dictionary<(int who, float shadow), Action> DrawList = new();

    public static bool canUseTarget = false;

    public override void Load()
    {
        if (Main.dedServ)
            return;

        Main.OnResolutionChanged += InitializeRT;
        Main.RunOnMainThread(() =>
        {
            Target = new(Main.instance.GraphicsDevice, Main.screenWidth, Main.screenHeight);
            ScaleTarget = new(Main.instance.GraphicsDevice, Main.screenWidth, Main.screenHeight);
        });

        On_Main.CheckMonoliths += DrawTargets;
        On_LegacyPlayerRenderer.DrawPlayer += On_LegacyPlayerRenderer_DrawPlayer;
    }

    private void On_LegacyPlayerRenderer_DrawPlayer(On_LegacyPlayerRenderer.orig_DrawPlayer orig, LegacyPlayerRenderer self, Camera camera, Player drawPlayer, Vector2 position, float rotation, Vector2 rotationOrigin, float shadow, float scale)
    {
        if (drawPlayer.TryGetModPlayer(out ResizedPlayer resizedPlayer) && resizedPlayer.IsScaled && !DrawList.Any(e => e.Key == (drawPlayer.whoAmI, shadow)))
        {
            DrawList.Add((drawPlayer.whoAmI, shadow), () => orig(self, camera, drawPlayer, position, rotation, rotationOrigin, shadow, scale));
        }

        else orig(self, camera, drawPlayer, position, rotation, rotationOrigin, shadow, scale);
    }

    /*private void OnDrawPlayer(On_LegacyPlayerRenderer.orig_DrawPlayerInternal orig, LegacyPlayerRenderer self, Camera camera, Player drawPlayer, Vector2 position, float rotation, Vector2 rotationOrigin, float shadow, float alpha, float scale, bool headOnly)
    {
        if (drawPlayer.TryGetModPlayer(out ResizedPlayer resizedPlayer) && resizedPlayer.IsScaled && !DrawList.Any(e => e.Key.shadow == shadow && e.Key.who == drawPlayer.whoAmI))
        {
            DrawList.Add((drawPlayer.whoAmI, shadow, position), () => orig(self, camera, drawPlayer, position, rotation, rotationOrigin, shadow, alpha, scale, headOnly));
        }

        else orig(self, camera, drawPlayer, position, rotation, rotationOrigin, shadow, alpha, scale, headOnly);

    }*/

    /*private static void InitializeRT(Vector2 obj)
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

        Target = new(gd, width, height);
        ScaleTarget = new(gd, width, height);
    }

    private void DrawTargets(On_Main.orig_CheckMonoliths orig)
    {
        orig();

        if (Main.gameMenu || DrawList.Count == 0)
            return;

        if (Main.player.Any(n => n.active))
            DrawPlayerTarget();

        if (Main.instance.tileTarget.IsDisposed)
            return;
    }

    private void DrawPlayerTarget()
    {
        RenderTargetBinding[] oldtargets2 = Main.graphics.GraphicsDevice.GetRenderTargets();
        canUseTarget = false;


        for (int i = 0; i < Main.maxPlayers; i++)
        {
            Player player = Main.player[i];
            if (!player.active || player.dye.Length <= 0 || !DrawList.Any(m => m.Key.who == i))
                continue;

            Main.graphics.GraphicsDevice.SetRenderTarget(ScaleTarget);
            Main.graphics.GraphicsDevice.Clear(Color.Transparent);


            var scale = player.GetModPlayer<ResizedPlayer>().Scale;

            while (DrawList.Any(m => m.Key.who == i))
            {
                player.ResetPlayerSize();

                //Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.EffectMatrix);

                var draw = DrawList.First(m => m.Key.who == i);

                draw.Value.Invoke();

                DrawList.Remove(draw.Key);
                //Main.PlayerRenderer.DrawPlayer(Main.Camera, player, player.position, player.fullRotation, player.fullRotationOrigin);


                //Main.spriteBatch.End();


                Main.graphics.GraphicsDevice.SetRenderTarget(Target);
                Main.graphics.GraphicsDevice.Clear(Color.Transparent);

                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Matrix.Identity);

                player.ApplyPlayerSize(scale);
                var center = player.position - Main.screenPosition;
                player.ResetPlayerSize();


                Main.spriteBatch.Draw(ScaleTarget, center - new Vector2(0, player.gfxOffY * scale), null, Color.White, 0, center, scale, SpriteEffects.None, 0);

                Main.spriteBatch.End();

                player.ApplyPlayerSize(scale);
            }
        }


        Main.graphics.GraphicsDevice.SetRenderTargets(oldtargets2);
        canUseTarget = true;


    }
}*/