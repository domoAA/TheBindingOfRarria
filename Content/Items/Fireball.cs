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
            //var increase => increase == (int)(player.GetDamage(DamageClass.Generic) * 4);
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

            float modifier = item.useAnimation;
            var velocity = player.Center.DirectionTo(new Vector2(Main.mouseX + Main.screenPosition.X, Main.mouseY + Main.screenPosition.Y)) * 9;
            var total = player.GetModPlayer<FireballPlayer>().fireBALLS;
            if (total % 2 == 0)
                velocity = velocity.RotatedBy(-MathHelper.PiOver4 / 8f);
            var rotation = 0f;
            for (int i = 0; i < total; i++)
            {
                var direction = i % 2 == 0 ? -i : i;
                rotation += MathHelper.PiOver2 / 8f * direction;
                Projectile.NewProjectile(player.GetSource_ItemUse(item), player.Center, velocity.RotatedBy(rotation), ModContent.ProjectileType<FlyingFireball>(), 1, 3, player.whoAmI, modifier);
            }
        }
    }
}