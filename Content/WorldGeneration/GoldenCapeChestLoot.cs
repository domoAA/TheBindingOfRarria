using System.ComponentModel;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Items;

namespace TheBindingOfRarria.Content.WorldGeneration;

public class GoldenCapeChestLoot : ModSystem
{
    public static int GAmount = 0;
    public override void PostWorldGen()
    {
        for (int chestIndex = 0; chestIndex < Main.maxChests; chestIndex++)
        {
            Chest chest = Main.chest[chestIndex];

            if (chest == null)
                continue;

            Tile chestTile = Main.tile[chest.x, chest.y];
            if (chestTile.TileType == TileID.Containers)
            {
                if (WorldGen.genRand.NextFloat() > 0.2f)
                    continue;

                for (int inventoryIndex = 0; inventoryIndex < Chest.maxItems; inventoryIndex++)
                {
                    if (chest.item[inventoryIndex].type == ItemID.None)
                    {
                        GAmount++;
                        chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<GoldenCape>());
                        break;
                    }
                }
            }
        }
    }
}