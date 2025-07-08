using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using TheBindingOfRarria.Content.Tiles;

namespace TheBindingOfRarria.Content.WorldGeneration;

public class ScaleySand : GlobalTile
{
    public static int counter = 0;

    public override void RandomUpdate(int x, int y, int type)
    {
        if (!Main.rand.NextBool(3000))
            return;

        if (Main.tile[x, y].TileType == TileID.Sand && WorldGen.TileEmpty(x, y - 1))
        {
            WorldGen.PlaceTile(x, y - 1, ModContent.TileType<SuspiciousScaleTile>(), style: Main.rand.Next(2));
            counter++;
        }
    }
}