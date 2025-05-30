using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria;
using TheBindingOfRarria.Content.Items;
using Terraria.ObjectData;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
namespace TheBindingOfRarria.Content.Tiles;
using static Terraria.GameContent.Drawing.TileDrawing;
public class SuspiciousScaleTile : ModTile
{
    public override string Texture => ContentPath + "Tiles/" + Name;

    public override void SetStaticDefaults()
    {
        DustType = -1;
        MineResist = 3f;

        Main.tileSpelunker[Type] = true;
        Main.tileOreFinderPriority[Type] = 228;
        Main.tileShine2[Type] = true;
        Main.tileFrameImportant[Type] = true;
        Main.tileSolid[Type] = false;
        Main.tileNoAttach[Type] = true;

        TileObjectData.newTile.CoordinateHeights = [12];
        TileObjectData.newTile.CoordinateWidth = 16;
        TileObjectData.newTile.CoordinatePadding = 2;
        TileObjectData.newTile.DrawYOffset = 4;
        TileObjectData.newTile.AnchorBottom = new AnchorData(Terraria.Enums.AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
        TileObjectData.newTile.StyleHorizontal = true;

        TileObjectData.newAlternate.CoordinateHeights = [12];
        TileObjectData.newAlternate.CoordinateWidth = 16;
        TileObjectData.newAlternate.CoordinatePadding = 2;
        TileObjectData.newAlternate.DrawYOffset = 4;
        TileObjectData.newAlternate.AnchorBottom = new AnchorData(Terraria.Enums.AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
        TileObjectData.newAlternate.StyleHorizontal = true;
        TileObjectData.addAlternate(1);

        LocalizedText name = CreateMapEntryName();
        AddMapEntry(Color.DarkSlateGray, name);

        RegisterItemDrop(ModContent.ItemType<YghernScale>());

        TileObjectData.addTile(Type);
    }

    public override void PostTileFrame(int i, int j, int up, int down, int left, int right, int upLeft, int upRight, int downLeft, int downRight)
    {
        var t = Main.tile[i, j];

        if (Main.rand.NextBool())
            t.TileFrameX = 18;

        else t.TileFrameX = 0;
    }
    public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
    {
        Main.instance.TilesRenderer.AddSpecialPoint(i, j, TileCounterType.CustomNonSolid);

        return false;
    }

    public override void SpecialDraw(int i, int j, SpriteBatch spriteBatch)
    {
        DrawScales(i, j, spriteBatch);
    }

    public void DrawScales(int i, int j, SpriteBatch spriteBatch)
    {
        var texture = TextureAssets.Tile[Type];

        var tile = Main.tile[i, j];

        var rect = new Rectangle(tile.TileFrameX, tile.TileFrameY, texture.Width() / 2, texture.Height());

        var offset = new Vector2(0, 4);

        //Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);

        if (Main.tile[i, j + 1].IsHalfBlock)
            offset.Y += 8;

        spriteBatch.Draw(texture.Value, new Point(i, j).ToWorldCoordinates() - Main.screenPosition /* + zero*/ + offset, rect, Lighting.GetColor(i, j), 0, rect.Size() / 2, 1, SpriteEffects.None, 0);
    }
}