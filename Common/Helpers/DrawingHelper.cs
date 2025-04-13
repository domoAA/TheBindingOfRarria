using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using TheBindingOfRarria.Common.Systems;

namespace TheBindingOfRarria.Common.Helpers;

public static partial class Helper
{
    private static readonly Matrix HalfScale = Matrix.CreateScale(0.5f);

    public static Vector2 ScreenSize => new(Main.screenWidth, Main.screenHeight);

    public static void DrawPixellated(this SpriteBatch spriteBatch, Texture2D texture, Vector2 position, Rectangle? sourceRect, Vector2 scale, float rotation, Vector2 origin, Color color, PixellationSystem.RenderType renderType)
    {
        position = Vector2.Transform(position, HalfScale);
        scale = Vector2.Transform(scale, HalfScale);

        PixellationSystem.QueuePixelationAction(() =>
        {
            spriteBatch.Draw(texture, position, sourceRect, color, rotation, origin, scale, SpriteEffects.None, 0);
        }, renderType);
    }

    public static void DrawPixellated(this SpriteBatch spriteBatch, Texture2D texture, Vector2 position, Rectangle? sourceRect, Vector2 scale, float rotation, Vector2 origin, Color color, SpriteEffects effects, PixellationSystem.RenderType renderType)
    {
        position = Vector2.Transform(position, HalfScale);
        scale = Vector2.Transform(scale, HalfScale);

        PixellationSystem.QueuePixelationAction(() =>
        {
            spriteBatch.Draw(texture, position, sourceRect, color, rotation, origin, scale, effects, 0);
        }, renderType);
    }

    public static void DrawPixellated(this SpriteBatch spriteBatch, Texture2D texture, Vector2 position, Rectangle? sourceRect, float scale, float rotation, Vector2 origin, Color color, PixellationSystem.RenderType renderType) =>
        spriteBatch.DrawPixellated(texture, position, sourceRect, scale * Vector2.One, rotation, origin, color, renderType);

    #region Transparency Slop
    public static void DrawWithTransparency(this Texture2D texture, Vector2 center, float scale, Color color, byte alpha)
    {
        color *= alpha * (1f / 255);

        Main.spriteBatch.Draw(texture, center, texture.Bounds, color with { A = 0 }, 0, texture.Size() * 0.5f, scale * 0.5f, SpriteEffects.None, 0);
    }
    public static void DrawWithTransparency(this Texture2D texture, Vector2 center, float scale, float rotation, Color color, byte alpha)
    {
        color *= alpha * (1f / 255);

        Main.spriteBatch.Draw(texture, center, texture.Bounds, color with { A = 0 }, rotation, texture.Size() * 0.5f, scale * 0.5f, SpriteEffects.None, 0);
    }
    public static void DrawWithTransparency(this Texture2D texture, Vector2 center, float scale, float rotation, Color color, byte alpha, SpriteEffects effect, bool apply)
    {
        effect = apply ? effect : SpriteEffects.None;
        Main.spriteBatch.Draw(texture, center, texture.Bounds, color with { A = 0 }, rotation, texture.Size() * 0.5f, scale * 0.5f, effect, 0);
    }

    #endregion
}
