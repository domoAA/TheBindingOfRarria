using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using TheBindingOfRarria.Content.Tiles;

namespace TheBindingOfRarria.Content.WorldGeneration;

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