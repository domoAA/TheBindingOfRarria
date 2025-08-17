using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Items;

namespace TheBindingOfRarria.Content.Tiles;

public class PaleOreTile : ModTile
{
    public override string Texture => ContentPath + "Tiles/" + Name;

    public override void SetStaticDefaults()
    {
        RegisterItemDrop(ModContent.ItemType<PaleOre>());

        TileID.Sets.Ore[Type] = true;
        TileID.Sets.FriendlyFairyCanLureTo[Type] = true;
        Main.tileSpelunker[Type] = true;
        Main.tileOreFinderPriority[Type] = 404;
        Main.tileShine2[Type] = true;
        Main.tileShine[Type] = 600;
        Main.tileMergeDirt[Type] = true;
        Main.tileSolid[Type] = true;
        Main.tileBlockLight[Type] = true;

        LocalizedText name = CreateMapEntryName();
        AddMapEntry(Color.GhostWhite, name);

        DustType = DustID.WhiteTorch;
        HitSound = SoundID.Tink;
        MineResist = 4f;
        MinPick = 50;
    }

    public override bool CanExplode(int x, int y) => false;

    public override void RandomUpdate(int x, int y)
    {
        Point16 random = new(Main.rand.Next(-1, 2), Main.rand.Next(-1, 2));

        Point16 randomNeighbor = random + new Point16(x, y);
        Tile neighbor = Main.tile[randomNeighbor];

        if (Main.rand.NextFloat() < 0.0005f)
        {
            if (WorldGen.SolidOrSlopedTile(neighbor) && neighbor.TileType == TileID.IceBlock)
            {
                neighbor.ResetToType((ushort)ModContent.TileType<PaleOreTile>());
                WorldGen.SquareTileFrame(randomNeighbor.X, randomNeighbor.Y);
                NetMessage.SendTileSquare(-1, randomNeighbor.X, randomNeighbor.Y, 1);
            }
            else if (
                neighbor.TileType != TileID.LihzahrdBrick &&
                neighbor.TileType != TileID.BlueDungeonBrick &&
                neighbor.TileType != TileID.PinkDungeonBrick &&
                neighbor.TileType != TileID.GreenDungeonBrick &&
                TileID.Count > neighbor.TileType &&
                (!Main.tileOreFinderPriority.Contains((short)neighbor.TileType) ||
                Main.tileOreFinderPriority[neighbor.TileType] < 404))
            {
                neighbor.ResetToType((ushort)TileID.IceBlock);

                WorldGen.SquareTileFrame(randomNeighbor.X, randomNeighbor.Y);
                NetMessage.SendTileSquare(-1, randomNeighbor.X, randomNeighbor.Y, 1);
            }
        }
    }
}
