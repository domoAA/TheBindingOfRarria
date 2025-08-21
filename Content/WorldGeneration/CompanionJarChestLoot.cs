using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Items;

namespace TheBindingOfRarria.Content.WorldGeneration;

public class CompanionJarChestLoot : ModSystem
{
    public static int JAmount = 0;
    public override void PostWorldGen()
    {
        for (int chestIndex = 0; chestIndex < Main.maxChests; chestIndex++)
        {
            Chest chest = Main.chest[chestIndex];

            if (chest == null)
                continue;

            if (JAmount < 8 && (JAmount < 5 || WorldGen.genRand.NextFloat() > 0.7f) && chest.item.Any(item => item.type == ItemID.BandofRegeneration || item.type == ItemID.MagicMirror || item.type == ItemID.CloudinaBottle || item.type == ItemID.HermesBoots || item.type == ItemID.Mace || item.type == ItemID.EnchantedBoomerang || item.type == ItemID.ShoeSpikes))
            {
                for (int inventoryIndex = Chest.maxItems - 1; inventoryIndex > 0; inventoryIndex--)
                {
                    chest.item[inventoryIndex].SetDefaults(chest.item[inventoryIndex - 1].type);
                    if (inventoryIndex == 1)
                    {
                        JAmount++;
                        chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<CompanionJar>());
                        break;
                    }
                }

            }
        }
    }
}