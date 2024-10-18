using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ReLogic.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using System.Collections.Generic;
using Terraria.GameContent;

namespace TheBindingOfRarria.Content.Buffs.DoTUI
{
    public class DoTUIElement : UIElement
    {
        public Texture2D poisonTexture = ModContent.Request<Texture2D>("TheBindingOfRarria/Content/Buffs/PoisonSign").Value;
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            Texture2D texture = ModContent.Request<Texture2D>("TheBindingOfRarria/Content/Buffs/PoisonSign").Value;
            if (Main.dedServ)
                return;

            Rectangle screen = new(0, 0, Main.screenWidth, Main.screenHeight);

            foreach (var member in DamageOverTtimeUserInterfacePlayer.DamageOverTimeUICollection)
            {
                var player = member.Key.Item1;
                Vector2 UIPosition = new Vector2(player.MountedCenter.X - Main.screenPosition.X - 10 * Main.GameZoomTarget, player.MountedCenter.Y - Main.screenPosition.Y - Main.GameZoomTarget * 60) / Main.UIScale;
                Rectangle UI = new(
                    (int)UIPosition.X - (texture.Width / 2), 
                    (int)UIPosition.Y - (texture.Height / 2), 
                    texture.Width, 
                    texture.Height);

                if (screen.Contains(UI))
                    spriteBatch.Draw(texture, UIPosition, null, Color.White, 0, new(0, 0), Main.GameZoomTarget / Main.UIScale, SpriteEffects.None, 0);

                spriteBatch.DrawString(FontAssets.MouseText.Value, member.Value.ToString(), new Vector2(UIPosition.X + (texture.Width / 2), UIPosition.Y + (texture.Height / 2)), Color.White);
            }
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