using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Items;

namespace TheBindingOfRarria.Content.WorldGeneration;

public class JJKDeerChestLoot : ModSystem
{
    public static int DAmount = 0;
    public override void PostWorldGen()
    {
        if (!Main.expertMode)
            return;

        for (int chestIndex = 0; chestIndex < Main.maxChests; chestIndex++)
        {
            Chest chest = Main.chest[chestIndex];

            if (chest == null)
                continue;

            Tile chestTile = Main.tile[chest.x, chest.y];
            if (chestTile.TileType == TileID.Containers && (chestTile.TileFrameX == 3 * 36 || chestTile.TileFrameX == 4 * 36))
            {
                if (WorldGen.genRand.NextFloat() > 0.2f)
                    continue;

                for (int inventoryIndex = 0; inventoryIndex < Chest.maxItems; inventoryIndex++)
                {
                    if (chest.item[inventoryIndex].type == ItemID.None)
                    {
                        DAmount++;
                        chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<HornOfTheRoundDeer>());
                        break;
                    }
                }
            }
        }
    }
}