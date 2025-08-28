using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Items;

namespace TheBindingOfRarria.Content.WorldGeneration;

public class SolitudeArmorChestLoot : ModSystem
{
    public static int SAmount = 0;
    public override void PostWorldGen()
    {
        if (!Main.expertMode)
            return;

        for (int chestIndex = 0; chestIndex < Main.maxChests; chestIndex++)
        {
            Chest chest = Main.chest[chestIndex];

            if (chest == null)
                continue;

            if (SAmount < 4 && (SAmount < 2 || WorldGen.genRand.NextFloat() > 0.8f) && chest.item.Any(item => item.type == ItemID.ShadowKey))
            {
                for (int inventoryIndex = Chest.maxItems - 1; inventoryIndex > 2; inventoryIndex--)
                {
                    chest.item[inventoryIndex].SetDefaults(chest.item[inventoryIndex - 3].type);
                }
                chest.item[0].SetDefaults(ModContent.ItemType<SolitudeHelmet>());
                chest.item[1].SetDefaults(ModContent.ItemType<SolitudePlatemail>());
                chest.item[2].SetDefaults(ModContent.ItemType<SolitudeGreaves>());
                SAmount++;
            }
        }
    }
}