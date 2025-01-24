using Terraria.ModLoader;
using Terraria;
using TheBindingOfRarria.Content.Buffs;
using Terraria.ID;

namespace TheBindingOfRarria.Content.Items
{
    public class BrokenAnkh : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 30;
            Item.accessory = true;
            Item.rare = ItemRarityID.LightPurple;
            Item.value = Item.buyPrice(0, 3);
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