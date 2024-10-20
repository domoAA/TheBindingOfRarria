using System;
using Terraria;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Projectiles;

namespace TheBindingOfRarria.Content.Items
{
    public class SlippedRib : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 24;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<SlipperyRibblerPlayer>().IsTheRibbler = true;
        }
    }
    public class SlipperyRibblerPlayer : ModPlayer
    {
        public bool IsTheRibbler = false;
        public override void ResetEffects()
        {
            IsTheRibbler = false;
        }
        public override void PostUpdateEquips()
        {
            if (!IsTheRibbler || Player.ownedProjectileCounts[ModContent.ProjectileType<ReflectiveRib>()] > 0)
                return;

            var position = Player.Center + new Microsoft.Xna.Framework.Vector2(-30, 30);
            Projectile.NewProjectile(Player.GetSource_FromThis(), position, new Microsoft.Xna.Framework.Vector2(0, 0), ModContent.ProjectileType<ReflectiveRib>(), 9, 1, Player.whoAmI);
        }
    }
}