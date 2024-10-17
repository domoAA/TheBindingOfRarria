using Terraria;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items
{
    public class CupidsArrow : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 30;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<CupidArrowPlayer>().HasTheThing = true;
        }
    }
    public class CupidArrowPlayer : ModPlayer
    {
        public bool HasTheThing = false;
        public override void ResetEffects()
        {
            HasTheThing = false;
        }
    }
}