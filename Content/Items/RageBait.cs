
using System.Linq;
using Terraria.GameContent.ItemDropRules;

namespace TheBindingOfRarria.Content.Items;

public class RageBait : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.width = 28;
        Item.height = 30;
        Item.value = Item.sellPrice(0, 5, 30);
        Item.rare = ItemRarityID.Expert;
        Item.expert = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual) => player.GetModPlayer<RageFishPlayer>().Bait = true;
}

public class CrateLootRageBait : GlobalItem
{
    public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
    {
        if (item.type == ItemID.CrimsonFishingCrate || item.type == ItemID.CrimsonFishingCrateHard)
        {
            IItemDropRule rule = ItemDropRule.ByCondition(new Conditions.IsExpert(), ModContent.ItemType<RageBait>(), 6);
            itemLoot.Add(rule);
        }
    }
}

public class RageFishPlayer : ModPlayer
{
    public bool Bait = false;

    public override void ResetEffects() => Bait = false;

    public override void CatchFish(FishingAttempt attempt, ref int itemDrop, ref int npcSpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition)
    {
        base.CatchFish(attempt, ref itemDrop, ref npcSpawn, ref sonar, ref sonarPosition);

        if (npcSpawn <= 0 && Main.rand.NextBool(6) && Bait)
        {
            var failNum = 100;
            while (attempt.rolledEnemySpawn <= 0 && npcSpawn <= 0 && failNum > 0)
            {
                failNum--;
                Main.projectile.FirstOrDefault()?.FishingCheck_RollEnemySpawns(ref attempt);
                npcSpawn = attempt.rolledEnemySpawn;
            }

            if (npcSpawn > 0)
                itemDrop = -1;
        }
    }
}