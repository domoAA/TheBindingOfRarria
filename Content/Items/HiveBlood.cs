using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using System;
using System.Linq;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

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
            player.GetModPlayer<HiveBloodPlayer>().Hive = Item;
        }
    }
    public class HiveBloodPlayer : ModPlayer
    {
        public bool RespectsBees = false;
        public Item Hive = null;
        public override void ResetEffects()
        {
            RespectsBees = false;
            Hive = null;
        }
        public void BeeHeal(int damage, PlayerDeathReason playerDeathReason)
        {
            if (!RespectsBees)
                return;

            Player.Heal(damage / 20);
            if (Main.myPlayer == 255 || Hive == null)
                return;

            for (int i = Main.rand.Next(1, 7); i > 0; i--)
            {
                Projectile.NewProjectile(Player.GetSource_Accessory_OnHurt(Hive, playerDeathReason), Player.Center, new Vector2(5, 5).RotatedByRandom(MathHelper.TwoPi), ProjectileID.Bee, damage / 10, 2, Player.whoAmI);
            }

            // Those who respect bees
            // Those who don't bother them
            // Those they don't sting
            // Those they bring honey for
        }
        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
        {
            BeeHeal(hurtInfo.SourceDamage, hurtInfo.DamageSource);
            base.OnHitByProjectile(proj, hurtInfo);
        }
        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            BeeHeal(hurtInfo.SourceDamage, hurtInfo.DamageSource);
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