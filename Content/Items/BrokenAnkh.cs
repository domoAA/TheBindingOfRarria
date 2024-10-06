using Terraria.ModLoader;
using Terraria;
using TheBindingOfRarria.Content.Buffs;

namespace TheBindingOfRarria.Content.Items
{
    public class BrokenAnkh : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 30;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (!player.HasBuff(ModContent.BuffType<AnubisCurse>()))
                Item.type = ModContent.ItemType<Ankh>();
        }
        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            Item.type = ModContent.ItemType<Ankh>();
        }
        public override void UpdateInventory(Player player)
        {
            Item.type = ModContent.ItemType<Ankh>();
        }
    }
}