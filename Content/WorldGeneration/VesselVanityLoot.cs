using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using TheBindingOfRarria.Content.Items;
using System.Linq;

namespace TheBindingOfRarria.Content.WorldGeneration;

public class VesselVanityLoot : ModSystem
{
    public static int VAmount = 0;
    public override void PostWorldGen()
    {
        for (int chestIndex = 0; chestIndex < Main.maxChests; chestIndex++)
        {
            Chest chest = Main.chest[chestIndex];

            if (chest == null)
                continue;

            if (Main.expertMode && VAmount < 5 && (VAmount < 4 || WorldGen.genRand.NextFloat() > 0.8f) && chest.item.Any(item => item.type == ItemID.MagicMirror || item.type == ItemID.ShoeSpikes || item.type == ItemID.FlareGun))
            {
                for (int inventoryIndex = 0; inventoryIndex < Chest.maxItems; inventoryIndex++)
                {
                    if (chest.item[inventoryIndex].type == ItemID.None)
                    {
                        VAmount++;
                        if (Main.rand.NextBool())
                            chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<VesselMask0>());
                        else
                            chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<VesselMask1>());

                        break;
                    }
                }

            }
        }
    }
}