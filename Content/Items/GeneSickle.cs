using System;
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
            player.AddBuff(ModContent.BuffType<LifePool>(), 2);
            player.GetModPlayer<GeneThiefPlayer>().counter--;
            if (player.GetModPlayer<GeneThiefPlayer>().counter < 0)
            {
                player.GetModPlayer<GeneThiefPlayer>().counter = 300;
                player.GetModPlayer<GeneThiefPlayer>().genePool -= player.GetModPlayer<GeneThiefPlayer>().genePool > 0 ? 1 : 0;
            }
            player.statLifeMax2 += player.GetModPlayer<GeneThiefPlayer>().genePool;
        }
    }
    public class GeneThiefPlayer : ModPlayer
    {
        public int genePool;
        public int counter = 300;
        public override void PostUpdate()
        {
            if (!Player.HasBuff(ModContent.BuffType<LifePool>()))
                genePool = 0;

            genePool = Math.Min(genePool, 150);

            base.PostUpdate();
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!target.active && genePool < 150)
                genePool += 1;

            base.OnHitNPC(target, hit, damageDone);
        }
    }
}