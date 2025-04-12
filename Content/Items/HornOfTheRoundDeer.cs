using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Buffs;

namespace TheBindingOfRarria.Content.Items;

public class HornOfTheRoundDeer : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.width = 26;
        Item.height = 32;
        Item.rare = ItemRarityID.Expert;
        Item.value = Item.buyPrice(0, 2);
        Item.expert = true;
    }

    private int counter = 0;

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
