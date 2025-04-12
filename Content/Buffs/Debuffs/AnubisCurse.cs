using Terraria;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Buffs.Debuffs;

public class AnubisCurse : ModBuff
{
    public override string Texture => ContentPath + "Buffs/Debuffs/" + Name;

    public override void SetStaticDefaults() => Main.debuff[Type] = true;
}

public class MummyPlayer : ModPlayer
{
    public override void PostUpdate()
    {
        if (Player.HasBuff(ModContent.BuffType<AnubisCurse>()))
            Player.lifeRegen -= 2;
    }
}