using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.UI.Chat;
using TheBindingOfRarria.Content.Items;

namespace TheBindingOfRarria.Content.Buffs;

public class YghernBuff : ModBuff
{
    public override string Texture => ContentPath + "Buffs/" + Name;

    public override void PostDraw(SpriteBatch spriteBatch, int buffIndex, BuffDrawParams drawParams)
    {
        DynamicSpriteFont font = FontAssets.MouseText.Value;
        string text = $"{Main.LocalPlayer.GetModPlayer<YghernAccPlayer>().Block}";

        Vector2 position = drawParams.Position + drawParams.Texture.Size();

        Vector2 origin = drawParams.Texture.Size() * 0.5f;

        var color = Color.White;
        color.A = 100;

        ChatManager.DrawColorCodedStringWithShadow(spriteBatch, font, text, position, color, 0f, origin, new Vector2(0.7f));
    }
}