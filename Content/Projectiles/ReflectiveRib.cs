using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Projectiles
{
    public class ReflectiveRib : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Default;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
        }
        public Vector2[] stripe =
        {
            new( 0.5f, 0.5f),
            new( 0.5f, 0.5f),
            new( 0.5f, 0.5f),
            new( 0.5f, 0.5f),
            new( 0.5f, 0.5f),
            new( 0.5f, 0.5f)
        };
        public override void AI()
        {
            Projectile.ai[0]++;

            var rotation = (MathHelper.TwoPi / 360 * Projectile.ai[0]);
            Projectile.OrbitingPlayer(1.6f, 400, rotation);
            Projectile.ReflectProjectiles(true, 1f);

            for (int i = 0; i < 6; i++)
            {
                    stripe[0] = Projectile.Center;
                stripe[i] = Projectile.Center + Projectile.Center.DirectionTo(Main.player[Projectile.owner].Center) * Projectile.Center.Distance(Main.player[Projectile.owner].Center) * i / 6 + Projectile.Center.DirectionTo(Main.player[Projectile.owner].Center).RotatedBy(Main.rand.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4)) * Projectile.Center.Distance(Main.player[Projectile.owner].Center) / 6;
            }

            //var pull = Projectile.Center.DirectionTo(owner.Center);//(Projectile.velocity.LengthSquared() * 40);
            //Projectile.velocity = pull.RotatedBy(MathHelper.PiOver2) * 3;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            var vertices = new VertexPositionColorTexture[6]
            { // Depth can be zero in all. The order of the vertices is clockwise, if its counter clockwise it might not draw, depends on the RasterizerState.
                new VertexPositionColorTexture(new Vector3(stripe[0], 0f), Color.Green, Vector2.Zero), 
                new VertexPositionColorTexture(new Vector3(stripe[1], 0f), Color.Blue, Vector2.Zero),
                new VertexPositionColorTexture(new Vector3(stripe[2], 0f), Color.Green, Vector2.Zero),
                new VertexPositionColorTexture(new Vector3(stripe[3], 0f), Color.Blue, Vector2.Zero), 
                new VertexPositionColorTexture(new Vector3(stripe[4], 0f), Color.Green, Vector2.Zero),
                new VertexPositionColorTexture(new Vector3(stripe[5], 0f), Color.Blue, Vector2.Zero),
            };
            vertices.DrawPrimStripe();
            return base.PreDraw(ref lightColor);
        }
    }
}