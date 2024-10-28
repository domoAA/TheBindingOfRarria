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
            int index = Array.FindIndex(Main.projectile, proj => proj.active && proj.owner == Player.whoAmI && proj.type == ModContent.ProjectileType<ReflectiveRib>());

            if (!IsTheRibbler || index != -1)
                return;

            var position = new Microsoft.Xna.Framework.Vector2(Player.Center.X, Player.Center.Y - 40);// + new Microsoft.Xna.Framework.Vector2(-30, 30);
            Projectile.NewProjectile(Player.GetSource_FromThis(), position, new Microsoft.Xna.Framework.Vector2(5, 5), ModContent.ProjectileType<ReflectiveRib>(), 9, 1, Player.whoAmI);
        }
    }
}