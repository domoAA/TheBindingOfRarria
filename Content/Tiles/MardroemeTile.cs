using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.DataStructures;

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


        TileObjectData.addTile(Type);

        AddMapEntry(Color.YellowGreen, Language.GetText("Mods.TheBindingOfRarria.Tiles.MardroemeTile.MapEntry"));
    }

    public override void PlaceInWorld(int i, int j, Item item)
    {
        if (Main.rand.NextBool()) 
            Main.tile[i, j].TileFrameX += 48;
    }

    public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
    {
        base.SetDrawPositions(i, j, ref width, ref offsetY, ref height, ref tileFrameX, ref tileFrameY);
    }
}