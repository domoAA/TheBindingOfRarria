
namespace TheBindingOfRarria.Content.Items
{
    public class TojiNullWeapon : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 34;
            Item.height = 40;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<TojiNullPlayer>().counter--;
            if (player.GetModPlayer<TojiNullPlayer>().counter <= 0)
                player.GetModPlayer<TojiNullPlayer>().CanNullify = true;
        }
    }
    public class TojiNullPlayer : ModPlayer
    {
        public bool CanNullify = false;
        public int counter = 0;
        public override void ResetEffects()
        {
            CanNullify = false;
        }
        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (CanNullify && target.damage > 0)
            {
                counter = 900;
                target.GetGlobalNPC<NullifiedDamageNPC>().dmg = target.damage;
                target.AddBuff(ModContent.BuffType<NullifiedPowers>(), 300);
            }
        }
    }
    public class NullifiedDamageNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public int dmg = 0;
        public override bool PreAI(NPC npc)
        {
            /*if (dmg != 0) 
            {
                if (!npc.HasBuff(ModContent.BuffType<NullifiedPowers>()))
                {
                    npc.damage = dmg;
                    dmg = 0;
                }
            }*/
            return base.PreAI(npc);
        }
        public override bool CanHitPlayer(NPC npc, Player target, ref int cooldownSlot)
        {
            if (npc.HasBuff(ModContent.BuffType<NullifiedPowers>()))
                return false;

            return base.CanHitPlayer(npc, target, ref cooldownSlot);
        }
        public override bool CanHitNPC(NPC npc, NPC target)
        {
            if (npc.HasBuff(ModContent.BuffType<NullifiedPowers>()))
                return false;

            return base.CanHitNPC(npc, target);
        }
    }
}