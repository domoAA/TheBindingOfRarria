using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.Localization;

namespace TheBindingOfRarria.Content.Items;

public class Mardroeme : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.width = 30;
        Item.height = 30;
        Item.maxStack = 999;
        Item.consumable = true;
        Item.useTime = 45;
        Item.useAnimation = 45;
        Item.useStyle = ItemUseStyleID.HoldUp;
        Item.value = Item.sellPrice(gold: 1);
        Item.rare = ItemRarityID.Green;
        Item.UseSound = SoundID.Item4;
    }

    public override bool CanUseItem(Player player)
    {
        if (player.GetModPlayer<MardroemePlayer>().counter > 0 || player.statLife < 50)
            return false;
        return true;
    }

    public override bool? UseItem(Player player)
    {
        player.Hurt(PlayerDeathReason.ByCustomReason(NetworkText.FromLiteral($"{player.name} sacrificed their vitality.")), 50, 0, false, false, 0, false, 999);
        player.GetModPlayer<MardroemePlayer>().counter = 300;

        return true;
    }
}

public class MardroemePlayer : ModPlayer
{
    public int counter = 0;
    public override void PostUpdate()
    {
        base.PostUpdate();
        if (counter > 0)
        {
            counter--;
            if (counter == 0)
            {
                Player.Heal(250);
            }
        }
    }
}