using Terraria;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items
{
    public class CursedChain : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<RiskyHealPlayer>().HasChoker = true;
        }
    }
    public class RiskyHealPlayer : ModPlayer
    {
        public bool HasChoker = false;
        public int totalHeal = 0;
        public float currentHeal = 0.24f;
        public int CD = 60;
        public override void ResetEffects()
        {
            HasChoker = false;
        }
        public override void PostUpdateEquips()
        {
            Player.endurance -= 0.5f;

            if (totalHeal != 0)
                CD--;
            else
                CD = 60;

            if (CD <= 0) {
                Player.Heal((int)(totalHeal * currentHeal));
                bool IsLast = currentHeal > 0.09f;
                currentHeal = IsLast ? currentHeal - 0.03f : 0.24f;
                if (IsLast)
                    totalHeal = 0;
                CD = 60; }
        }
        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) {
            OnHitByAnything(hurtInfo); }
        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) {
            OnHitByAnything(hurtInfo); }
        public void OnHitByAnything(Player.HurtInfo hurtInfo)
        {
            totalHeal += (int)(hurtInfo.Damage * 0.75f);
        }
    }
}