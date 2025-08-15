using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items;

public class UdjatEye : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.height = 20;
        Item.width = 32;
        Item.rare = ItemRarityID.Green;
        Item.value = Item.sellPrice(0, 1, 12);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.dangerSense = true;
        player.findTreasure = true;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.FossilOre, 30)
            .AddIngredient(ItemID.SpelunkerPotion, 8)
            .AddTile(TileID.Solidifier)
            .Register();
    }
}

public class UdjatExtractinatorDrop : GlobalItem
{
    public override void ExtractinatorUse(int extractType, int extractinatorBlockType, ref int resultType, ref int resultStack)
    {
        if (extractType == ItemID.DesertFossil && Main.rand.NextFloat() < 0.001f) 
        {
            resultStack = 1;
            resultType = ModContent.ItemType<UdjatEye>();
        }
        else
            base.ExtractinatorUse(extractType, extractinatorBlockType, ref resultType, ref resultStack);
    }
}