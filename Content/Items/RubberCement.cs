using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items
{
    public class RubberCement : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.height = 26;
            Item.width = 24;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.buyPrice(0, 1, 12);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<CementosPlayer>().Cementos = true;
        }
    }
    public class CementosPlayer : ModPlayer
    {
        public bool Cementos = false;
        public override void ResetEffects()
        {
            Cementos = false;
        }
    }
    public class QSBagLoot : GlobalItem
    {
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            if (item.type == ItemID.QueenSlimeBossBag)
            {
                var rule = new ItemDropWithConditionRule(ModContent.ItemType<RubberCement>(), 4, 1, 1, new Conditions.IsExpert());
                itemLoot.Add(rule);
            }
        }
    }
    public class QSLootNPC : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.QueenSlimeBoss)
            {
                npcLoot.Add(ItemDropRule.ByCondition(new Conditions.NotExpert(), ModContent.ItemType<RubberCement>(), 6));
            }
            base.ModifyNPCLoot(npc, npcLoot);
        }
    }
}