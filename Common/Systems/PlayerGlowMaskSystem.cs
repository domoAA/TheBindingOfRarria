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

namespace TheBindingOfRarria.Common.Systems;

public class PlayerGlowMask : ModSystem
{
    public override void Load()
    {
        On_LegacyPlayerRenderer.DrawPlayerInternal += On_LegacyPlayerRenderer_DrawPlayerInternal;
    }

    public static float[] Alpha = new float[255];

    private void On_LegacyPlayerRenderer_DrawPlayerInternal(On_LegacyPlayerRenderer.orig_DrawPlayerInternal orig, LegacyPlayerRenderer self, Camera camera, Player drawPlayer, Vector2 position, float rotation, Vector2 rotationOrigin, float shadow, float alpha, float scale, bool headOnly)
    {
        var a = alpha;
        alpha = Alpha[drawPlayer.whoAmI];
        Alpha[drawPlayer.whoAmI] = 0;

        orig(self, camera, drawPlayer, position, rotation, rotationOrigin, shadow, a, scale, headOnly);

        Alpha[drawPlayer.whoAmI] = alpha;
        if (!headOnly && !Main.gameMenu && shadow == 0 && Alpha[drawPlayer.whoAmI] != 0)
        {
            orig(self, camera, drawPlayer, position, rotation, rotationOrigin, shadow, alpha, scale, headOnly);
        }
    }
}

public class GlowMaskPlayer : ModPlayer
{
    public Color GlowColor = Color.White;

    public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
    {
        ref var alpha = ref PlayerGlowMask.Alpha[drawInfo.drawPlayer.whoAmI];
        if (alpha != 0)
        {
            r *= GlowColor.ToVector3().X; g *= GlowColor.ToVector3().Y; b *= GlowColor.ToVector3().Z; a *= alpha;
            alpha = 0;
        }
    }
}