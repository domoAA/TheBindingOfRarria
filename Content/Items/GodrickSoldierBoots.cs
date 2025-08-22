using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items;

[AutoloadEquip(EquipType.Legs)]
public class GodrickSoldierBoots : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetStaticDefaults()
    {
        ArmorIDs.Legs.Sets.HidesBottomSkin[Item.legSlot] = true;
    }

    public override void SetDefaults()
    {
        Item.width = 22;
        Item.height = 16;
        Item.defense = 4;
        Item.value = Item.sellPrice(0, 1, 0, 0);
    }

    public override void UpdateEquip(Player player)
    {

    }
}

public partial class GodrickSoldierItemsNPCShop : GlobalNPC
{
    public void AddBoots(ref Item[] items)
    {

        var index = Array.FindIndex(items, e => e != null && e.type == ModContent.ItemType<GodrickSoldierTabard>());

        if (index != -1)
        {
            if (items[index + 1] == null)
                items[index + 1] = new Item(ModContent.ItemType<GodrickSoldierBoots>())
                {
                    shopCustomPrice = 4,
                    shopSpecialCurrency = CustomCurrencyID.DefenderMedals

                };
            else
            {
                for (int i = items.Length - 1; i > index + 1; i--)
                {
                    items[i] = items[i - 1];
                }

                items[index + 1] = new Item(ModContent.ItemType<GodrickSoldierBoots>())
                {
                    shopCustomPrice = 4,
                    shopSpecialCurrency = CustomCurrencyID.DefenderMedals

                };
            }
        }
    }
}