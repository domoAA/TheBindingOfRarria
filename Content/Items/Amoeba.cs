using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Common.Helpers;
using TheBindingOfRarria.Common.Registries;

namespace TheBindingOfRarria.Content.Items;

public class Amoeba : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.width = 20;
        Item.height = 20;
        Item.accessory = true;
    }

    public int counter = 0;

    public Vector2[] AuraFadedSides = [];

    public Vector2[] GetSides(int timer)
    {
        if (timer >= 120 || AuraFadedSides is null || AuraFadedSides.Length == 0)
            AuraFadedSides = [new Vector2(0.14f).RotatedByRandom(TwoPi), new Vector2(0.14f).RotatedByRandom(TwoPi), new Vector2(0.14f).RotatedByRandom(TwoPi), new Vector2(0.14f).RotatedByRandom(TwoPi)];

        return AuraFadedSides;
    }

    public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
    {
        counter = counter % 120 + 1;

        var effect = Effects.AmoebaShader?.Value;

        spriteBatch.End(out var snapshot);
        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.Default, Main.Rasterizer, effect, snapshot.matrix);


        Main.GetItemDrawFrame(Item.type, out var texture, out var itemFrame);

        

        //effect.Parameters["uImage"].SetValue(texture);
        effect?.Parameters["uSides"].SetValue(GetSides(counter));
        effect?.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly);
        effect?.CurrentTechnique.Passes[0].Apply();


        Vector2 drawOrigin = itemFrame.Size() / 2f;
        Vector2 drawPosition = Item.Bottom - Main.screenPosition - new Vector2(0, Item.height / 2);

        spriteBatch.Draw(texture, drawPosition, texture.Bounds, Color.White, rotation, texture.Size() / 2, scale, SpriteEffects.None, 0);

        spriteBatch.End();
        spriteBatch.Begin(snapshot);

        return false;
    }
}