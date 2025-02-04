using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Projectiles
{
    public class Extra98Bomb : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 20;
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.timeLeft = 132;
        }
        private NPC Target
        {
            get => Projectile.ai[0] == 0 ? null : Main.npc[(int)Projectile.ai[0]];
            set
            {
                Projectile.ai[0] = value == null ? 0 : value.whoAmI;
            }
        }
        private Vector2 Offset = new(0, 0);
        public Vector2[] bomba =
        {
            Vector2.One.RotatedBy(MathHelper.PiOver4 + Main.rand.NextFloat(-MathHelper.Pi / 10, MathHelper.Pi / 10)),
            Vector2.One.RotatedBy(MathHelper.PiOver4 * 2 + Main.rand.NextFloat(-MathHelper.Pi / 10, MathHelper.Pi / 10)),
            Vector2.One.RotatedBy(MathHelper.PiOver4 * 3 + Main.rand.NextFloat(-MathHelper.Pi / 10, MathHelper.Pi / 10)),
            Vector2.One.RotatedBy(MathHelper.PiOver4 * 4 + Main.rand.NextFloat(-MathHelper.Pi / 10, MathHelper.Pi / 10)),
            Vector2.One.RotatedBy(MathHelper.PiOver4 * 5 + Main.rand.NextFloat(-MathHelper.Pi / 10, MathHelper.Pi / 10)),
            Vector2.One.RotatedBy(MathHelper.PiOver4 * 6 + Main.rand.NextFloat(-MathHelper.Pi / 10, MathHelper.Pi / 10)),
            Vector2.One.RotatedBy(MathHelper.PiOver4 * 7 + Main.rand.NextFloat(-MathHelper.Pi / 10, MathHelper.Pi / 10)),
            Vector2.One.RotatedBy(MathHelper.PiOver4 * 8 + Main.rand.NextFloat(-MathHelper.Pi / 10, MathHelper.Pi / 10))
        };
        public override void AI()
        {
            if (Target  == null || !Target.active)
                Projectile.Kill();
            else if (Projectile.timeLeft > 12)
            {
                Offset = new Vector2(Projectile.ai[1], Projectile.ai[2]);
                Projectile.Center = Target.Center - Offset;
                Projectile.netUpdate = true;
            }
        }
        public override bool? CanHitNPC(NPC target)
        {
            if (Projectile.timeLeft > 4)
                return false;

            return base.CanHitNPC(target);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.scale = 0.5f;
            if (Projectile.timeLeft > 12)
                Projectile.DrawPixellated();
            else
            {
                var extra98 = Terraria.GameContent.TextureAssets.Extra[98].Value;
                var color = lightColor;
                PixellationSystem.QueuePixelationAction(() => {
                    for (int i = bomba.Length - 1; i > 0; i--)
                    {
                        Main.EntitySpriteDraw(extra98, (Projectile.Center + bomba[i] * (12 - Projectile.timeLeft) - Main.screenPosition) / 2, extra98.Bounds, color, bomba[i].ToRotation() + MathHelper.PiOver2, extra98.Size() / 2, 0.15f, SpriteEffects.None);
                    }
                }, PixellationSystem.RenderType.AlphaBlend);
            }

            return false;
        }
    }
}