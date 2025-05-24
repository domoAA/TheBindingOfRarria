using System;
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
        var p = player.GetModPlayer<RedMushPlayer>();

        if (player.CanFitSpace((int)((p.Growth - 1) * 42)))
            p.Growth = 
            Math.Min(
                1.5f, 
                player.GetModPlayer<RedMushPlayer>().Growth + 0.015f
                );
    }
}

public class RedMushPlayer : ModPlayer
{
    public float Growth = 1;

    public override void ResetEffects()
    {

    }

    public override void PostUpdateEquips()
    {
        ResizedPlayerUtils.SetScale(Player, Growth);
        Growth = Math.Max(1, Growth - 0.005f);
    }
}