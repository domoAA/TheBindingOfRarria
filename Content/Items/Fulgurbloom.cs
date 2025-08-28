using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Tiles;

namespace TheBindingOfRarria.Content.Items;

public class Fulgurbloom : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetStaticDefaults()
    {
        ItemID.Sets.DisableAutomaticPlaceableDrop[Type] = true; 
        Item.ResearchUnlockCount = 25;
    }

    public override void SetDefaults()
    {
        Item.DefaultToPlaceableTile(ModContent.TileType<FulgurbloomTile>());
        Item.width = 34;
        Item.height = 30;
        Item.value = Item.sellPrice(silver: 15);
        Item.rare = ItemRarityID.Green;
    }
}