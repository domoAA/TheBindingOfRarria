using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Common.Systems;

namespace TheBindingOfRarria.Content.Items;

public class Virus : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.width = 20;
        Item.height = 26;
        Item.rare = ItemRarityID.Master;
        Item.value = Item.sellPrice(0, 2, 28);
        Item.master = true;
        Item.expertOnly = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual) => HPDecreaseSystem.Virus = true;

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.ZombieArm)
            .AddIngredient(ItemID.RottenChunk, 2)
            .AddIngredient(ItemID.Bottle)
            .AddTile(TileID.Bottles)
            .AddCondition(Condition.InExpertMode)
            .Register();
    }
}

