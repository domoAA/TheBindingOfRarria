using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria;
using TheBindingOfRarria.Common;

namespace TheBindingOfRarria.Content.Dusts
{
    public class PixellatedDustE98 : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.noLight = true;
        }
        public override bool Update(Dust dust)
        {
            dust.position += dust.velocity;
            dust.rotation = dust.velocity.ToRotation();
            dust.velocity *= 0.9f;
            dust.color.A -= 8;
            float light = 0.002f * dust.color.A;

            Lighting.AddLight(dust.position, light, light, light);

            if (dust.color.A < 100)
            {
                dust.active = false;
            }

            return false;
        }
        public override bool PreDraw(Dust dust)
        {
            Texture2D.Value.DrawPixellated((dust.position - Main.screenPosition) / 2, dust.scale * new Vector2(0.9f, 0.015f * dust.color.A), dust.rotation + MathHelper.PiOver2, dust.color, PixellationSystem.RenderType.Additive);
            return false;
        }
    }
}