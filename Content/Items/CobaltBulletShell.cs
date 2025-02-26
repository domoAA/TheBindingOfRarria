using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using TheBindingOfRarria.Content.Buffs;
using Terraria.ModLoader.IO;
using System.IO;
using Terraria.DataStructures;
using System.Collections.Generic;

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
            player.GetModPlayer<FerromagneticPlayer>().Ferromagnetic = true;
        }
    }
    public class FerromagneticPlayer : ModPlayer
    {
        public bool Ferromagnetic = false;
        public override void ResetEffects()
        {
            Ferromagnetic = false;
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Ferromagnetic && BulletGlobalProjectile.Bullet.Contains(proj.type))
            {
                target.AddBuff(ModContent.BuffType<MagneticField>(), 180);
            }
        }
    }
    public class BulletGlobalProjectile : GlobalProjectile
    {
        public static HashSet<int> Bullet = [];
        public override bool InstancePerEntity => true;
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            var owner = Main.player[projectile.owner];
            if (!Bullet.Contains(projectile.type) && projectile.aiStyle == ProjAIStyleID.Arrow && projectile.friendly && !owner.HeldItem.IsAir && owner.HeldItem != null && owner.ChooseAmmo(owner.HeldItem).ammo == AmmoID.Bullet)
                Bullet.Add(projectile.type);
            
        }
    }
}