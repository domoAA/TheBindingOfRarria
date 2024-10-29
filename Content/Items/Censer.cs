using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace TheBindingOfRarria.Content.Items
{
    public class Censer : ModItem
    {
        public int prev = 0;
        public int num = 0;
        public int timer = 5;
        public float rotation = 0;
        public const float max = MathHelper.PiOver4 / 1.65f;
        public State state = State.Left;
        public enum State
        {
            Left,
            SlowLeft,
            Right,
            SlowRight
        }
        public override void SetStaticDefaults()
        {
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
            var draw = new DrawAnimationVertical(8, 4);
            Main.RegisterItemAnimation(Item.type, draw);
        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 36;
            Item.accessory = true;
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            scale *= 2;
            return base.PreDrawInWorld(spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            var position = Item.Center - Main.screenPosition;
            DrawExtra(spriteBatch, position, scale);
        }
        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            DrawExtra(spriteBatch, position, scale);
        }
        public void DrawExtra(SpriteBatch spriteBatch, Vector2 position, float scale)
        {
            var texture = TheBindingOfRarria.CenserExtra.Value;

            timer--;
            if (timer <= 0)
                timer = 5;


            if (timer == 5)
                num++;
            if (num >= 5)
                num = 0;

            switch (rotation)
            {
                case < -max:
                        state = State.Right;
                    break;
                case > max:
                        state = State.Left;
                    break;
            }

            rotation = Math.Clamp(rotation, -max, max);

            switch (state)
            {
                case State.Right:
                    rotation = 0.99f * rotation + MathHelper.Pi / 360;
                    break;
                case State.Left:
                    rotation = 0.99f * rotation - MathHelper.Pi / 360;
                    break;
            }

            var extraScale = 1.35f * scale;
            var extraFrame = texture.Frame(1, 5, 0, num, 0, -2);
            position += new Vector2(0, (10 * num) * extraScale).RotatedBy(rotation);

            position.Y += 3f;

            var color = Color.White;
            color.A += 20;

            spriteBatch.Draw(texture, position, extraFrame, color, rotation, extraFrame.Center(), extraScale, SpriteEffects.None, 0);

            prev = Main.itemAnimations[Item.type].FrameCounter;
        }
    }
}