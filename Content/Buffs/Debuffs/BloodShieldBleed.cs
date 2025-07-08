using Terraria;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Items;

namespace TheBindingOfRarria.Content.Buffs.Debuffs;

public class BloodShieldBleed : ModBuff
{
    public override string Texture => ContentPath + "Buffs/Debuffs/" + Name;

    public override void SetStaticDefaults() => Main.debuff[Type] = true;

    public override void Update(Player player, ref int buffIndex)
    {
        player.lifeRegen -= player.statLifeMax2 / 10;

        player.GetModPlayer<KamoPlayer>().Stored += player.statLifeMax2 * (1f / 7200);
        player.statDefense += (int)player.GetModPlayer<KamoPlayer>().Stored;
    }
}