using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using System.Collections.Generic;

namespace TheBindingOfRarria.Content.Buffs.DoTUI
{
    public class DoTUIElement : UIElement
    {
        public Texture2D poisonTexture = ModContent.Request<Texture2D>("TheBindingOfRarria/Content/Buffs/PoisonSign").Value;
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            Texture2D texture = ModContent.Request<Texture2D>("TheBindingOfRarria/Content/Buffs/PoisonSign").Value;
            spriteBatch.Draw(texture, new Vector2(Main.screenWidth - 20, Main.screenHeight - 130) / 2, Color.White);
        }
    }
    public class DoTUIState : UIState
    {
        public DoTUIElement element;
        public override void OnInitialize()
        {
            base.OnInitialize();
            element = new DoTUIElement();
            element.IgnoresMouseInteraction = true;
            //element.Height.Set(26, 0);
            //element.Width.Set(22, 0);
            Append(element);
        }
    }
    [Autoload(Side = ModSide.Client)]
    public class DoTUISystem : ModSystem
    {
        internal DoTUIState state;
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
            state = new DoTUIState();
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
            int index = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (index == -1)
                return;

            layers.Insert(index, new LegacyGameInterfaceLayer("TheBindingOfRarria: DoT UI", delegate
            {
                Interface.Draw(Main.spriteBatch, new GameTime());
                return true;
            }, InterfaceScaleType.UI));
        }
    }
}
