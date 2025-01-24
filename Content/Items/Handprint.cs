using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Buffs;

namespace TheBindingOfRarria.Content.Items
{
    public class Handprint : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemTrader.ChlorophyteExtractinator.AddOption_FromAny(ModContent.ItemType<Handprint>(), ItemID.DesertFossil);
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.buyPrice(0, 0, 50);
            Item.rare = ItemRarityID.Green;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<HandbrickPlayer>().HasABrick = true;
        }
    }
    public class HandbrickPlayer : ModPlayer
    {
        public bool HasABrick = false;
        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            if (HasABrick)
                npc.AddBuff(ModContent.BuffType<RevengeDebuff>(), 720);
        }
        public override void ResetEffects()
        {
            HasABrick = false;
        }
    }
}