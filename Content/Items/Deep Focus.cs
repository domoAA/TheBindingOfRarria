using Terraria.ModLoader;
using Terraria;

namespace TheBindingOfRarria.Content.Items
{
    public class DeepFocus : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.height = 30;
            Item.width = 30;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<FocusedPlayer>().IsFocused = true;

            player.potionDelayTime = (int)(player.potionDelayTime * 1.2f);
            player.mushroomDelayTime = (int)(player.mushroomDelayTime * 1.2f);
            player.restorationDelayTime = (int)(player.restorationDelayTime * 1.2f);
        }
    }
    public class FocusedPlayer : ModPlayer
    {
        public bool IsFocused;
        public override void ResetEffects()
        {
            IsFocused = false;
        }

        public override void GetHealLife(Item item, bool quickHeal, ref int healValue)
        {
            if (IsFocused)
            {
                healValue *= 2;
            }
        }
    }
}
