using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Dusts;

public class GlorpyHealingPlus : ModDust
{
    public override string Texture => ContentPath + "Dusts/" + Name;

    public override void OnSpawn(Dust dust)
    {
        dust.noGravity = true;
        dust.noLight = true;
    }

    public override bool Update(Dust dust)
    {
        dust.position += dust.velocity;
        dust.velocity *= 0.97f;
        dust.scale -= 0.02f;

        var light = dust.color;

        Lighting.AddLight(dust.position, light.R / 2000f, light.G / 2000f, light.B / 2000f);

        if (dust.scale < 0.5f)
            dust.active = false;

        return false;
    }

    public override bool PreDraw(Dust dust)
    {
        Texture2D texture = Texture2D.Value;

        var rect = texture.Frame(1, 3, 0);

        var color = dust.color;
        color *= color.A / 255f;

        Main.spriteBatch.Draw(texture, dust.position - Main.screenPosition, rect, color with { A = 0 });
        return false;
    }
}