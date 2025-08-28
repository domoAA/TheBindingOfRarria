using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Drawing;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using TheBindingOfRarria.Common;
using TheBindingOfRarria.Common.Helpers;
using TheBindingOfRarria.Common.Registries;
using TheBindingOfRarria.Common.Systems;
using TheBindingOfRarria.Content.Buffs;

namespace TheBindingOfRarria.Content.Projectiles;

public class GoldenHalberdProj : ModProjectile
{
    public override string Texture => ContentPath + "Items/GoldenHalberd";

    /// <summary>
    /// This is what will contain the points for trail drawing. 
    /// <br>Add a point to this every tick, and remove the oldest once you hit a desired amount of points, also every tick.</br>
    /// </summary>
    public Queue<Vector2> Trail { get; private set; } = [];

    /// <summary>
    /// This will control whether or not your trail should be drawn.
    /// </summary>
    public bool ShouldDrawTrail { get; private set; } = true;

    /// <summary>
    /// This controls the length of the collision line used for hit checks.
    /// </summary>
    public float BladeLength { get; private set; } = 130;

    /// <summary>
    /// Creates a unit vector from the projectile's rotation, and lengthens it to <see cref="BladeLength"/>.
    /// </summary>
    public Vector2 CollisionLine => Projectile.rotation.ToRotationVector2() * BladeLength;

    public override void SetDefaults()
    {
        //for this example, width/height wont matter too much
        Projectile.width = Projectile.height = 1;

        //custom swords should not break on hit or go through tiles
        Projectile.penetrate = -1;
        Projectile.ownerHitCheck = true;

        //ensure the projectile only hits once
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = -1;

        Projectile.timeLeft = 50;
        Projectile.scale = 1f;
        Projectile.friendly = true;
        Projectile.DamageType = DamageClass.MeleeNoSpeed;
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        Main.player[Projectile.owner].AddBuff(ModContent.BuffType<GoldenVow>(), 300);
    }

    public override void AI()
    {
        Projectile.CenteredOnPlayer();
        Projectile.spriteDirection = Projectile.direction = Main.player[Projectile.owner].direction;

        if (Projectile.timeLeft == 50)
            Projectile.rotation = Projectile.spriteDirection * -PiOver2;



        Projectile.rotation += 0.1f * Projectile.spriteDirection / (0.3f + float.Pow(Projectile.rotation, 2) / 1.5f);

        if (Projectile.timeLeft < 15)
        {
            if (Trail.Count > 0)
                Trail.Dequeue();

            Projectile.alpha += 13;
        }

        if (ShouldDrawTrail)
        {
            Vector2 trailPoint = CollisionLine;
            float rot = Projectile.spriteDirection == 1 ? -PiOver4 : -PiOver2 - PiOver4; //this is handled assuming your texture is diagonal with the tip facing up to the right.
                                                                                         
            if (Trail.Count < 15)
                Trail.Enqueue(trailPoint.RotatedBy(rot));

            else Trail.Dequeue();
        }

        else Trail.Clear(); //if you wish, you could use Dequeue (for a gradual fadeout) but this approach removes the trail entirely at once.
    }

