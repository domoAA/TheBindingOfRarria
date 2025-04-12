using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using TheBindingOfRarria.Common.Systems;

namespace TheBindingOfRarria.Common.Helpers;

public static partial class Helper
{
    private static readonly Matrix HalfScale = Matrix.CreateScale(0.5f);

    public static Vector2 ScreenSize => new(Main.screenWidth, Main.screenHeight);

        // TODO: kain use a proper matrix in your pixellation system please.
    public static void DrawPixellated(this SpriteBatch spriteBatch, Texture2D texture, Vector2 position, Rectangle? sourceRect, Vector2 scale, float rotation, Vector2 origin, Color color, PixellationSystem.RenderType renderType)
    {
        position = Vector2.Transform(position, HalfScale);
        scale = Vector2.Transform(scale, HalfScale);

        PixellationSystem.QueuePixelationAction(() =>
        {
            spriteBatch.Draw(texture, position, sourceRect, color, rotation, origin, scale, SpriteEffects.None, 0);
        }, renderType);
    }

    #region Transparency Slop

        // Kain im not fucking with this but PLEASE don't do this.
    public static void DrawWithTransparency(this Texture2D texture, Vector2 center, Rectangle? rect, float rotation, Color color, byte alpha, byte alphaStep, float scale, float scaleStep, int layers)
    {
        color.A += alpha;
        scale *= Main.GameZoomTarget;
        for (int i = 0; i < layers; i++)
        {
            color.A += alphaStep;
            scale -= scaleStep;

            Main.spriteBatch.Draw(texture, center, rect, color with { A = 0 }, rotation, texture.Size() * 0.5f, scale * 0.5f, SpriteEffects.None, 0);
        }
    }
    public static void DrawWithTransparency(this Texture2D texture, Vector2 center, float scale, Color color, byte alpha)
    {
        color.A += alpha;
        scale *= Main.GameZoomTarget;
        Main.spriteBatch.Draw(texture, center, texture.Bounds, color with { A = 0 }, 0, texture.Size() * 0.5f, scale * 0.5f, SpriteEffects.None, 0);
    }
    public static void DrawWithTransparency(this Texture2D texture, Vector2 center, float scale, float rotation, Color color, byte alpha)
    {
        color.A += alpha;
        scale *= Main.GameZoomTarget;
        Main.spriteBatch.Draw(texture, center, texture.Bounds, color with { A = 0 }, rotation, texture.Size() * 0.5f, scale * 0.5f, SpriteEffects.None, 0);
    }
    public static void DrawWithTransparency(this Texture2D texture, Vector2 center, float scale, float rotation, Color color, byte alpha, SpriteEffects effect, bool apply)
    {
        color.A += alpha;
        scale *= Main.GameZoomTarget;
        effect = apply ? effect : SpriteEffects.None;
        Main.spriteBatch.Draw(texture, center, texture.Bounds, color with { A = 0 }, rotation, texture.Size() * 0.5f, scale * 0.5f, effect, 0);
    }

    #endregion
}
