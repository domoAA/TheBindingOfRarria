using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using TheBindingOfRarria.Common.Helpers;

namespace TheBindingOfRarria.Content.Projectiles;

public class LightCone : ModProjectile
{
    public override string Texture => ContentPath + "Projectiles/" + Name;

    public override void SetStaticDefaults()
    {
        Projectile.penetrate = -1;
        Projectile.ignoreWater = true;
        Projectile.tileCollide = false;
        Projectile.width = 113;
        Projectile.height = 120;
        Projectile.damage = 0;
        Projectile.netImportant = true;
    }

        // I'm not touching this with a 90 foot pole.
            // But I'm begging you, STOP USING Main.GameZoomTarget TO MULTIPLY SHIT, MATRICIES AUTO APPLY.
                // Seriously gonna hit my god damn mother fucking limit rn, TS PMO SM !
    public override void AI()
    {
        Projectile.tileCollide = false;
        Projectile.friendly = true;
        var owner = Main.player[Projectile.owner];
        if (!owner.active)
            Projectile.Kill();

        var vel = owner.velocity / owner.velocity.Length();

        var basePos = new Vector2(360, 0);
        if (owner.velocity.LengthSquared() <= 1)
            vel = new Vector2(owner.direction, 0);
        Projectile.Center = owner.Center + basePos.RotatedBy(Projectile.Center.DirectionFrom(owner.Center).ToRotation().AngleTowards(basePos.RotatedBy(vel.ToRotation()).ToRotation(), Pi / 20));

        Lighting.AddLight(Projectile.Center.DirectionTo(owner.Center) * 220 + Projectile.Center, Color.LightYellow.ToVector3() / 5);
        Lighting.AddLight(Projectile.Center.DirectionTo(owner.Center) * 170 + Projectile.Center, Color.LightYellow.ToVector3() / 4);
        Lighting.AddLight(Projectile.Center.DirectionTo(owner.Center) * 120 + Projectile.Center, Color.LightYellow.ToVector3() / 3);
        Lighting.AddLight(Projectile.Center.DirectionTo(owner.Center) * 70 + Projectile.Center, Color.LightYellow.ToVector3() / 2);

        Projectile.rotation = owner.Center.DirectionTo(Projectile.Center).ToRotation() + PiOver2;

        foreach (var target in Main.ActiveNPCs)
        {
            if (target.CanBeChasedBy() && !target.friendly)
            {
                var a = owner.Center - Main.screenPosition;
                var b = Projectile.Center + Projectile.Center.DirectionFrom(a).RotatedBy(PiOver2) * 113 * Main.GameZoomTarget - Main.screenPosition;
                var c = Projectile.Center + Projectile.Center.DirectionFrom(a).RotatedBy(-PiOver2) * 113 * Main.GameZoomTarget - Main.screenPosition;
                if (IsPointInTriangle(target.Center - Main.screenPosition, a, b, c))
                {
                    target?.GetSlowed(TheBindingOfRarria.State.Slow, 30);
                }
            }
        }
        foreach (var target in Main.ActiveProjectiles)
        {
            if (target.hostile)
            {
                var a = (owner.Center - Main.screenPosition - new Vector2(Main.screenWidth / 2, Main.screenHeight / 2)) * Main.GameZoomTarget + new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
                var b = (Projectile.Center - Main.screenPosition + Projectile.Center.DirectionFrom(a).RotatedBy(PiOver2) * 113 - new Vector2(Main.screenWidth / 2, Main.screenHeight / 2)) * Main.GameZoomTarget + new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
                var c = (Projectile.Center - Main.screenPosition + Projectile.Center.DirectionFrom(a).RotatedBy(-PiOver2) * 113 - new Vector2(Main.screenWidth / 2, Main.screenHeight / 2)) * Main.GameZoomTarget + new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
                if (IsPointInTriangle(target.Center - Main.screenPosition, a, b, c))
                {
                    target?.GetSlowed(TheBindingOfRarria.State.Slow, 3000);
                }
            }
        }
        Projectile.netUpdate = true;
    }

    public override bool PreDraw(ref Color lightColor)
    {
        Projectile.scale = 3;

        Texture2D texture = TextureAssets.Projectile[Type].Value;

        Color color = Color.LightYellow * (50 / 255f);

        float scale = Projectile.scale * 0.5f;
        scale -= 0.04f;

        Vector2 ownerCenter = Main.player[Projectile.owner].Center;
        Vector2 offset = Projectile.Center.DirectionTo(ownerCenter) * Projectile.Center.Distance(ownerCenter) * 0.5f;
        Vector2 position = Projectile.Center + offset;

        Main.spriteBatch.Draw(texture, position, null, color with { A = 0 }, Projectile.rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0);

        return false;
    }

        // Move to MathHelper or some shit.
    public static bool IsPointInTriangle(Vector2 point, Vector2 a, Vector2 b, Vector2 c)
    {
            // Compute vectors
        var v0 = c - a;
        var v1 = b - a;
        var v2 = point - a;

            // Compute dot products
        float dot00 = Vector2.Dot(v0, v0);
        float dot01 = Vector2.Dot(v0, v1);
        float dot02 = Vector2.Dot(v0, v2);
        float dot11 = Vector2.Dot(v1, v1);
        float dot12 = Vector2.Dot(v1, v2);

            // Compute barycentric coordinates
        float invDenom = 1 / (dot00 * dot11 - dot01 * dot01);
        float u = (dot11 * dot02 - dot01 * dot12) * invDenom;
        float v = (dot00 * dot12 - dot01 * dot02) * invDenom;

            // Check if point is in triangle
        return (u >= 0) && (v >= 0) && (u + v <= 1);
    }
}