namespace TheBindingOfRarria.Common.Systems;

public class HPDecreaseSystem : ModSystem
{
    public static int HPDecrease = 0;

    public static bool Virus = false;

    public static bool Basillisk = false;

    public static bool Occult = false;

    public override void PreUpdateEntities()
    {
        HPDecrease = 0;

        if (Virus) HPDecrease += 5;
        if (Basillisk) HPDecrease += 10;
        if (Occult) HPDecrease += 7;

        Virus = false;
        Basillisk = false;
        Occult = false;
    }
}

public class HPDecreaseNPC : GlobalNPC
{
    public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
    {
        return entity.CanBeChasedBy();
    }

    public override void ApplyDifficultyAndPlayerScaling(NPC npc, int numPlayers, float balance, float bossAdjustment)
    {
        npc.lifeMax = (int)(npc.lifeMax * (1f - HPDecreaseSystem.HPDecrease / 100f));
    }
}