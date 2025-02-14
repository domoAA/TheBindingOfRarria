using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Projectiles;

namespace TheBindingOfRarria.Content.Items
{
    public class GodHead : ModItem
    {
        //public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        //{
        //    return !equippedItem.HasTag(TheBindingOfRarria.reflectItems) || !incomingItem.HasTag(TheBindingOfRarria.reflectItems);
        //}
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.height = 22;
            Item.width = 30;
            Item.value = Item.buyPrice(0, 6);
            Item.rare = ItemRarityID.Pink;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (Main.myPlayer == player.whoAmI)
                player.SpawnProjectileIfNotSpawned(ModContent.ProjectileType<CircleOfLight>(), player.GetSource_Accessory(Item));
        }
        public override void AddRecipes()
        {
            Recipe.Create(Item.type)
                .AddIngredient(ItemID.CrossNecklace)
                .AddIngredient(ItemID.SoulofLight, 3)
                .AddIngredient(ItemID.AngelStatue)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();

            base.AddRecipes();
        }
    }
}