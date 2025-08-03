using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using TheBindingOfRarria.Common.Config;
using TheBindingOfRarria.Common.Helpers;
using TheBindingOfRarria.Common.Registries;
using TheBindingOfRarria.Content.Items;

namespace TheBindingOfRarria.Common.UI;

public class BloodStorageUIElement : UIElement
{
    private static readonly Vector2 GiftOffset = new(25f);

    private static readonly Color OuterCircleColor = new(70, 160, 150, 150);
    private const float OuterCircleScale = 0.2f;

    private const byte InnerCircleOpacity = 150;
    private const float InnerCircleScale = 0.195f;

    public override void OnInitialize() => IgnoresMouseInteraction = true;

    public override void Draw(SpriteBatch spriteBatch)
    {
        Vector2 center = ModContent.GetInstance<ClientConfig>().SanguineGiftUIPosition * Helper.ScreenSize + GiftOffset;

        Texture2D texture = Textures.CD[0].Value;

        Vector2 origin = texture.Size() * 0.5f;

        spriteBatch.Draw(texture, center, null, OuterCircleColor, 0, origin, OuterCircleScale, SpriteEffects.None, 0);

        texture = Textures.CD[1].Value;
        origin = texture.Size() / 2;

        Rectangle rect = texture.Bounds;
        rect.Height = (int)(rect.Height * (Main.LocalPlayer.GetModPlayer<SanguinePlayer>().Stored * (1f / (Main.LocalPlayer.statLifeMax2 / 10))));

        Color innerColor = ModContent.GetInstance<ClientConfig>().SanguineGiftUIColor;
        innerColor.A = InnerCircleOpacity;

        spriteBatch.Draw(texture, center, rect, innerColor, Pi, origin, InnerCircleScale, SpriteEffects.None, 0);

            // What.
        texture = Textures.CD[0].Value;
        Vector2 offset = new(texture.Width / 12.5f, 0);
        Utils.DrawBorderString(spriteBatch, $"{Main.LocalPlayer.GetModPlayer<SanguinePlayer>().Stored}", center + offset.RotatedBy(PiOver2), Color.White, 1f, 0.5f, 0.5f);

        texture = Textures.BloodOrbs[1].Value;
        origin = texture.Size() * 0.5f;
        spriteBatch.Draw(texture, center, null, Color.White, 0, origin, 1, SpriteEffects.None, 0);
    }
}
