using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using TheBindingOfRarria.Content.Tiles;

namespace TheBindingOfRarria.Content.WorldGeneration;

public class MardroemeTree : GlobalTile
{
    public static int counter = 0;

    public override void RandomUpdate(int x, int y, int type)
    {
        if (!Main.rand.NextBool(1000) || !Main.hardMode)
            return;

        if (Main.tile[x, y].TileType == TileID.Trees && Main.tile[x + 1, y].TileType != ModContent.TileType<MardroemeTile>() && Main.tile[x - 1, y].TileType != ModContent.TileType<MardroemeTile>())
        {
            WorldGen.GetTreeBottom(x, y, out int a, out int b);
            if (WorldGen.GetTreeType(Main.tile[a, b].TileType) != Terraria.Enums.TreeTypes.Forest)
                return;

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