using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Common.Helpers;
using TheBindingOfRarria.Content.Projectiles;

namespace TheBindingOfRarria.Content.Items;

public class Censer : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.width = 26;
        Item.height = 34;
        Item.accessory = true;
        Item.value = Item.sellPrice(0, 9);
        Item.rare = ItemRarityID.LightPurple;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        if (Main.myPlayer == player.whoAmI)
            player.SpawnProjectileIfNotSpawned(ModContent.ProjectileType<SlowingAura>(), player.Center, player.GetSource_Accessory(Item));
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.FastClock)
            .AddIngredient(ItemID.PutridScent)
            .AddIngredient(ItemID.SoulofLight, 10)
            .AddTile(TileID.TinkerersWorkbench)
            .Register();
    }
}