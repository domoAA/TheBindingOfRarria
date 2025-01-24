using Terraria;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items
{
    public class PushPin : ModItem
    {
        public override void SetDefaults()
        {
            Item.height = 24;
            Item.width = 22;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PinnedPlayer>().Pinned = true;
        }
    }
    public class PinnedPlayer : ModPlayer
    {
        public bool Pinned = false;
        public override void ResetEffects()
        {
            Pinned = false;
        }
    }
}