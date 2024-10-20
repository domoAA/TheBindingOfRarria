using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using TheBindingOfRarria.Content.Projectiles;

namespace TheBindingOfRarria.Content.Items
{
    public class Fireball : ModItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(8, 4));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 32;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<FireballPlayer>().fireBALLS = 1;
        }
    }
    public class FireballPlayer : ModPlayer
    {
        public int fireBALLS = 0;
        public override void ResetEffects()
        {
            fireBALLS = 0;
        }
    }
    public class ItemShootingFireball : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override void UseAnimation(Item item, Player player)
        {
            if (player.GetModPlayer<FireballPlayer>().fireBALLS <= 0)
                return;

            float modifier = 60 / item.useAnimation;
            var velocity = modifier * player.Center.DirectionTo(new Vector2(Main.mouseX, Main.mouseY)) * (player.Center.DistanceSQ(new Vector2(Main.mouseX, Main.mouseY)) / 9);
            Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, velocity, ModContent.ProjectileType<FlyingFireball>(), 10, 3, player.whoAmI, modifier);
        }
    }
}