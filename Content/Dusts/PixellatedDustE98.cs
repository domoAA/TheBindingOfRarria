using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using TheBindingOfRarria.Common.Helpers;
using TheBindingOfRarria.Common.Systems;

namespace TheBindingOfRarria.Content.Dusts;

public class PixellatedDustE98 : ModDust
{
    public override string Texture => ContentPath + "Dusts/" + Name;

    public override void OnSpawn(Dust dust)
    {
        dust.noGravity = true;
        dust.noLight = true;
        dust.alpha = dust.color.A;
        dust.color.A = 0;
    }

    public override bool Update(Dust dust)
    {
        dust.position += dust.velocity;
        dust.rotation = dust.velocity.ToRotation();
        dust.velocity *= 0.9f;
        if (dust.color.A == 0)
            dust.color.A = (byte)dust.alpha;

        dust.alpha = 0;
        
        dust.color.A -= 8;

        float light = 0.002f * dust.color.A;

        Lighting.AddLight(dust.position, light, light, light);

        if (dust.color.A < 100)
            dust.active = false;

        return false;
    }

    public override bool PreDraw(Dust dust)
    {
        Texture2D texture = Texture2D.Value;

        Vector2 scale = dust.scale * new Vector2(0.9f, 0.015f * dust.color.A);

        Main.spriteBatch.DrawPixellated(texture, dust.position - Main.screenPosition, null, scale, dust.rotation + PiOver2, texture.Size() * 0.5f, dust.color, PixellationSystem.RenderType.Additive, PixellationSystem.RenderLayer.OverEverything);
        return false;
    }
}