using Terraria;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Projectiles;

namespace TheBindingOfRarria.Content.Items
{
    public class FishTank : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.height = 36;
            Item.width = 34;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<FishiousPlayer>().IsTuna = true;
        }
    }
    public class FishiousPlayer : ModPlayer
    {
        public bool IsTuna = false;
        public override void ResetEffects()
        {
            if (IsTuna && Player.timeSinceLastDashStarted <= 1)
                // Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, new Microsoft.Xna.Framework.Vector2(0, 0), ModContent.ProjectileType<FloppingFish>(), 10, 6, Player.whoAmI);
                // Commented ^^^ this out so I could test it.
                IsTuna = false;
        }
    }
}