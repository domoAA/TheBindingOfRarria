using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Projectiles;

namespace TheBindingOfRarria.Content.Items
{
    public class HiveBlood : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.height = 32;
            Item.width = 30;
            Item.value = Item.buyPrice(0, 6);
            Item.rare = ItemRarityID.Master;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.AddBuff(BuffID.Honey, 2);
            player.GetModPlayer<HiveBloodPlayer>().RespectsBees = true;
            player.GetModPlayer<HiveBloodPlayer>().counter--;
            if (Main.myPlayer != player.whoAmI || player.GetModPlayer<HiveBloodPlayer>().counter > 0)
                return;

            Projectile.NewProjectile(player.GetSource_Accessory(Item), player.Center, Vector2.Zero, ModContent.ProjectileType<HivePulse>(), player.statLifeMax2 / 100, 2, player.whoAmI);
            player.GetModPlayer<HiveBloodPlayer>().counter = 300;
        }
    }
    public class HiveBloodPlayer : ModPlayer
    {
        public bool RespectsBees = false;
        public int counter = 300;
        public override void ResetEffects()
        {
            RespectsBees = false;
        }
        public void BeeHeal(int damage)
        {
            if (!RespectsBees || Main.myPlayer != Player.whoAmI)
                return;

            Player.Heal(damage / 20);

            // Those who respect bees
            // Those who don't bother them
            // Those they don't sting
            // Those they bring honey for
        }
        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
        {
            BeeHeal(hurtInfo.SourceDamage);
            base.OnHitByProjectile(proj, hurtInfo);
        }
        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            BeeHeal(hurtInfo.SourceDamage);
            base.OnHitByNPC(npc, hurtInfo);
        }
    }
    public class QBBagLoot : GlobalItem
    {
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            if (item.type == ItemID.QueenBeeBossBag)
            {
                var rule = new ItemDropWithConditionRule(ModContent.ItemType<HiveBlood>(), 10, 1, 1, new Conditions.IsMasterMode());
                itemLoot.Add(rule);
            }
        }
    }
}