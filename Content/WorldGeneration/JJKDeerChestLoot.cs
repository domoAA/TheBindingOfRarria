using System.Linq;
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

            if (DAmount < 8 && (DAmount < 4 || WorldGen.genRand.NextFloat() > 0.7f) && chest.item.Any(item => item.type == ItemID.DarkLance || item.type == ItemID.Sunfury || item.type == ItemID.FlowerofFire))
            {
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