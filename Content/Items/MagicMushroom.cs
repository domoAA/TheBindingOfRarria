using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Common.Systems;

namespace TheBindingOfRarria.Content.Items;

public class MagicMushroom : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.width = 24;
        Item.height = 28;
        Item.rare = ItemRarityID.LightRed;
        Item.value = Item.buyPrice(0, 2);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<ScaledDrawPlayer>().ApplyScaleToPlayer(player.whoAmI, 2);
        PlayerDrawScaleSystem.RegisterPlayerForDraw(player.whoAmI);
    }
}