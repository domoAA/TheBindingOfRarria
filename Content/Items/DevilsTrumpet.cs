using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items
{
    public class DevilsTrumpet : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 32;
            Item.height = 30;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MasterMirrorPlayer>().IsEvilIncarnate = true;
        }
    }
    public class MasterMirrorPlayer : ModPlayer
    {
        public bool IsEvilIncarnate = false;
        public override void ResetEffects()
        {
            IsEvilIncarnate = false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
            if (IsEvilIncarnate && target.HasBuff(BuffID.Poisoned))
            {
                var index = target.FindBuffIndex(BuffID.Poisoned);
                var time = target.buffTime[index];
                target.DelBuff(index);
                target.AddBuff(BuffID.Venom, time);
            }
        }
    }
}