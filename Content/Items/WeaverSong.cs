using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace TheBindingOfRarria.Content.Items
{
    public class WeaverSong : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 30;
            Item.height = 30;
            Item.rare = ItemRarityID.Expert;
            Item.value = Item.buyPrice(0, 2, 28);
            Item.expert = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.maxMinions += 2;
            player.GetModPlayer<SpooderPlayer>().Spooder = true;
        }
    }
    public class SpooderPlayer : ModPlayer
    {
        public bool Spooder = false;
        public override void ResetEffects()
        {
            Spooder = false;
        }
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            base.ModifyHitNPCWithProj(proj, target, ref modifiers);
            
            if (proj.minion && Spooder)
            {
                target.GetSlowed(TheBindingOfRarria.State.Slow, 120);
            }
        }
    }
}