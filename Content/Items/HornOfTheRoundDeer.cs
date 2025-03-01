using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Buffs;

namespace TheBindingOfRarria.Content.Items
{
    public class HornOfTheRoundDeer : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 26;
            Item.height = 32;
            Item.rare = ItemRarityID.Expert;
            Item.value = Item.buyPrice(0, 2);
            Item.expert = true;
        }
        public int counter = 0;
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (counter <= 0 && player.statLife <= player.statLifeMax2 / 3)
            {
                player.AddBuff(ModContent.BuffType<ReverseCT>(), 100);
                counter = 1800;
            }
            counter--;
        }
    }
    public class JJKDeerChestLoot : ModSystem
    {
        public override void PostWorldGen()
        {
            if (!Main.expertMode)
                return;
            for (int chestIndex = 0; chestIndex < Main.maxChests; chestIndex++)
            {
                Chest chest = Main.chest[chestIndex];
                if (chest == null)
                {
                    continue;
                }
                Tile chestTile = Main.tile[chest.x, chest.y];
                if (chestTile.TileType == TileID.Containers && chestTile.TileFrameX == 3 * 36)
                {
                    if (WorldGen.genRand.NextFloat() > 0.2f)
                        continue;

                    for (int inventoryIndex = 0; inventoryIndex < Chest.maxItems; inventoryIndex++)
                    {
                        if (chest.item[inventoryIndex].type == ItemID.None)
                        {
                            chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<HornOfTheRoundDeer>());
                            break;
                        }
                    }
                }
            }
        }
    }
}