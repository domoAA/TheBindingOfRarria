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
    public string Source;

    public LifeBonus(int amount, int duration, Predicate<Player> erasureCondition, string source)
    {
        Amount = amount;
        TimeLeft = duration;
        ErasureCondition = erasureCondition;
        Source = source;
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