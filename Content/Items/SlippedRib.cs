using Terraria;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Projectiles;

namespace TheBindingOfRarria.Content.Items
{
    public class SlippedRib : ModItem
    {
        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
            return !equippedItem.HasTag(TheBindingOfRarria.reflectItems) || !incomingItem.HasTag(TheBindingOfRarria.reflectItems);
        }
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 24;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.SpawnProjectileIfNotSpawned(ModContent.ProjectileType<ReflectiveRib>(), new Microsoft.Xna.Framework.Vector2(player.Center.X, player.Center.Y - 40));
        }
    }
}