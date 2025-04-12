using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items;

public class AdamantineTalisman : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.height = 30;
        Item.width = 26;
        Item.rare = ItemRarityID.LightRed;
        Item.value = Item.buyPrice(0, 4, 0, 4);
    }

    public override void UpdateAccessory(Player player, bool hideVisual) => player.GetModPlayer<NewRollPlayer>().Talisman = true;
    
    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ModContent.ItemType<WoodenDice>())
            .AddIngredient(ItemID.WhitePearl)
            .AddIngredient(ItemID.AdamantiteOre, 20)
            .AddIngredient(ModContent.ItemType<PaleOre>(), 20)
            .AddIngredient(ItemID.Diamond, 10)
            .AddTile(TileID.AdamantiteForge)
            .Register();
    }
}

        // Scratch that,,.. why the HELL is this partial.
    // Rewrite this base class, and place it elsewhere.
public partial class NewRollPlayer : ModPlayer
{
    public bool Talisman = false;

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

    public static void TalismanRoll(Player self, ref int Damage)
    {
        if (LuckRoll.rolled != 0 && Damage % LuckRoll.rolled == 0 && self.GetModPlayer<NewRollPlayer>().Talisman)
        {
            Damage = CustomRangeDamageVar(Damage, 100 - Main.DefaultDamageVariationPercent, 100, -self.luck);
            LuckRoll.rolled = Damage;
        }
    }

    private double UseLowLuckRoll(On_Player.orig_Hurt_PlayerDeathReason_int_int_refHurtInfo_bool_bool_int_bool_float_float_float orig, Player self, PlayerDeathReason damageSource, int Damage, int hitDirection, out Player.HurtInfo info, bool pvp, bool quiet, int cooldownCounter, bool dodgeable, float armorPenetration, float scalingArmorPenetration, float knockback)
    {
        TalismanRoll(self, ref Damage);

        DiceyPlayer.DiceReroll(self, ref Damage);

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