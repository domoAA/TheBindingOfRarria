using System;
using System.Linq;
using TheBindingOfRarria.Common.Systems;

namespace TheBindingOfRarria.Content.Items;

public class OccultSkull : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.width = 40;
        Item.height = 40;
        Item.rare = ItemRarityID.Master;
        Item.value = Item.sellPrice(0, 6, 66);
        Item.master = true;
        Item.expertOnly = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        HPDecreaseSystem.Occult = true;

        int target = -1;
        int hp = 0;

        foreach (var t in Main.npc)
        {
            if (t.active && !t.friendly && !t.immortal && t.CanBeChasedBy() && t.lifeMax > hp)
            {
                hp = t.lifeMax;
                target = t.whoAmI;
            }
        }

        if (target > -1)
        {
            var t = Main.npc[target];
            player.statLifeMax2 += (int)(player.GetModPlayer<GeneThiefPlayer>().maxHP * (t.lifeMax - t.life) / (float)t.lifeMax) / 2;
            var name = t.FullName;
        }
    }
}

public class OccultItemNPCShop : GlobalNPC
{
    public override void ModifyActiveShop(NPC npc, string shopName, Item[] items)
    {
        if (npc.type == NPCID.Merchant && Main.masterMode && NPC.downedBoss3)
        {

            var index = Array.FindLastIndex(items, i => i is not null && (i.type == ItemID.IronAnvil || i.type == ItemID.LeadAnvil));
            if (index == -1)
                return;

            for (int i = items.Length - 1; i > index; i--)
            {
                items[i] = items[i - 1];
            }

            items[index + 1] = new Item(ModContent.ItemType<OccultSkull>());
        }
    }
}