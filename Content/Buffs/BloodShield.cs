namespace TheBindingOfRarria.Content.Buffs;

public class BloodShield : ModBuff
{
    public override void Update(Player player, ref int buffIndex)
    {
        player.statDefense += (int)player.GetModPlayer<KamoPlayer>().Stored;
    }
}
public class BloodShieldBleed : ModBuff
{
    public override void SetStaticDefaults()
    {
        Main.debuff[Type] = true;
    }
    public override void Update(Player player, ref int buffIndex)
    {
        player.lifeRegen -= player.statLifeMax2 / 10;
        
        player.GetModPlayer<KamoPlayer>().Stored += player.statLifeMax2 * (1f / 7200);
        player.statDefense += (int)player.GetModPlayer<KamoPlayer>().Stored;
    }
}