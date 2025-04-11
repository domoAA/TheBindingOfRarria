using Humanizer;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader.Config;
using Terraria.UI;

namespace TheBindingOfRarria.Common.UI;

public class SanguineGiftUI
{

    public class BloodStorageUIElement : UIElement
    {
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (Main.dedServ)
                return;

            //spriteBatch.End();
            //spriteBatch.Begin();


            var center = ModContent.GetInstance<ClientConfig>().SanguineGiftUIPosition * new Vector2(Main.screenWidth, Main.screenHeight) + new Vector2(25f, 25f);

            var texture = TheBindingOfRarria.BloodStorage["cd"].Value;
            var origin = texture.Size() / 2;
            var color = Color.White;
            color *= (150 * (1 / 255f));

            spriteBatch.Draw(texture, (center ), null, color with { A = 150, R = 70 }, 0, origin, 0.2f, SpriteEffects.None, 0); 
            


            texture = TheBindingOfRarria.BloodStorage["cdfiller"].Value;
            origin = texture.Size() / 2;

            var rect = texture.Bounds;
            rect.Height = (int)(rect.Height * (Main.LocalPlayer.GetModPlayer<SanguinePlayer>().Stored * (1f / (Main.LocalPlayer.statLifeMax2 / 10)))); 
            
            //color = Color.SpringGreen;
            //color *= (150 * (1 / 255f));

            color = ModContent.GetInstance<ClientConfig>().SanguineGiftUIColor;

            spriteBatch.Draw(texture, (center), rect, color with { A = 150 }, Pi, origin, 0.195f, SpriteEffects.None, 0);
            


            texture = TheBindingOfRarria.BloodStorage["cd"].Value;
            var offset = new Vector2(texture.Width / 12.5f, 0);
            Utils.DrawBorderString(spriteBatch, $"{Main.LocalPlayer.GetModPlayer<SanguinePlayer>().Stored}", (center + offset.RotatedBy(PiOver2)), Color.White, 1f, 0.5f, 0.5f);

