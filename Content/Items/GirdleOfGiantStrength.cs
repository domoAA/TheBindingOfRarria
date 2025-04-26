using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Common.Helpers;

namespace TheBindingOfRarria.Content.Items;

public class GirdleOfGiantStrength : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.DefaultToAccessory(26, 24);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        GirdleOfGiantStrengthPlayer girdleOfGiantStrengthPlayer = player.GetModPlayer<GirdleOfGiantStrengthPlayer>();
        girdleOfGiantStrengthPlayer.HasGirdleOfGiantStrength = true;
        var activeBonuses = girdleOfGiantStrengthPlayer.activeBonuses;
        for (int i = activeBonuses.Count - 1; i >= 0; i--)
        {
            activeBonuses[i].TimeLeft--;
            player.statLifeMax2 += activeBonuses[i].Amount;

            if (activeBonuses[i].TimeLeft <= 0)
            {
                player.statLifeMax2 -= activeBonuses[i].Amount;
                activeBonuses.RemoveAt(i);
            }
        }
    }
}
public class GirdleOfGiantStrengthPlayer : ModPlayer
{
    public bool HasGirdleOfGiantStrength = false;
    public readonly List<LifeBonus> activeBonuses = [];

    public class LifeBonus
    {
        public int Amount;
        public int TimeLeft;

        public LifeBonus(int amount, int duration)
        {
            Amount = amount;
            TimeLeft = duration;
        }
    }

    public override void Load()
    {
        base.Load();
        Terraria.On_Player.Heal += On_Player_Heal;
    }

    private void On_Player_Heal(On_Player.orig_Heal orig, Player self, int amount)
    {
        orig(self, amount);
        if (self.GetModPlayer<GirdleOfGiantStrengthPlayer>().HasGirdleOfGiantStrength)
        {
            int bonus = amount / 2;
            if (bonus > 0)
            {
                self.statLifeMax2 += bonus;
                self.GetModPlayer<GirdleOfGiantStrengthPlayer>().activeBonuses.Add(new LifeBonus(bonus, 300));
            }
        }
    }


    public override void ResetEffects()
    {
        HasGirdleOfGiantStrength = false;
    }
}