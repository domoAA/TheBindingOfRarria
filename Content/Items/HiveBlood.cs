using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items;

public class HiveBlood : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.height = 32;
        Item.width = 30;
        Item.value = Item.sellPrice(0, 6);
        Item.rare = ItemRarityID.Master;
        Item.master = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.AddBuff(BuffID.Honey, 2);
        player.GetModPlayer<HiveBloodPlayer>().RespectsBees = true;
    }
}

public class HiveBloodPlayer : ModPlayer
{
    public bool RespectsBees = false;

    public override void ResetEffects() => RespectsBees = false;
    
    public void BeeHeal(int damage)
    {
        if (!RespectsBees || Main.myPlayer != Player.whoAmI)
            return;

        Player.Heal(damage / 10 + 1);

            // Those who respect bees.
            // Those who don't bother them.
            // Those they don't sting.
            // Those they bring honey for.
                // Those who have to refactor kain-code.
                // Those who are peak (zen)
    }

    public override void OnHurt(Player.HurtInfo info) => BeeHeal(info.Damage);

}

public class QBBagLoot : GlobalItem
{
    public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
    {
        if (item.type == ItemID.QueenBeeBossBag)
        {
            IItemDropRule rule = new ItemDropWithConditionRule(ModContent.ItemType<HiveBlood>(), 10, 1, 1, new Conditions.IsMasterMode());
            itemLoot.Add(rule);
        }
    }
}