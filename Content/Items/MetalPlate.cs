using System;
using Terraria;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items
{
    public class MetalPlate : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 22;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PlatedPlayer>().HasPlate = true;
        }
    }
    public class PlatedPlayer : ModPlayer
    {
        public bool HasPlate = false;
        public bool Reflected = false;
        public override void ResetEffects()
        {
            HasPlate = false;
            Reflected = false;
        }
        public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
        {
            base.ModifyHitByProjectile(proj, ref modifiers);
            if (!HasPlate)
                return;

            Reflected = Player.ReflectProjectiles(Player.getRect(), DamageClass.Ranged, 0.25f);
        }
        public override bool FreeDodge(Player.HurtInfo info)
        {
            if (HasPlate && Reflected)
            {
                return true;
            }
            return base.FreeDodge(info);
        }
    }
}