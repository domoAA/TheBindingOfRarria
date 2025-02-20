using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items
{
    // Errors will not show up here. Rename to work on this file. Orignal name:
    // HeavyBlow.cs
    public class HeavyBlow : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.height = 30;
            Item.width = 30;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            float knockback = .3f;
            player.GetKnockback<GenericDamageClass>() *= knockback;
            player.GetDamage<GenericDamageClass>() *= knockback;
            // ^^^ I feel like you have some more complicated idea in mind but... tada! 
        }
    }
}