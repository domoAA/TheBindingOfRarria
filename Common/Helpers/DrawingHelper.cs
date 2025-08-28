using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Common.Systems;

namespace TheBindingOfRarria.Common.Helpers;

public static partial class Helper
{
    private static readonly Matrix HalfScale = Matrix.CreateScale(0.5f);

    public static Vector2 ScreenSize => new(Main.screenWidth, Main.screenHeight);

    public static string GetVanillaExtraTexture(int extraID) => $"Terraria/Images/Extra_{extraID}";
    public static string GetVanillaItemTexture(int itemID) => $"Terraria/Images/Item_{itemID}";
    public static string GetVanillaProjectileTexture(int projectileID) => $"Terraria/Images/Projectile_{projectileID}";

    /// <summary>
    /// Allows you to retrieve the texture that should be paired with some modded type
    /// <br>Assumes the texture has the same name as the class and is in the same folder</br>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="extraText"></param>
    /// <returns></returns>
    public static string GetTextureFromOther<T>(string extraText = "") where T : ModType
    {
        if (typeof(T) is null)
            return GetVanillaItemTexture(ItemID.Torch); //torch.

        T instance = ModContent.GetInstance<T>();
        return instance.GetType().Namespace.Replace(".", "/") + "/" + instance.Name + extraText;
    }

    public static void DrawPixellated(this SpriteBatch spriteBatch, Texture2D texture, Vector2 position, Rectangle? sourceRect, Vector2 scale, float rotation, Vector2 origin, Color color, PixellationSystem.RenderType renderType, PixellationSystem.RenderLayer layer)
    {
        position = Vector2.Transform(position, HalfScale);
        scale = Vector2.Transform(scale, HalfScale);

        PixellationSystem.QueuePixellationAction(() =>
        {
            spriteBatch.Draw(texture, position, sourceRect, color, rotation, origin, scale, SpriteEffects.None, 0);
        }, renderType, layer);
    }

    public static void DrawPixellated(this SpriteBatch spriteBatch, Texture2D texture, Vector2 position, Rectangle? sourceRect, Vector2 scale, float rotation, Vector2 origin, Color color, SpriteEffects effects, PixellationSystem.RenderType renderType, PixellationSystem.RenderLayer layer)
    {
        position = Vector2.Transform(position, HalfScale);
        scale = Vector2.Transform(scale, HalfScale);

        PixellationSystem.QueuePixellationAction(() =>
        {
            spriteBatch.Draw(texture, position, sourceRect, color, rotation, origin, scale, effects, 0);
        }, renderType, layer);
    }

    public static void DrawPixellated(this SpriteBatch spriteBatch, Texture2D texture, Vector2 position, Rectangle? sourceRect, float scale, float rotation, Vector2 origin, Color color, PixellationSystem.RenderType renderType, PixellationSystem.RenderLayer layer) =>
        spriteBatch.DrawPixellated(texture, position, sourceRect, scale * Vector2.One, rotation, origin, color, renderType, layer);

    #region Transparency Slop
    public static void DrawWithTransparency(this Texture2D texture, Vector2 center, float scale, Color color, byte alpha)
    {
        color *= alpha / 255f;

        Main.spriteBatch.Draw(texture, center, texture.Bounds, color with { A = 0 }, 0, texture.Size() * 0.5f, scale * 0.5f, SpriteEffects.None, 0);
    }
    public static void DrawWithTransparency(this Texture2D texture, Vector2 center, Rectangle frame, float scale, Color color, byte alpha)
    {
        color *= alpha / 255f;

        Main.spriteBatch.Draw(texture, center, frame, color with { A = 0 }, 0, texture.Size() * 0.5f, scale * 0.5f, SpriteEffects.None, 0);
    }
    public static void DrawWithTransparency(this Texture2D texture, Vector2 center, float scale, float rotation, Color color, byte alpha)
    {
        color *= alpha / 255f;

        Main.spriteBatch.Draw(texture, center, texture.Bounds, color with { A = 0 }, rotation, texture.Size() * 0.5f, scale * 0.5f, SpriteEffects.None, 0);
    }
    public static void DrawWithTransparency(this Texture2D texture, Vector2 center, float scale, float rotation, Color color, byte alpha, SpriteEffects effect, bool apply)
    {
        color *= alpha / 255f;

        effect = apply ? effect : SpriteEffects.None;
        Main.spriteBatch.Draw(texture, center, texture.Bounds, color with { A = 0 }, rotation, texture.Size() * 0.5f, scale * 0.5f, effect, 0);
    }

    #endregion
}
