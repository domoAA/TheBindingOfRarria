using MonoMod.Cil;
using System;
using Terraria;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items
{
    public class AdamantineTalisman : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.height = 30;
            Item.width = 26;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<DNDLuckRollPlayer>().Unfair = true;
        }
    }
    public class DNDLuckRollPlayer : ModPlayer
    {
        public bool Unfair = false;
        public static (float, int) DNDLuckRollDMG = (0, 0);
        public static bool DNDRolling = false;
        public override void ResetEffects()
        {
            Unfair = false;
        }
        public override void Load()
        {
            On_Projectile.HurtPlayer += SwitchDNDLuckRoll;
            
            On_Main.DamageVar_float_int_float += RegisterDNDLuckRoll;
            On_Player.Hurt_PlayerDeathReason_int_int_bool_bool_bool_int_bool_float += UseDNDLuckRoll;
            On_Player.Hurt_PlayerDeathReason_int_int_bool_bool_int_bool_float_float_float += UseDNDLuckRoll2;
        }

        private void SwitchDNDLuckRoll(On_Projectile.orig_HurtPlayer orig, Projectile self, Microsoft.Xna.Framework.Rectangle hitbox)
        {
            DNDRolling = true;
            orig(self, hitbox);
        }

        private double UseDNDLuckRoll2(On_Player.orig_Hurt_PlayerDeathReason_int_int_bool_bool_int_bool_float_float_float orig, Player self, Terraria.DataStructures.PlayerDeathReason damageSource, int Damage, int hitDirection, bool pvp, bool quiet, int cooldownCounter, bool dodgeable, float armorPenetration, float scalingArmorPenetration, float knockback)
        {
            if (DNDRolling && Damage == DNDLuckRollDMG.Item2 && self.GetModPlayer<DNDLuckRollPlayer>().Unfair)
            {
                Damage = Math.Max(1, (int)Math.Round(DNDLuckRollDMG.Item1 * (1 - Main.DefaultDamageVariationPercent * 0.01f)));
                DNDLuckRollDMG = (0, 0);
                DNDRolling = false;
            }
            return orig(self, damageSource, Damage, hitDirection, pvp, quiet, cooldownCounter, dodgeable, armorPenetration, scalingArmorPenetration, knockback);
        }

        private double UseDNDLuckRoll(On_Player.orig_Hurt_PlayerDeathReason_int_int_bool_bool_bool_int_bool_float orig, Player self, Terraria.DataStructures.PlayerDeathReason damageSource, int Damage, int hitDirection, bool pvp, bool quiet, bool Crit, int cooldownCounter, bool dodgeable, float armorPenetration)
        {
            if (DNDRolling && Damage == DNDLuckRollDMG.Item2 && self.GetModPlayer<DNDLuckRollPlayer>().Unfair)
            {
                Damage = Math.Max(1, (int)Math.Round(DNDLuckRollDMG.Item1 * (1 - Main.DefaultDamageVariationPercent * 0.01f)));
                DNDLuckRollDMG = (0, 0);
                DNDRolling = false;
            }
            return orig(self, damageSource, Damage, hitDirection, pvp, quiet, Crit, cooldownCounter, dodgeable, armorPenetration);
        }

        private int RegisterDNDLuckRoll(On_Main.orig_DamageVar_float_int_float orig, float dmg, int percent, float luck)
        {
            var result = orig(dmg, percent, luck);
            if (DNDRolling)
            {
                DNDLuckRollDMG.Item1 = dmg;
                DNDLuckRollDMG.Item2 = result;
            }
            return result;
        }
    }
}