using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Common.Systems;

public class LifeBonus
{
    public int Amount;
    public int TimeLeft;
    public Predicate<Player> ErasureCondition;

    public LifeBonus(int amount, int duration, Predicate<Player> erasureCondition)
    {
        Amount = amount;
        TimeLeft = duration;
        ErasureCondition = erasureCondition;
    }
    public void UpdateLifeBonus(ref int life)
    {
        TimeLeft--;
        life += Amount;
    }
}

public class TemporaryLifePlayer : ModPlayer
{
    public List<LifeBonus> bonuses = [];

    public override void PostUpdateEquips()
    {
        for (int e = 0; e < bonuses.Count - 1; e++)
        {
            if (bonuses[e].ErasureCondition.Invoke(Player) || bonuses[e].TimeLeft <= 0)
                bonuses.Remove(bonuses[e]);

            else bonuses[e].UpdateLifeBonus(ref Player.statLifeMax2);
        }
    }
}