using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Items;

namespace TheBindingOfRarria.Content.WorldGeneration;

public class SandclockChestLoot : ModSystem
{
    public static int SAmount = 0;
    public override void PostWorldGen()
    {
        for (int chestIndex = 0; chestIndex < Main.maxChests; chestIndex++)
        {
            Chest chest = Main.chest[chestIndex];

            if (chest == null)
                continue;

            if (Main.masterMode && chest.item.Any(item => item.type == ItemID.FlyingCarpet || item.type == ItemID.SandstorminaBottle || item.type == ItemID.PharaohsMask || item.type == ItemID.PharaohsRobe))
            {
                for (int i = chest.item.Length - 1; i >= 0; i--)
                {
                    if (i == 0) chest.item[0] = new Item(ModContent.ItemType<MysticSandclock>());

                    else chest.item[i] = chest.item[i - 1];
                }

                SAmount++;
                break;
            }
        }
    }
}