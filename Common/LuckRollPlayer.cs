using System;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria;

namespace TheBindingOfRarria.Common;

public class LuckRollPlayer : ModPlayer
{
    public bool Talisman = false;
    public bool Dice = false;

    public static (float original, int rolled) LuckRoll = (0, 0);

    public override void ResetEffects() => Talisman = false;

    public override void Load()
    {
        On_Main.DamageVar_float_int_float += RegisterLuckRoll;

        On_Player.Hurt_PlayerDeathReason_int_int_refHurtInfo_bool_bool_int_bool_float_float_float += UseLowLuckRoll;
    }

    public static int CustomRangeDamageVar(float dmg, int min = 85, int max = 115, float luck = 0f)
    {
        float result = dmg * Main.rand.Next(min, max + 1) / 100;

        if (luck == 0)
            return (int)Math.Round(result);

        float roll = dmg * Main.rand.Next(min, max + 1) / 100;

        if (Main.rand.NextFloat() < float.Abs(luck))
        {
            if (luck > 0f)
            {
                if (roll > result)
                    result = roll;
            }
            else if (roll < result)
                result = roll;
        }

        return (int)Math.Round(result);
    }

    public static void DiceReroll(Player self, ref int Damage)
    {
        if (LuckRoll.rolled != 0 && Damage % LuckRoll.rolled == 0 && self.GetModPlayer<LuckRollPlayer>().Dice)
        {
            int bound = self.GetModPlayer<LuckRollPlayer>().Talisman ? 100 : 100 + Main.DefaultDamageVariationPercent;

            int roll = Math.Max(1, CustomRangeDamageVar(Damage, 100 - Main.DefaultDamageVariationPercent, bound));

            if (roll > Damage)
                Damage = roll;
        }
    }
    public static void TalismanRoll(Player self, ref int Damage)
    {
        if (LuckRoll.rolled != 0 && Damage % LuckRoll.rolled == 0 && self.GetModPlayer<LuckRollPlayer>().Talisman)
        {
            Damage = CustomRangeDamageVar(Damage, 100 - Main.DefaultDamageVariationPercent, 100, -self.luck);
            LuckRoll.rolled = Damage;
        }
    }

    private double UseLowLuckRoll(On_Player.orig_Hurt_PlayerDeathReason_int_int_refHurtInfo_bool_bool_int_bool_float_float_float orig, Player self, PlayerDeathReason damageSource, int Damage, int hitDirection, out Player.HurtInfo info, bool pvp, bool quiet, int cooldownCounter, bool dodgeable, float armorPenetration, float scalingArmorPenetration, float knockback)
    {
        TalismanRoll(self, ref Damage);

        DiceReroll(self, ref Damage);

        LuckRoll = (0, 0);

        return orig(self, damageSource, Damage, hitDirection, out info, pvp, quiet, cooldownCounter, dodgeable, armorPenetration, scalingArmorPenetration, knockback);
    }

    private int RegisterLuckRoll(On_Main.orig_DamageVar_float_int_float orig, float dmg, int percent, float luck)
    {
        int result = orig(dmg, percent, luck);

        LuckRoll.original = dmg;
        LuckRoll.rolled = result;

        return result;
    }
}