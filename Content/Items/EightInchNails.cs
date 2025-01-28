using Terraria;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items
{
    public class EightInchNails : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 36;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<ProjectileTransformPlayer>().IsNailed = true;
        }
    }
    public class ProjectileTransformPlayer : ModPlayer
    {
        public bool IsNailed = false;
        public override void ResetEffects()
        {
            IsNailed = false;
        }
    }
}