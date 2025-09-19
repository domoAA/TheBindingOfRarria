using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Items;

namespace TheBindingOfRarria.Content.WorldGeneration;

public class BlueSealChestLoot : ModSystem
{
    public static int VAmount = 0;
    public override void PostWorldGen()
    {
        for (int chestIndex = 0; chestIndex < Main.maxChests; chestIndex++)
        {
            Chest chest = Main.chest[chestIndex];

            if (chest == null)
                continue;

            if (VAmount < 8 && (VAmount < 5 || WorldGen.genRand.NextFloat() > 0.2f) && chest.item.Any(item => item.type == ItemID.BandofRegeneration || item.type == ItemID.MagicMirror || item.type == ItemID.CloudinaBottle || item.type == ItemID.HermesBoots || item.type == ItemID.Mace || item.type == ItemID.EnchantedBoomerang || item.type == ItemID.ShoeSpikes))
            {
                for (int inventoryIndex = Chest.maxItems - 1; inventoryIndex > 0; inventoryIndex--)
                {
                    chest.item[inventoryIndex].SetDefaults(chest.item[inventoryIndex - 1].type);
                    if (inventoryIndex == 1)
                    {
                        VAmount++;
                        chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<BlueSeal>());
                        break;
                    }
                }

            }
        }
    }
}