using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using TheBindingOfRarria.Common;

namespace TheBindingOfRarria.Content.Projectiles
{
    public class HeavyBlowThing : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 72;
            Projectile.tileCollide = false;
            Projectile.damage = 0;
            Projectile.ignoreWater = true;
            Projectile.light = 0.3f;
            Projectile.timeLeft = 12;
        }
        public override void AI()
        {
            Projectile.ai[0] += 10;
            Projectile.rotation = Projectile.velocity.ToRotation();
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            var texture = Terraria.GameContent.TextureAssets.Projectile[Type].Value;
            var scale = Projectile.scale / 4;
            var color = Color.White;
            color.A -= (byte)Projectile.ai[0];

            PixellationSystem.QueuePixelationAction(() => {
                Main.EntitySpriteDraw(texture, (Projectile.Center + Projectile.velocity * 5 - Main.screenPosition) / 2, texture.Bounds, color, Projectile.rotation + MathHelper.PiOver2, texture.Size() / 2, scale * 2, SpriteEffects.None, 0);
                Main.EntitySpriteDraw(texture, (Projectile.Center - Main.screenPosition) / 2, texture.Bounds, color, Projectile.rotation, texture.Size() / 2, scale * 3, SpriteEffects.None, 0);
            }, PixellationSystem.RenderType.AlphaBlend);
            return false;
        }
    }
}