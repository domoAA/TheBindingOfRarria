namespace TheBindingOfRarria.Content.Items;

public class BerserkersPauldron : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 26;
        Item.height = 28;
        Item.value = Item.sellPrice(gold: 2);
        Item.rare = ItemRarityID.Pink;
        Item.accessory = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<BerserkersPauldronPlayer>().Active = true;
    }
}

public class BerserkersPauldronPlayer : ModPlayer
{
    private const int EffectTimeframeMax = 5 * 60;
    private const int NumEnemiesToKillForEffect = 3;
    private const int BerserkBuffTime = 6 * 60;

    public bool Active;

    private int _effectTimeFrame;
    private int _killedEnemiesCount;

    public override void ResetEffects()
    {
        Active = false;
    }

    public override void PostUpdateEquips()
    {
        _effectTimeFrame--;
        if (_effectTimeFrame <= 0)
        {
            _killedEnemiesCount = 0;
        }
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (target.active || !Active)
        {
            return;
        }

        _effectTimeFrame = EffectTimeframeMax;
        _killedEnemiesCount++;

        if (_killedEnemiesCount >= NumEnemiesToKillForEffect)
        {
            _effectTimeFrame = 0;
            _killedEnemiesCount = 0;
            Player.AddBuff(ModContent.BuffType<BerserkersPauldronBuff>(), BerserkBuffTime);
        }
    }
}
public class BerserkersPauldronDropRule : GlobalNPC
{
    public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
    {
        return NPCID.Sets.BelongsToInvasionGoblinArmy[entity.type];
    }

    public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
    {
        int dropChanceDenominator = npc.type == NPCID.GoblinSummoner ? 25 : 200;
        npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsHardmode(), ModContent.ItemType<BerserkersPauldron>(), dropChanceDenominator));
    }
}