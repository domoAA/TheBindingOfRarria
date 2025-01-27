using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items
{
    public class GnawedLeaf : ModItem
    {
        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
            return !equippedItem.HasTag(TheBindingOfRarria.invulItems) || !incomingItem.HasTag(TheBindingOfRarria.invulItems);
        }
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.height = 24;
            Item.width = 22;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<VegeterianPlayer>().IsStupidEnoughToChompAFuckingLeaf = true;
            player.GetDamage(DamageClass.Generic).Flat -= 0.1f;
        }
    }
    public class VegeterianPlayer : ModPlayer
    {
        public bool IsStupidEnoughToChompAFuckingLeaf = false;
        public int counter = 180;
        public override void ResetEffects()
        {
            IsStupidEnoughToChompAFuckingLeaf = false;
        }
        public override void PostUpdate()
        {
            if (Player.velocity.LengthSquared() == 0 && !Player.ItemAnimationActive && IsStupidEnoughToChompAFuckingLeaf)
            {
                counter--;
            }
            else
                counter = 180;

            if (counter == 0)
            {
                Player.buffImmune[BuffID.Stoned] = false;
                if (!Player.HasBuff(BuffID.Stoned))
                {
                    Player.immune = true;
                    Player.immuneTime = 180;
                    Player.SetImmuneTimeForAllTypes(180);
                    Player.AddBuff(BuffID.Stoned, 180);
                }
            }
            else if (counter <= -180)
            {
                counter = 180;
            }
        }
    }
}