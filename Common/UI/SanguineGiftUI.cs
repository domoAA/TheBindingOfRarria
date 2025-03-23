using Terraria.UI;

namespace TheBindingOfRarria.Common.UI
{
    public class SanguineGiftUI
    {

        public class BloodStorageUIElement : UIElement
        {
            public override void Draw(SpriteBatch spriteBatch)
            {
                base.Draw(spriteBatch);
                if (Main.dedServ)
                    return;

                spriteBatch.End();
                spriteBatch.Begin();

                var center = new Vector2(1250, 120);
                var texture = TheBindingOfRarria.BloodStorage["cd"].Value;
                var origin = texture.Size() / 2;
                var offset = new Vector2(texture.Width / 3.75f, 0);

                //for (float i = 0; i < TwoPi; i += 0.1f)
                //{
                    spriteBatch.Draw(texture, (center ), null, Color.White, 0, origin, 1f, SpriteEffects.None, 0); 
                
                texture = TheBindingOfRarria.BloodStorage["cdfiller"].Value;
                origin = texture.Size() / 2;
                var rect = texture.Bounds;
                rect.Height = (int)(rect.Height * (Main.LocalPlayer.GetModPlayer<SanguinePlayer>().Stored * (1f / (Main.LocalPlayer.statLifeMax2 / 10))));
                spriteBatch.Draw(texture, (center), rect, Color.LightGreen, Pi, origin, 1f, SpriteEffects.None, 0);
                //}

                /*texture = TheBindingOfRarria.BloodStorage["cdfiller"].Value;
                origin = texture.Size() / 2;
                for (float i = 0; i < TwoPi * (Main.LocalPlayer.GetModPlayer<SanguinePlayer>().Stored * (1f / (Main.LocalPlayer.statLifeMax2 / 6))); i += 0.1f)
                {
                    spriteBatch.Draw(texture, (center + offset.RotatedBy(i)), null, Color.White, 1, origin, 0.75f, SpriteEffects.None, 0);
                }*/

                texture = TheBindingOfRarria.BloodStorage["cd"].Value;
                offset = new Vector2(texture.Width / 2.5f, 0);
                Utils.DrawBorderString(spriteBatch, $"{Main.LocalPlayer.GetModPlayer<SanguinePlayer>().Stored}", (center + offset.RotatedBy(PiOver2)), Color.White, 1f, 0.5f, 0.5f);

                texture = TheBindingOfRarria.BloodStorage["orbsmol"].Value;
                origin = texture.Size() / 2;
                spriteBatch.Draw(texture, center, null, Color.White, 0, origin, 1, SpriteEffects.None, 0);

            }
        }
        public class BloodStorageUIState : UIState
        {
            public BloodStorageUIElement element;
            public override void OnInitialize()
            {
                base.OnInitialize();
                element = new BloodStorageUIElement();
                element.IgnoresMouseInteraction = true;
                //element.Height.Set(26, 0);
                //element.Width.Set(22, 0);
                Append(element);
            }
        }
        [Autoload(Side = ModSide.Client)]
        public class BloodStorageUISystem : ModSystem
        {
            internal BloodStorageUIState state;
            private UserInterface Interface;
            public void Show(UserInterface Interface)
            {
                Interface?.SetState(state);
            }
            public void Hide(UserInterface Interface)
            {
                Interface?.SetState(null);
            }
            public override void Load()
            {
                base.Load();
                state = new BloodStorageUIState();
                Interface = new UserInterface();
                state.Activate();
                Show(Interface);
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
    }
}