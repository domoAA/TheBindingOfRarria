using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using static Terraria.GameContent.Drawing.TileDrawing;
using TheBindingOfRarria.Content.Items;

namespace TheBindingOfRarria.Content.Tiles;

public class MardroemeTile : ModTile
{
    public override string Texture => ContentPath + "Tiles/" + Name;

    public override void SetStaticDefaults()
    {
        Main.tileFrameImportant[Type] = true;
        Main.tileSolid[Type] = false;
        Main.tileNoAttach[Type] = true;
        Main.tileNoFail[Type] = true;
        Main.tileCut[Type] = true;

        DustType = DustID.YellowStarfish;

        
        //TileObjectData.newTile.CopyFrom(TileObjectData.StyleTorch);
        TileObjectData.newTile.CoordinateHeights = [22];
        TileObjectData.newTile.CoordinateWidth = 22;
        TileObjectData.newTile.CoordinatePadding = 2;
        TileObjectData.newTile.DrawXOffset = -12;
        TileObjectData.newTile.DrawYOffset = -2;
        TileObjectData.newTile.AnchorLeft = new AnchorData(AnchorType.Tree, TileObjectData.newTile.Height, 0);
        TileObjectData.newTile.StyleHorizontal = true;

        TileObjectData.newAlternate.CoordinateHeights = [22];
        TileObjectData.newAlternate.CoordinateWidth = 22;
        TileObjectData.newAlternate.CoordinatePadding = 2;
        TileObjectData.newAlternate.DrawXOffset = 12;
        TileObjectData.newAlternate.DrawYOffset = -2;
        TileObjectData.newAlternate.AnchorRight = new AnchorData(AnchorType.Tree, TileObjectData.newAlternate.Height, 0);
        TileObjectData.newAlternate.StyleHorizontal = true;
        TileObjectData.addAlternate(1);

        TileObjectData.newAlternate.CoordinateHeights = [22];
        TileObjectData.newAlternate.CoordinateWidth = 22;
        TileObjectData.newAlternate.CoordinatePadding = 2;
        TileObjectData.newAlternate.DrawXOffset = -12;
        TileObjectData.newAlternate.DrawYOffset = -2;
        TileObjectData.newAlternate.AnchorLeft = new AnchorData(AnchorType.Tree, TileObjectData.newAlternate.Height, 0);
        TileObjectData.newAlternate.StyleHorizontal = true;
        TileObjectData.addAlternate(2);

        TileObjectData.newAlternate.CoordinateHeights = [22];
        TileObjectData.newAlternate.CoordinateWidth = 22;
        TileObjectData.newAlternate.CoordinatePadding = 2;
        TileObjectData.newAlternate.DrawXOffset = 12;
        TileObjectData.newAlternate.DrawYOffset = -2;
        TileObjectData.newAlternate.AnchorRight = new AnchorData(AnchorType.Tree, TileObjectData.newAlternate.Height, 0);
        TileObjectData.newAlternate.StyleHorizontal = true;
        TileObjectData.addAlternate(3);



        RegisterItemDrop(ModContent.ItemType<Mardroeme>());

        AddMapEntry(Color.YellowGreen, Language.GetText("Mods.TheBindingOfRarria.Tiles.MardroemeTile.MapEntry"));

        TileObjectData.addTile(Type);
    }

    public override void PostTileFrame(int i, int j, int up, int down, int left, int right, int upLeft, int upRight, int downLeft, int downRight)
    {
        var t = Main.tile[i, j];

        if (Main.tile[i + 1, j].TileType == TileID.Trees && t.TileFrameX % 48 == 0)
            t.TileFrameX += 24;

        else if (Main.tile[i - 1, j].TileType == TileID.Trees && t.TileFrameX % 48 != 0)
            t.TileFrameX += 24;
    }

    public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
    {
        Main.instance.TilesRenderer.AddSpecialPoint(i, j, TileCounterType.CustomNonSolid);

        return false;
    }

    public override void SpecialDraw(int i, int j, SpriteBatch spriteBatch)
    {
        DrawMushrooms(i, j, spriteBatch);
    }

    public void DrawMushrooms(int i, int j, SpriteBatch spriteBatch)
    {
        var texture = TextureAssets.Tile[Type];

        var tile = Main.tile[i, j];

        var rect = new Rectangle(tile.TileFrameX, tile.TileFrameY, texture.Width() / 4, texture.Height());

        var side = tile.TileFrameX / 24 % 2;
        var offset = new Vector2((side * 2 - 1) * 8 + side * 2, 0);

        //Vector2 zero = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange);

        spriteBatch.Draw(texture.Value, new Point(i, j).ToWorldCoordinates() - Main.screenPosition /* + zero*/ + offset, rect, Lighting.GetColor(i, j), 0, rect.Size() / 2, 1, SpriteEffects.None, 0);
    }
}

public class MardroemeGeneration : ModSystem
{
    public static int counter = 0;

    public override void PreUpdateWorld()
    {
        if (!Main.rand.NextBool(5000) || !Main.hardMode)
            return;

        for (int x = 200; x < Main.maxTilesX - 200; x++)
        {
            for (int y = 100; y < Main.worldSurface + 50; y++)
            {
                if (!Main.rand.NextBool(400))
                    continue;

                if (Main.tile[x, y].TileType == TileID.Trees && Main.tile[x + 1, y].TileType != ModContent.TileType<MardroemeTile>() && Main.tile[x - 1, y].TileType != ModContent.TileType<MardroemeTile>())
                {
                    if (Main.rand.NextBool())
                    {
                        if (WorldGen.TileEmpty(x + 1, y)) 
                        { 
                            WorldGen.PlaceTile(x + 1, y, ModContent.TileType<MardroemeTile>(), style: Main.rand.Next(2));
                            counter++;
                        }
                    }

                    else if (WorldGen.TileEmpty(x - 1, y)) 
                    { 
                        WorldGen.PlaceTile(x - 1, y, ModContent.TileType<MardroemeTile>(), style: Main.rand.Next(2));
                        counter++;
                    }
                }
            }
        }
    }
}