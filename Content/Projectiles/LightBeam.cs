using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Projectiles
{
    public class LightBeam : ModProjectile
    {
        public int CD = 0;
        public bool shining = false;
        public override void SetDefaults()
        {
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.timeLeft = 360;
            Projectile.friendly = true;
            Projectile.netImportant = true;
        }
        public override void AI()
        {
            Projectile.ai[2] += shining == true ? 0.2f : shining == false ? -0.2f : 0;

            if (Projectile.ai[2] > 2)
                shining = false;
            else if (Projectile.ai[2] < -2)
                shining = true;

            if (Projectile.timeLeft < 100)
                Projectile.Kill();
            else if (Projectile.timeLeft < 240)
                Projectile.timeLeft--;

            Projectile.scale = 2f;
            Projectile.width = (int)(24 * Projectile.scale);
            Projectile.height = (int)(24 * Projectile.scale);

            var rotation = Projectile.Center.DirectionFrom(Main.player[Projectile.owner].Center).ToRotation();
            Projectile.rotation = rotation + MathHelper.PiOver2;

            var offset = new Vector2(Projectile.ai[0], Projectile.ai[1]);
            Projectile.Center = Main.player[Projectile.owner].Center + offset * 2f;

            CD--;
            if (CD < 0)
                CD = 10;
            Projectile.netUpdate = true;
        }
        public override bool? CanHitNPC(NPC target)
        {
            if (CD > 0 || !Projectile.friendly)
                return false;

            LaserHit(target);

            return true;
        }
        public void LaserHit(NPC target)
        {
            float _ = float.NaN;
            if (Collision.CheckAABBvLineCollision(target.getRect().TopLeft(), target.getRect().Size(), Main.player[Projectile.owner].Center, Projectile.Center, 24 * Projectile.scale, ref _))
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
            float power = 0.0035f * (Projectile.timeLeft);

            byte alpha = (byte)(5 + Projectile.ai[2]);

            PixellationSystem.QueuePixelationAction(() => {
                Projectile.DrawLightBeam(TheBindingOfRarria.BeamEnd.Value, TheBindingOfRarria.BeamBody.Value, Color.LightYellow, alpha, 1, new Vector2(power, 1), new Vector2(0.05f, 0f), 15);
            }, PixellationSystem.RenderType.Additive);
            return false;
        }
    }
}