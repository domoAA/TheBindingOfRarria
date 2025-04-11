using System;
using Terraria;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Common.Helpers;

public static partial class Helper
{
    public static T Find<T>(this ActiveEntityIterator<T> iterator, Func<T, bool> predicate) where T : Entity
    {
        foreach (var entity in iterator)
        {
            if (predicate(entity))
                return entity;
        }
        return null;
    }

    #region Dust

    public static void SpawnDust(this Vector2 center, Vector2 totalRect, int type, float speed, float scale, Color color, int amount, float distance = 1, float rectRotation = 0)
    {
        for (int i = amount; i > 0; i--)
        {
            var direction = totalRect.RotatedBy(rectRotation) * Vector2.One.RotatedBy(TwoPi / amount * i + Main.rand.NextFloat(-Pi / 10, Pi / 10));
            Dust.NewDustPerfect(center + direction * distance, type, direction * speed, 0, color, scale);
        }
    }
    public static void SpawnDust(this Vector2 center, int type, float speed, float scale, Color color, int amount, float distance = 1, float totalSize = 1, float rotation = -PiOver4 * 3, int layers = 1, float scaleStep = 0f)
    {
        for (int x = layers; x > 0; x--)
        {
            scale += scaleStep;
            for (int i = amount; i > 0; i--)
            {
                var rot = Main.rand.NextFloat(-totalSize / 2, totalSize / 2);
                var direction = Vector2.One.RotatedBy(totalSize + rot + rotation);
                Dust.NewDustPerfect(center + direction * distance, type, direction * speed, 0, color, scale * Main.rand.NextFloat(0.8f, 1.3f));
            }
        }
    }
    public static void SpawnDust(this Vector2 center, Vector2[] directions, int type, float speed, float scale, Color color, int layers = 1, float scaleStep = 0, float random = 0, float distance = 1)
    {
        for (int i = layers; i > 0; i--)
        {
            scale += scaleStep;
            foreach (var direction in directions)
            {
                var rotation = Main.rand.NextFloat(-random / 2, random / 2);
                Dust.NewDustPerfect(
                    center + direction.RotatedBy(rotation) * distance,
                    type,
                    direction.RotatedBy(rotation) * speed,
                    255,
                    color,
                    scale * float.Abs(rotation) + 0.3f
                );
            }
        }
    }
    /// <summary>
    /// Frames dust properly based on the passed vanilla dust type. Automatically picks between the 3 available dust sprites.
    /// </summary>
    /// <example>
    /// dust.frame = FrameVanillaDust(DustID.Dirt)
    /// </example>
    /// <param name="dustType">The vanilla dust type for the desired sprite</param>
    /// <returns>A Rectangle framing the dust sprite</returns>
    public static Rectangle FrameVanillaDust(int dustType)
    {
        int frameX = dustType * 10 % 1000;
        int frameY = dustType * 10 / 1000 * 30 + Main.rand.Next(3) * 10;
        return new Rectangle(frameX, frameY, 8, 8);
    }

    #endregion
}
