using Terraria.ModLoader;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;

namespace TheBindingOfRarria.Content.Items
{
    public class TheLastFinger : ModItem
    {
        public override void SetDefaults()
        {
            Item.height = 30;
            Item.width = 26;
            Item.accessory = true;
            Item.rare = ItemRarityID.Expert;
            Item.value = Item.buyPrice(0, 1, 12);
            Item.expert = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statManaMax2 += 100;
            player.aggro -= 500;
        }
    }
    public class CrateLootThukunaFinger : GlobalItem
    {
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            if (item.type == ItemID.CrimsonFishingCrateHard)
            {
                var rule = ItemDropRule.ByCondition(new Conditions.IsExpert(), ModContent.ItemType<TheLastFinger>(), 6);
                itemLoot.Add(rule);
            }
        }
    }
    public class ThukunaLootNPC : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.BigMimicCorruption)
            {
                npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsExpert(), ModContent.ItemType<TheLastFinger>(), 6));
            }
            base.ModifyNPCLoot(npc, npcLoot);
        }
    }
}