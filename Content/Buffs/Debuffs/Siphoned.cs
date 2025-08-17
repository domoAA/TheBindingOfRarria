using Terraria;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Buffs.Debuffs;

public class Siphoned : ModBuff
{
    public override string Texture => ContentPath + "Buffs/Debuffs/" + Name;

    public override void SetStaticDefaults() => Main.debuff[Type] = true;
}