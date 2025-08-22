using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using System;

namespace TheBindingOfRarria.Content.Items;

public class MysticSandclock : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.height = 32;
        Item.width = 24;
        Item.value = Item.sellPrice(0, 2);
        Item.rare = ItemRarityID.Master;
        Item.master = true;
    }

    public int counter = 0;

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        counter++;
        if (counter < 3)
            return;

        counter = 0;

        for (int i = 0; i < player.buffType.Length; i++)
        {
            if (Main.debuff[player.buffType[i]])
            {
                player.buffTime[i] = Math.Max(1, player.buffTime[i] - 1);
            }
        }
    }
}