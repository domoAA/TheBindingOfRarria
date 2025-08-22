using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items;

[AutoloadEquip(EquipType.Body)]
public class GodrickSoldierTabard : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetStaticDefaults()
    {
        ArmorIDs.Body.Sets.HidesTopSkin[Item.bodySlot] = true;
    }

    public override void SetDefaults()
    {
        Item.width = 30;
        Item.height = 26;
        Item.defense = 4;
        Item.value = Item.sellPrice(0, 2, 0, 0);
    }

    public override void UpdateEquip(Player player)
    {
        player.endurance += 0.05f;
    }
}

public partial class GodrickSoldierItemsNPCShop : GlobalNPC
{
    public void AddTabard(ref Item[] items)
    {

        var index = Array.FindIndex(items, e => e != null && e.type == ModContent.ItemType<GodrickSoldierHelm>());

        if (index != -1)
        {
            if (items[index + 1] == null)
                items[index + 1] = new Item(ModContent.ItemType<GodrickSoldierTabard>())
                {
                    shopCustomPrice = 5,
                    shopSpecialCurrency = CustomCurrencyID.DefenderMedals

                };
            else
            {
                for (int i = items.Length - 1; i > index + 1; i--)
                {
                    items[i] = items[i - 1];
                }

                items[index + 1] = new Item(ModContent.ItemType<GodrickSoldierTabard>())
                {
                    shopCustomPrice = 5,
                    shopSpecialCurrency = CustomCurrencyID.DefenderMedals

                };
            }
        }
    }
}