    public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
    {
        float point = 0f;

        Vector2 length = -new Vector2(94).RotatedBy(Projectile.rotation + (Projectile.spriteDirection + 1) / 2 * PiOver2); //replace with desired length
        //length.X *= Projectile.spriteDirection;

        if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center + length * 0.05f, Projectile.Center + length, 20, ref point))
            return true;

        return false;
    }

    public void DrawTrail(Color colorStart, Color colorEnd, float trailZStart = 0.5f, float trailZEnd = 1f)
    {
        PixellationSystem.QueuePixellationAction(() =>
        {
            List<VertexInfo> vertices = []; //this is what we will be passing the trail points to, to draw the strip trail.
            List<Vector2> _trail = [.. Trail]; //despite a Queue being handy for adding/removing points, you cannot access indices the way you would with other things like List<T> and T[].

            for (int i = 0; i < _trail.Count; i++)
            {
                //define a completion ratio from start to end of the provided positions.
                float t = i / (float)_trail.Count; //cast this to a float here to avoid stupid truncation

                //using this ratio, we can create a gradient for the opacity of the trail to follow.
                //this starts at full opacity and fades out as we reach the end of the trail.
                float alpha = Lerp(0f, 1f, t); //remove Projectile.Opacity if you dont want this to fade with your projectile if it has such behaviour.

                //now, we need to actually add VertexInfo values to vertices.
                //we add 2 per iteration, an upper and a lower one.
                //the Z coordinate of the TexCoord is used by the shader for opacity, so pass it here.
                vertices.Add(new VertexInfo((Projectile.Center + _trail[i] * trailZStart), Color.White, new Vector3(t, 1f, alpha)));
                vertices.Add(new VertexInfo((Projectile.Center + _trail[i] * trailZEnd), Color.White, new Vector3(t, 0f, alpha)));
            }

            //here we retrieve our shader, simply setting it in MyMod.Load will do
            Effect effect = Effects.Trail?.Value;
            if (effect is null)
                return;

            //when drawing with shaders, you always need to restart the spritebatch to use immediate sorting.
            //when using immediate, draw data is immediately (hence the name) uploaded to the active gpu, which is important for things like this.
            Main.spriteBatch.End(out var parameters);
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone);

            //these ensure the vertex shader is applied to a proper position on the screen, and is accounted for by zoom and such.
            Matrix transform = Matrix.Identity;
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
            Matrix model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * transform;


            //the shader samles the first texture here, so assign it to the trail shape texture you want to use.
            //this is a Texture2D so have a static Asset<Texture2D> in your class, then set it to the trail texture on load and access .Value here
            Main.graphics.GraphicsDevice.Textures[0] = Textures.TrailTexture.Value;

            //set up the shader parameters. 
            effect.Parameters["uTransform"].SetValue(model * projection);
            effect.Parameters["uColor"].SetValue(colorStart.ToVector4());
            effect.Parameters["uEndColor"].SetValue(colorEnd.ToVector4());
            effect.Parameters["uLerpPower"].SetValue(0.25f);
            effect.CurrentTechnique.Passes["TrailColor"].Apply(); //you could also use [0] here too, or whatever pass your shader is gonna use (remember, it starts at 0 for the 1st pass, and then goes up from there)

            //actually draw the vertices, which will have the shader applied to them.
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices.ToArray(), 0, vertices.Count - 2);

            //make sure to restore the spritebatch after drawing is done. If you wish to layer multiple trails on top of eachother, then uncomment the line below.
            //vertices.Clear();

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(parameters);
        }, PixellationSystem.RenderType.Additive, PixellationSystem.RenderLayer.Projectiles);
    }

    public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
    {
        overPlayers.Add(index);
    }

    public override bool PreDraw(ref Color lightColor)
    {
        //draw the trail under the projectile.
        for (int i = 0; i < 2; i++)
        {
            DrawTrail(Color.DarkGoldenrod.MultiplyRGB(Color.DarkGoldenrod) with { A = 50 }, Color.DarkGoldenrod with { A = 50 }, 0.75f, 0.81f);
            DrawTrail(Color.DarkGoldenrod.MultiplyRGB(Color.DarkGoldenrod) with { A = 70 }, Color.DarkGoldenrod with { A = 70 }, 0.5f, 0.79f);
            DrawTrail(Color.DarkGoldenrod.MultiplyRGB(Color.DarkGoldenrod) with { A = 80 }, Color.DarkGoldenrod with { A = 80 }, 0.3f, 0.70f);
        }

        var texture = TextureAssets.Projectile[Type].Value;

        SpriteEffects spriteEffects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

        int frameHeight = texture.Height / Main.projFrames[Projectile.type];
        int startY = frameHeight * Projectile.frame;

        Rectangle sourceRectangle = new(0, startY, texture.Width, frameHeight);

        Vector2 origin = sourceRectangle.Size() / 2f;
        Vector2 drawPos = Projectile.Center - Main.screenPosition;
        Vector2 offset = -texture.Size().RotatedBy(Projectile.rotation + (Projectile.spriteDirection+ 1) / 2 * PiOver2) * 0.3f;
        Vector2 squish =  Vector2.One;
        Color color = Color.White;
        color.A -= (byte)Projectile.alpha;
        color *= color.A / 255f;

        //Main.spriteBatch.End(out var parameters);
        //Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone);

        Main.EntitySpriteDraw(texture, drawPos + offset, sourceRectangle, color, Projectile.rotation + 0, origin, Projectile.scale * 1f * squish, spriteEffects, 0);

        //Main.spriteBatch.End();
        //Main.spriteBatch.Begin(parameters); 
        
        return false;
    }
}