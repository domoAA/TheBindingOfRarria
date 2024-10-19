using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ReLogic.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using System.Collections.Generic;
using Terraria.GameContent;
using TheBindingOfRarria.Content.Buffs;
using System.Linq;

namespace TheBindingOfRarria.Content.Buffs.DoTUI
{
    public class DoTUIElement : UIElement
    {
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            Texture2D texturePoison = TheBindingOfRarria.PoisonTexture.Value; //ModContent.Request<Texture2D>("TheBindingOfRarria/Content/Buffs/PoisonSign").Value;
            Texture2D textureFire = TheBindingOfRarria.FireTexture.Value;//ModContent.Request<Texture2D>("TheBindingOfRarria/Content/Buffs/FireSign").Value;
            if (Main.dedServ)
                return;

            Rectangle screen = new(0, 0, Main.screenWidth, Main.screenHeight);
            float offset = -20;
            List<DamageOverTtimeUserInterfacePlayer.DamageOverTimeType> usedTypes = [];
            List<DamageOverTtimeUserInterfacePlayer.DamageOverTimeType> totalTypes = [];

            foreach (var type in DamageOverTtimeUserInterfacePlayer.DamageOverTimeUICollection.Keys) {
                if (!totalTypes.Contains(type.Item2))
                    totalTypes.Add(type.Item2); }
            offset = totalTypes.Count % 2 != 0 ? offset : 0;

            foreach (var member in DamageOverTtimeUserInterfacePlayer.DamageOverTimeUICollection)
            {
                Texture2D texture = texturePoison;
                switch (member.Key.Item2)
                {
                    case DamageOverTtimeUserInterfacePlayer.DamageOverTimeType.Poison:
                        texture = texturePoison;
                        break;
                    case DamageOverTtimeUserInterfacePlayer.DamageOverTimeType.Fire:
                        texture = textureFire;
                        break;
                    default:
                        texture = texturePoison;
                        continue;
                }

                if (!usedTypes.Contains(member.Key.Item2))
                {
                    usedTypes.Add(member.Key.Item2);
                    offset += usedTypes.Count % 2 != 0 ? usedTypes.Count * 20 : -usedTypes.Count * 20;
                }

                var player = member.Key.Item1;
                Vector2 UIPosition = new Vector2(player.MountedCenter.X - Main.screenPosition.X - 10 * Main.GameZoomTarget + offset, player.MountedCenter.Y - Main.screenPosition.Y - Main.GameZoomTarget * 60) / Main.UIScale;
                Rectangle UI = new(
                    (int)UIPosition.X - (texture.Width / 2), 
                    (int)UIPosition.Y - (texture.Height / 2), 
                    texture.Width, 
                    texture.Height);

                if (screen.Contains(UI))
                    spriteBatch.Draw(texture, UIPosition, null, Color.White, 0, new(0, 0), Main.GameZoomTarget / Main.UIScale, SpriteEffects.None, 0);
                Utils.DrawBorderString(spriteBatch, member.Value.ToString(), new Vector2(UIPosition.X + (texture.Width / 2), UIPosition.Y + (texture.Height / 2)), Color.White);
                //spriteBatch.DrawString(FontAssets.MouseText.Value, member.Value.ToString(), new Vector2(UIPosition.X + (texture.Width / 2), UIPosition.Y + (texture.Height / 2)), Color.White);
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