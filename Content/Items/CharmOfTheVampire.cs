using Terraria;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items
{
    public class CharmOfTheVampire : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 40;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<VampirePlayer>().HasCharm = true;
        }
    }
    public class VampirePlayer : ModPlayer
    {
        public bool HasCharm = false;
        public int KillCount = 0;
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!HasCharm)
                return;

            if (target.life > 0 && target.life > damageDone)
                return;

            KillCount++;
            if (KillCount < 13)
                return;

            KillCount = 0;
            Player.Heal(10);
        }
        public override void ResetEffects()
        {
            HasCharm = false;
        }
    }
}