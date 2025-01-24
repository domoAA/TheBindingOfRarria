using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria;
using TheBindingOfRarria.Content;
using TheBindingOfRarria.Content.Projectiles;

namespace TheBindingOfRarria.Common.UI
{
    public class ChargeUIElement : UIElement
    {
        public class ChargePlayer : ModPlayer
        {
            public bool IsASaint = false;
            public int Charge = 0;
            public int prev = 0;
            public int CD = 10;
            public override void ResetEffects()
            {
                IsASaint = false;
            }
            public override void PostUpdate()
            {
                base.PostUpdate();
                if (prev == CD || !IsASaint)
                    Charge = 0;

                prev = CD;


                var rotation = Main.MouseScreen.DirectionFrom(Player.Center - Main.screenPosition).ToRotation();

                var offset = new Vector2(24 * 15, 0).RotatedBy(rotation);

                if (Charge == 10) {
                    Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Vector2.Zero, ModContent.ProjectileType<LightBeam>(), 30, 3, Player.whoAmI, offset.X, offset.Y);
                    Charge = 0; }
            }
        }
        public class ChargePlayerItem : GlobalItem
        {
            public override void UseStyle(Item item, Player player, Rectangle heldItemFrame)
            {
                if (player.OwnsProjectile(ModContent.ProjectileType<LightBeam>()))
                    return;

                if (player.GetModPlayer<ChargePlayer>().CD <= 0) {
                    player.GetModPlayer<ChargePlayer>().Charge++;
                    player.GetModPlayer<ChargePlayer>().CD = 10; }
                else
                    player.GetModPlayer<ChargePlayer>().CD--;

                if (player.GetModPlayer<ChargePlayer>().Charge > 10)
                    player.GetModPlayer<ChargePlayer>().Charge = 0;

                base.UseStyle(item, player, heldItemFrame);
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (Main.dedServ)
                return;

            var texture = TheBindingOfRarria.ChargeIndicatorBar.Value;
            var player = Main.ActivePlayers.Find(player => player.GetModPlayer<ChargePlayer>().Charge > 0);

            if (player == null)
                return;

            var frame = texture.Frame(1, 10, 0, player.GetModPlayer<ChargePlayer>().Charge, 0, -2);

            Rectangle screen = new(0, 0, Main.screenWidth, Main.screenHeight);

            Vector2 UIPosition = new Vector2(player.MountedCenter.X - Main.screenPosition.X + 15 * Main.GameZoomTarget, player.MountedCenter.Y - Main.screenPosition.Y - Main.GameZoomTarget * 50) / Main.UIScale;
            Rectangle UI = new(
                (int)UIPosition.X - (frame.Width / 2),
                (int)UIPosition.Y - (frame.Height / 2),
                frame.Width,
                frame.Height);

            if (!screen.Contains(UI))
                return;

            spriteBatch.Draw(texture, UIPosition, frame, Color.White, 0, new Vector2(0, 0), (Main.GameZoomTarget / Main.UIScale) * 1.5f, SpriteEffects.None, 0);

        }
    }
    public class ChargeUIState : UIState
    {
        public ChargeUIElement element;
        public override void OnInitialize()
        {
            base.OnInitialize();
            element = new ChargeUIElement();
            element.IgnoresMouseInteraction = true;
            //element.Height.Set(26, 0);
            //element.Width.Set(22, 0);
            Append(element);
        }
    }
    [Autoload(Side = ModSide.Client)]
    public class ChargeUISystem : ModSystem
    {
        internal ChargeUIState state;
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
            state = new ChargeUIState();
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

            layers.Insert(index, new LegacyGameInterfaceLayer("TheBindingOfRarria: Charge UI", delegate
            {
                Interface.Draw(Main.spriteBatch, new GameTime());
                return true;
            }, InterfaceScaleType.UI));
        }
    }
}