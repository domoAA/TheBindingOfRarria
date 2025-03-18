
namespace TheBindingOfRarria.Content.Projectiles
{
    public class LightBeam : ModProjectile
    {
        public int CD = 0;
        public override void SetDefaults()
        {
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.timeLeft = 500;
            Projectile.friendly = true;
            Projectile.netImportant = true;
            Projectile.ai[2] = 0;
            ProjectileID.Sets.DrawScreenCheckFluff[Type] = 1000;
        }
        public override bool ShouldUpdatePosition() => false;
        public override void AI()
        {
            if (Projectile.timeLeft < 100)
                Projectile.Kill();
            else if (Projectile.timeLeft < 240)
                Projectile.timeLeft--;

            Projectile.scale = 2f;
            Projectile.width = (int)(24 * Projectile.scale);
            Projectile.height = (int)(24 * Projectile.scale);


            CD--;
            if (CD < 0)
                CD = 10;
            Projectile.netUpdate = true;
        }
        public override bool? CanHitNPC(NPC target)
        {
            if (CD > 0 || !Projectile.friendly || Projectile.timeLeft > 350)
                return false;

            LaserHit(target);

            return true;
        }
        public void LaserHit(NPC target)
        {
            float _ = float.NaN;
            if (Collision.CheckAABBvLineCollision(target.getRect().TopLeft(), target.getRect().Size(), Projectile.Center, Projectile.Center + Projectile.velocity, 24 * Projectile.scale, ref _))
                SyncedManualStrike(target);
        }
        public static void SyncedManualStrike(NPC target)
        {
            var info = new NPC.HitInfo
            {
                Damage = 30 - target.defense / 2,
                Knockback = 3,
                InstantKill = false,
                HideCombatText = false
            };
            target.StrikeNPC(info);
            NetMessage.SendStrikeNPC(target, info);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            var time = 400 - Projectile.timeLeft;
            float power = 0.0035f * (260 - (float.Pow(time - 150, 2) / 60));
            if (Projectile.timeLeft <= 360)
            {

                byte alpha = 50;

                QueuePixelationAction(() =>
                {
                    Projectile.DrawLightBeam(TheBindingOfRarria.BeamEnd.Value, TheBindingOfRarria.BeamBody.Value, Color.LightYellow, alpha, 1, new Vector2(power, 1), new Vector2(0.04f, 0f), 15);
                }, RenderType.Additive);
            }
                var texture = Terraria.GameContent.TextureAssets.Projectile[ModContent.ProjectileType<Extra98Bomb>()].Value;
                texture.DrawWithTransparency((Projectile.Center - Main.screenPosition - new Vector2(Main.screenWidth / 2, Main.screenHeight / 2)) * Main.GameZoomTarget + new Vector2(Main.screenWidth / 2, Main.screenHeight / 2), texture.Bounds, Color.LightYellow, 30, 3, Projectile.scale * 2 * float.Sin(power), 0.02f, 4);
            
            return false;
        }
    }
}