

using Terraria;

namespace TheBindingOfRarria.Content.Projectiles
{
    public class LifeSucker : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.penetrate = 2;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.timeLeft = 50;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.scale = 0.6f;

            Projectile.ai[1] = Main.rand.Next(3);
        }
        public override void AI()
        {
            if (Projectile.ai[0] != 0 && Main.npc[(int)Projectile.ai[0]].active)
                Projectile.Center = Main.npc[(int)Projectile.ai[0]].Center;
            else
                Projectile.Kill();

            Projectile.velocity = Projectile.Center - Main.player[Projectile.owner].Center;

            Projectile.scale += Projectile.ai[2] == 0 ? 0.05f : -0.08f;

            foreach (var proj in Main.ActiveProjectiles)
            {
                if (proj.type == Type && proj.owner == Projectile.owner && proj.ai[0] == Projectile.ai[0] && proj.identity != Projectile.identity)
                    proj.Kill();
                
            }

            Projectile.netUpdate = true;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override bool? CanHitNPC(NPC target)
        {
            if (target.whoAmI != Projectile.ai[0] || Projectile.scale < 1.2f || target.immortal)
                return false;

            return base.CanHitNPC(target);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Main.player[Projectile.owner].GetModPlayer<LifeSuckerPlayer>().heal += (damageDone / 5);
            Projectile.ai[2] = 1;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            var color = Color.DeepSkyBlue;
            color.A = 210;
            if (Projectile.timeLeft < 38)
            {
                color = Color.Red;
                //color.A = 210;
            }

            QueuePixelationAction(() => {
                var rot = PiOver2;
                for (int i = 0; i < 4; i++)
                {
                    rot += PiOver2;
                    Main.spriteBatch.Draw(Projectile.MyTexture(), (Projectile.Center - Main.screenPosition + new Vector2(10, 0).RotatedBy(rot)) / 2, Projectile.MyTexture().Bounds, color, rot, Projectile.MyTexture().Size() / 2, 0.9f * new Vector2(0.4f * (-float.Pow(Projectile.timeLeft - 20, 2) * 0.001f + 0.4f), Projectile.scale * 0.1f), SpriteEffects.None, 0);
                }
                Main.spriteBatch.Draw(Projectile.MyTexture(), (Projectile.Center - (Projectile.velocity / 2) - Main.screenPosition) / 2, Projectile.MyTexture().Bounds, color, Projectile.velocity.ToRotation() + Pi, Projectile.MyTexture().Size() / 2, new Vector2(Projectile.velocity.Length() / 256, Projectile.scale * 0.1f), SpriteEffects.None, 0);
            }, RenderType.Additive);

            return false;
        }
    }
}