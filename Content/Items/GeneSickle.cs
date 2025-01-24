using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Buffs;

namespace TheBindingOfRarria.Content.Items
{
    public class GeneSickle : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.height = 30;
            Item.width = 32;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (!player.HasBuff(ModContent.BuffType<LifePool>()))
                player.AddBuff(ModContent.BuffType<LifePool>(), 3600);
        }
    }
    public class GeneThiefPlayer : ModPlayer
    {
        public int genePool;
        public override void PostUpdate()
        {
            if (!Player.HasBuff(ModContent.BuffType<LifePool>()))
                genePool = 0;
            base.PostUpdate();
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!target.active && genePool < 300)
                genePool += 3;

            base.OnHitNPC(target, hit, damageDone);
        }
    }
}