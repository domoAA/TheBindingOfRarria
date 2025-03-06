using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Projectiles;

namespace TheBindingOfRarria.Content.Items
{
    public class PhoenixKunai : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 18;
            Item.height = 30;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<KunaiPlayer>().kunai = Item;
        }
    }
    public class KunaiPlayer : ModPlayer
    {
        public Item kunai = null;
        public int counter = -1;
        public override void ResetEffects()
        {
            kunai = null;
        }
        public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (kunai != null)
            {
                counter = counter < 3 ? counter + 1 : 0;
                if (counter == 0)
                {
                    var pos = Player.Center + new Vector2(30, 30).RotatedByRandom(MathHelper.TwoPi);

                    Projectile.NewProjectile(Player.GetSource_Accessory(kunai), pos, (pos - Main.screenPosition).DirectionTo(new Vector2(Main.mouseX, Main.mouseY)) * 14, ModContent.ProjectileType<FlyingKunai>(), damage / 2, 1, Player.whoAmI);
                }
            }
            
            return base.Shoot(item, source, position, velocity, type, damage, knockback);
        }
    }
}