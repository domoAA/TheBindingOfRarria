using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items
{
    [AutoloadEquip(EquipType.Face)]
    public class SuspiciousLookingMask : ModItem
    {
        public override void SetDefaults()
        {
            Item.height = 30;
            Item.width = 28;
            Item.accessory = true;
            Item.rare = ItemRarityID.Expert;
            Item.value = Item.buyPrice(0, 3);
            Item.expert = true;
        }
        public Dictionary<int, int> immunities = [];
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<CrazyPlayer>().Insane = true;

            foreach (var buff in player.buffType) {
                if (Main.debuff[buff] && !immunities.ContainsKey(buff))
                    immunities.Add(buff, 300); }

            foreach (var immunity in immunities)
            {
                immunities[immunity.Key] -= 1;
                if (immunity.Value < -600 || (!player.HasBuff(immunity.Key) && immunity.Value > 0))
                    immunities.Remove(immunity.Key);
                else if (immunity.Value < 0)
                    player.buffImmune[immunity.Key] = true;
            }
        }
        public override void AddRecipes()
        {
            Recipe.Create(Item.type)
                .AddIngredient(ItemID.BoneHelm)
                .AddIngredient(ModContent.ItemType<MedicalIceBag>())
                .AddIngredient(ItemID.Vitamins)
                .AddIngredient(ItemID.SoulofNight, 20)
                .AddTile(TileID.ImbuingStation)
                .Register();

            base.AddRecipes();
        }
    }
    public class CrazyPlayer : ModPlayer
    {
        public bool Insane = false;
        public override void ResetEffects()
        {
            Insane = false;
        }
        public void OnHitByAnything(Player.HurtInfo info, Vector2 target)
        {
            for (int i = Main.rand.Next(1, 3); i > 0; i--)
            {
                var offset = new Vector2(Main.screenWidth * Main.rand.NextFloat(0.2f, 0.8f), Main.screenHeight * Main.rand.NextFloat(0.2f, 0.8f));
                var pos = Main.screenPosition + offset;
                pos += pos.DirectionTo(target) * (pos.Distance(target) / 2 - 50);
                Projectile.NewProjectile(Player.GetSource_OnHurt(info.DamageSource), pos, pos.DirectionTo(target) * 6, ProjectileID.InsanityShadowFriendly, info.SourceDamage / 5 + 5, 3, Player.whoAmI);
            }
        }
        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            if (Insane)
                OnHitByAnything(hurtInfo, npc.Center);
            base.OnHitByNPC(npc, hurtInfo);
        }
        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
        {
            if (Insane)
            {
                var pos = Player.Center;
                var distance = 800f * 800;
                foreach (var target in Main.ActiveNPCs)
                {
                    if (!target.friendly && target.Center.DistanceSQ(Player.Center) < distance)
                    {
                        distance = target.Center.DistanceSQ(Player.Center);
                        pos = target.Center;
                    }
                }
                OnHitByAnything(hurtInfo, pos);
            }
            base.OnHitByProjectile(proj, hurtInfo);
        }
    }
}