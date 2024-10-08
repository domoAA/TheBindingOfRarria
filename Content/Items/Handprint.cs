using Terraria;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Buffs;

namespace TheBindingOfRarria.Content.Items
{
    public class Handprint : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 32;
            Item.height = 32;
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