            texture = TheBindingOfRarria.BloodStorage["orbsmol"].Value;
            origin = texture.Size() / 2;
            spriteBatch.Draw(texture, center, null, Color.White, 0, origin, 1, SpriteEffects.None, 0);

        }
    }
    public class BloodStorageUIState : UIState
    {
        public BloodStorageUIElement element;
        public StolenDraggableUIPanel panel;
        public override void OnInitialize()
        {
            base.OnInitialize();
            element = new BloodStorageUIElement();
            element.IgnoresMouseInteraction = true;
            //element.Height.Set(26, 0);
            //element.Width.Set(22, 0);

            //Append(element);

            panel = new StolenDraggableUIPanel();
            panel.SetPadding(0);
            SetRectangle(panel, left: Main.screenWidth * 0.78125f, top: Main.screenHeight * 0.13888f, width: 50f, height: 50f);

            panel.BackgroundColor = Color.Transparent;
            panel.BorderColor = Color.Transparent;


            panel.Append(element);
            Append(panel);
        }
        private void SetRectangle(UIElement uiElement, float left, float top, float width, float height)
        {
            uiElement.Left.Set(left, 0f);
            uiElement.Top.Set(top, 0f);
            uiElement.Width.Set(width, 0f);
            uiElement.Height.Set(height, 0f);
        }
    }
    [Autoload(Side = ModSide.Client)]
    public class BloodStorageUISystem : ModSystem
    {
        internal BloodStorageUIState state;
        private UserInterface Interface;
        public void Show()
        {
            Interface?.SetState(state);
        }
        public void Hide()
        {
            Interface?.SetState(null);
        }
        public override void Load()
        {
            base.Load();
            state = new BloodStorageUIState();
            Interface = new UserInterface();
            state.Activate();
        }
        public override void UpdateUI(GameTime gameTime)
        {
            base.UpdateUI(gameTime);
            Interface?.Update(gameTime);
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            base.ModifyInterfaceLayers(layers);
            int index = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Entity Health Bars"));
            if (index == -1)
                return;

            layers.Insert(index, new LegacyGameInterfaceLayer("TheBindingOfRarria: Blood Storage", delegate
            {
                Interface.Draw(Main.spriteBatch, new GameTime());
                return true;
            }, InterfaceScaleType.UI));
        }
    }
    public class StolenDraggableUIPanel : UIPanel
    {
        // Stores the offset from the top left of the UIPanel while dragging
        private Vector2 offset;
        // A flag that checks if the panel is currently being dragged
        private bool dragging;

        public override void LeftMouseDown(UIMouseEvent evt)
        {
            // When you override UIElement methods, don't forget call the base method
            // This helps to keep the basic behavior of the UIElement
            base.LeftMouseDown(evt);
            // When the mouse button is down on this element, then we start dragging
            if (evt.Target == this && ModContent.GetInstance<ClientConfig>().SanguineGiftUIDraggable)
            {
                DragStart(evt);
            }
        }

        public override void LeftMouseUp(UIMouseEvent evt)
        {
            base.LeftMouseUp(evt);
            // When the mouse button is up, then we stop dragging
            if (evt.Target == this && ModContent.GetInstance<ClientConfig>().SanguineGiftUIDraggable)
            {
                DragEnd(evt);
            }
        }

        private void DragStart(UIMouseEvent evt)
        {
            // The offset variable helps to remember the position of the panel relative to the mouse position
            // So no matter where you start dragging the panel, it will move smoothly
            if (ModContent.GetInstance<ClientConfig>().SanguineGiftUIDraggable)
            {
                offset = new Vector2(evt.MousePosition.X - Left.Pixels, evt.MousePosition.Y - Top.Pixels);
                dragging = true;
            }
        }

        private void DragEnd(UIMouseEvent evt)
        {
            if (ModContent.GetInstance<ClientConfig>().SanguineGiftUIDraggable)
            {
                Vector2 endMousePosition = evt.MousePosition;
                dragging = false;

                Left.Set(endMousePosition.X - offset.X, 0f);
                Top.Set(endMousePosition.Y - offset.Y, 0f);

                Recalculate();

                ModContent.GetInstance<ClientConfig>().SanguineGiftUIPosition = new Vector2(GetOuterDimensions().X / Main.screenWidth, GetOuterDimensions().Y / Main.screenHeight);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Checking ContainsPoint and then setting mouseInterface to true is very common
            // This causes clicks on this UIElement to not cause the player to use current items
            if (ContainsPoint(Main.MouseScreen))
            {
                Main.LocalPlayer.mouseInterface = true;
            }

            if (dragging)
            {
                Left.Set(Main.mouseX - offset.X, 0f); // Main.MouseScreen.X and Main.mouseX are the same
                Top.Set(Main.mouseY - offset.Y, 0f);
                Recalculate();

                ModContent.GetInstance<ClientConfig>().SanguineGiftUIPosition = new Vector2(GetOuterDimensions().X / Main.screenWidth, GetOuterDimensions().Y / Main.screenHeight);
            }
            else
            {
                Left.Set(ModContent.GetInstance<ClientConfig>().SanguineGiftUIPosition.X * Main.screenWidth, 0f);
                Top.Set(ModContent.GetInstance<ClientConfig>().SanguineGiftUIPosition.Y * Main.screenHeight, 0f);
                Recalculate();
            }

            // Here we check if the DraggableUIPanel is outside the Parent UIElement rectangle
            // (In our example, the parent would be ExampleCoinsUI, a UIState. This means that we are checking that the DraggableUIPanel is outside the whole screen)
            // By doing this and some simple math, we can snap the panel back on screen if the user resizes his window or otherwise changes resolution
            var parentSpace = Parent.GetDimensions().ToRectangle();
            if (!GetDimensions().ToRectangle().Intersects(parentSpace))
            {
                Left.Pixels = Utils.Clamp(Left.Pixels, 0, parentSpace.Right - Width.Pixels);
                Top.Pixels = Utils.Clamp(Top.Pixels, 0, parentSpace.Bottom - Height.Pixels);
                // Recalculate forces the UI system to do the positioning math again.
                Recalculate();
            }
        }
    }
}