using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.DataStructures;
using System;
using Microsoft.Xna.Framework;
using TheBindingOfRarria.Content.Projectiles;
using TheBindingOfRarria.Content.Buffs;

namespace TheBindingOfRarria.Content.Items
{
    public class CobaltBulletShell : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 18;
            Item.height = 36;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.buyPrice(0, 0, 69);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MagnetoPlayer>().MagneticBullets = true;
        }
    }
    public class MagnetoPlayer : ModPlayer
    {
        public bool MagneticBullets = false;
        public override void ResetEffects()
        {
            MagneticBullets = false;
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (proj.GetGlobalProjectile<GlobalTypeProjectile>().bullet)
                target.AddBuff(ModContent.BuffType<MagneticPower>(), 300);
        }
    }
    public class GlobalTypeProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public bool bullet = false;
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (projectile.aiStyle == ProjAIStyleID.Arrow && projectile.friendly && Main.myPlayer == projectile.owner && !Main.LocalPlayer.HeldItem.IsAir && Main.LocalPlayer.HeldItem != null && Main.LocalPlayer.ChooseAmmo(Main.LocalPlayer.HeldItem).ammo == AmmoID.Bullet)
                bullet = true;
        }
    }
    public class MagnetizedNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public float power = 0;
        public Projectile MagneticField = null;
        public override void PostAI(NPC npc)
        {
            power = Math.Clamp(power, 0, 1);

            if (!npc.HasBuff(ModContent.BuffType<MagneticPower>()))
                power = 0;

            if (power > 0)
            {
                if (MagneticField != null)
                {
                    MagneticField.ai[0] = npc.Hitbox.Size().LengthSquared() / (100 * 100);
                    MagneticField.ai[1] = power;
                    MagneticField.Center = npc.Center;
                    MagneticField.timeLeft = 300;
                }
                else if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    MagneticField = Projectile.NewProjectileDirect(npc.GetSource_FromAI(), npc.Center, Vector2.Zero, ModContent.ProjectileType<MagneticField>(), 0, 0, Main.myPlayer, npc.Size.Length() / 20f, power);
                }
            }

            else if (MagneticField != null && MagneticField.active)
                MagneticField.Kill();
        }
    }
}