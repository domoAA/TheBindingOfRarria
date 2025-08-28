using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Projectiles;

namespace TheBindingOfRarria.Content.Items;

public class GoldenHalberd : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.height = 94;
        Item.width = 94;
        Item.shoot = ModContent.ProjectileType<GoldenHalberdProj>();
        Item.damage = 153;
        Item.DamageType = DamageClass.MeleeNoSpeed;
        Item.noUseGraphic = true;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.noMelee = true;
        Item.useTime = 65;
        Item.useAnimation = 65;
        Item.useTurn = false;
        Item.value = Item.sellPrice(gold: 1, silver: 60);
        Item.rare = ItemRarityID.Pink;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.GoldAxe)
            .AddIngredient(ItemID.HallowedBar, 5)
            .AddIngredient(ModContent.ItemType<PaleOre>(), 15)
            .AddTile(TileID.MythrilAnvil)
            .Register();
    }
}