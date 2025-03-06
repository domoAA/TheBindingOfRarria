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
        public static (float original, int rolled) LuckRoll = (0, 0);
        public override void ResetEffects()
        {
            Unfair = false;
        }
        public override void Load()
        {
            On_Main.DamageVar_float_int_float += RegisterLuckRoll;

            On_Player.Hurt_PlayerDeathReason_int_int_bool_bool_bool_int_bool_float += UseLuckRoll;
            On_Player.Hurt_PlayerDeathReason_int_int_bool_bool_int_bool_float_float_float += UseLuckRoll2;
        }

        private double UseLuckRoll2(On_Player.orig_Hurt_PlayerDeathReason_int_int_bool_bool_int_bool_float_float_float orig, Player self, Terraria.DataStructures.PlayerDeathReason damageSource, int Damage, int hitDirection, bool pvp, bool quiet, int cooldownCounter, bool dodgeable, float armorPenetration, float scalingArmorPenetration, float knockback)
        {
            if (Damage % LuckRoll.rolled == 0 && self.GetModPlayer<DNDLuckRollPlayer>().Unfair)
            {
                Damage = Math.Max(1, (int)Math.Round(LuckRoll.original * (Damage / LuckRoll.rolled) * (1 - Main.DefaultDamageVariationPercent * 0.01f)));
                LuckRoll = (0, 0);
            }
            return orig(self, damageSource, Damage, hitDirection, pvp, quiet, cooldownCounter, dodgeable, armorPenetration, scalingArmorPenetration, knockback);
        }

        private double UseLuckRoll(On_Player.orig_Hurt_PlayerDeathReason_int_int_bool_bool_bool_int_bool_float orig, Player self, Terraria.DataStructures.PlayerDeathReason damageSource, int Damage, int hitDirection, bool pvp, bool quiet, bool Crit, int cooldownCounter, bool dodgeable, float armorPenetration)
        {
            if (Damage % LuckRoll.rolled == 0 && self.GetModPlayer<DNDLuckRollPlayer>().Unfair)
            {
                Damage = Math.Max(1, (int)Math.Round(LuckRoll.original * (Damage / LuckRoll.rolled) * (1 - Main.DefaultDamageVariationPercent * 0.01f)));
                LuckRoll = (0, 0);
            }
            return orig(self, damageSource, Damage, hitDirection, pvp, quiet, Crit, cooldownCounter, dodgeable, armorPenetration);
        }

        private int RegisterLuckRoll(On_Main.orig_DamageVar_float_int_float orig, float dmg, int percent, float luck)
        {
            var result = orig(dmg, percent, luck);

            LuckRoll.original = dmg;
            LuckRoll.rolled = result;
            
            return result;
        }
    }
}