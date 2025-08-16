using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace TheBindingOfRarria.Content.Items;

public class Aegis : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.height = 34;
        Item.width = 34;
        Item.rare = ItemRarityID.Pink;
        Item.value = Item.sellPrice(0, 9, 60);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<AegisPlayer>().Aegis = true;
    }
}

public class AegisPlayer : ModPlayer
{
    public bool Aegis = false;

    public override void PostUpdateMiscEffects()
    {
        if (Aegis)
        {
        }
    }
}