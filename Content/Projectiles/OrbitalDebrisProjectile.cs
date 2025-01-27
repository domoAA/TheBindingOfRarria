using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria;

namespace TheBindingOfRarria.Content.Projectiles
{
    public class OrbitalDebrisProjectile : GlobalProjectile
    {
        public bool Orbiting = false;
        public bool? FollowingARing = false;
        public Projectile SaturnusRing = null;
        public override bool InstancePerEntity => true;
        public float rotation = Main.rand.NextBool() ? MathHelper.PiOver2 : -MathHelper.PiOver2;
        public float speed = 0.9f;
        public float IndividualOffset = (Main.rand.NextFloat() - 0.5f) * 128;
        public override void PostAI(Projectile projectile)
        {
            base.PostAI(projectile);
            if (!Orbiting)
                return;

            if (FollowingARing == true && !SaturnusRing.active)
            {
                Orbiting = false;
                FollowingARing = null;
                SaturnusRing = null;
            }

            projectile.tileCollide = false;
            var r = projectile.Center.Distance(Main.player[projectile.owner].Center);
            var velocity = projectile.velocity.RotatedBy(rotation);
            velocity.Normalize();
            if (r < 32)
                return;

            projectile.velocity += velocity * projectile.velocity.LengthSquared() / r;
            var gravity = 0.01f * projectile.Center.DirectionTo(Main.player[projectile.owner].Center) * r;
            var offset = (projectile.Center.DirectionTo(Main.player[projectile.owner].Center).ToRotation() + rotation) - projectile.velocity.ToRotation();
            projectile.velocity = projectile.velocity.RotatedBy(offset);
            if (r > 270 + IndividualOffset)
                projectile.velocity += gravity;
            else if (r < 250 + IndividualOffset)
                projectile.velocity -= gravity;
            else
                projectile.velocity *= 1.03f;
            speed += speed < 0.98f ? 0.005f : 0;
            projectile.velocity *= speed;
        }
    }
}