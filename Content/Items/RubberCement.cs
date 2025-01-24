using Terraria;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items
{
    public class RubberCement : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.height = 26;
            Item.width = 24;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<CementosPlayer>().Cementos = true;
        }
    }
    public class CementosPlayer : ModPlayer
    {
        public bool Cementos = false;
        public override void ResetEffects()
        {
            Cementos = false;
        }
    }
}