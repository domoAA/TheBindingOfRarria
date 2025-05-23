using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria;
using System.Linq;
using Terraria.DataStructures;

namespace TheBindingOfRarria.Common.Systems;
public class PlayerDrawScaleSystem : ModSystem
{
    public static HashSet<int> PlayersToDraw
    {
        get;
        private set;
    } = [];

    public static void RegisterPlayerForDraw(int whoAmI)
    {
        PlayersToDraw.Add(whoAmI);

        ReInitializeRT();
    }

    public static bool canUseTarget = false;

    private static void ReInitializeRT()
    {
        if (Main.dedServ)
            return;

        PlayerDrawTarget?.Dispose();

        GraphicsDevice gd = Main.instance.GraphicsDevice;
        int width = Main.screenWidth;
        int height = Main.screenHeight * PlayersToDraw.Count;

        PlayerDrawTarget = new(gd, width, height);
    }
    private void InitializeRT(Vector2 obj)
    {
        if (Main.dedServ)
        {
            return;
        }
        PlayerDrawTarget?.Dispose();

        GraphicsDevice gd = Main.instance.GraphicsDevice;
        int width = Main.screenWidth;
        int height = Main.screenHeight;

        PlayerDrawTarget = new(gd, width, height);
    }

    public static RenderTarget2D PlayerDrawTarget { get; set; }

    public override void Load()
    {

        if (!Main.dedServ)
        {
            Main.OnResolutionChanged += InitializeRT;

            Main.RunOnMainThread(() =>
            {
                PlayerDrawTarget = new(Main.instance.GraphicsDevice, Main.screenWidth, Main.screenHeight);
            });
        }
        On_Main.CheckMonoliths += DrawToRT;
    }

    private void DrawToRT(On_Main.orig_CheckMonoliths orig)
    {
        orig();

        if (Main.gameMenu)
            return;

        if (PlayersToDraw.Count > 0)
            DrawPlayerTarget();

        if (Main.instance.tileTarget.IsDisposed)
            return;
    }

    Vector2 oldPos;
    Vector2 oldCenter;
    Vector2 oldMountedCenter;
    Vector2 oldScreen;
    Vector2 oldItemLocation;

    /// <summary>
    /// Draws every player on the rt
    /// </summary>
    private void DrawPlayerTarget()
    {
        if (PlayerDrawTarget.Height != PlayersToDraw.Count * Main.screenHeight)
            ReInitializeRT();

        RenderTargetBinding[] oldtargets2 = Main.graphics.GraphicsDevice.GetRenderTargets();
        canUseTarget = false;

        Main.graphics.GraphicsDevice.SetRenderTarget(PlayerDrawTarget);
        Main.graphics.GraphicsDevice.Clear(Color.Transparent);

        Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.EffectMatrix);

        foreach (var i in PlayersToDraw)
        {
            Player player = Main.player[i];

            if (player.active && player.dye.Length > 0)
            {
                int oldHeldProj = player.heldProj;
                oldMountedCenter = player.MountedCenter;
                oldItemLocation = player.itemLocation;
                player.itemLocation = oldItemLocation;
                player.MountedCenter = oldMountedCenter - oldPos;

                //temp change Player's actual position to lock into their frame
                player.heldProj = -1;

                Main.PlayerRenderer.DrawPlayer(Main.Camera, player, player.position, player.fullRotation, player.fullRotationOrigin, 0f);

                player.heldProj = oldHeldProj;
                player.itemLocation = oldItemLocation;
                player.MountedCenter = oldMountedCenter;
            }
        }

        Main.spriteBatch.End();

        Main.graphics.GraphicsDevice.SetRenderTargets(oldtargets2);
        canUseTarget = true;
    }
}

public class ScaledDrawPlayer : ModPlayer
{
    public float Scale
    {
        get;
        private set;
    } = 1;

    public override void ResetEffects()
    {
        if (Scale != 1)
            ResetScale();
    }

    private void ResetScale()
    {
        var box = Player.Hitbox;
        //var center = box.Center();

        var width = (int)(box.Width / Scale);
        var height = (int)(box.Height / Scale);

        Player.Hitbox = box with { Width = width, Height = height };

        Scale = 1;
    }

    private void SetScale(float scale)
    {
        var box = Player.Hitbox;

        var width = (int)(box.Width * Scale);
        var height = (int)(box.Height * Scale);

        Player.Hitbox = box with { Width = width, Height = height };
    }

    public void ApplyScaleToPlayer(int whoAmI, float scale)
    {
        if (Scale != 1)
            ResetScale();

        if (whoAmI == Player.whoAmI)
            Scale = scale;

        PlayerSize = Player.Hitbox.Size();

        SetScale(scale);
    }

    public Vector2 PlayerSize = new Vector2(20, 42);

    public override void PostUpdate()
    {
        if (Player.Hitbox.Size() != new Vector2((int)(PlayerSize.X * Scale), (int)(PlayerSize.Y * Scale)))
            ApplyScaleToPlayer(Player.whoAmI, Scale);
    }

    public override void HideDrawLayers(PlayerDrawSet drawInfo)
    {
        if (!PlayerDrawScaleSystem.canUseTarget || Scale == 1)
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
        if (!PlayerDrawScaleSystem.canUseTarget || Scale == 1)
        {
            return;
        }

        var rect = PlayerDrawScaleSystem.PlayerDrawTarget.Frame(1, PlayerDrawScaleSystem.PlayersToDraw.Count, 0, Player.whoAmI);
        Main.spriteBatch.Draw(PlayerDrawScaleSystem.PlayerDrawTarget, rect.Size() / 2, rect, fullBright ? Color.White : new Color(r, g, b, a), 0, rect.Size() / 2, Scale, SpriteEffects.None, 0);
    }
}