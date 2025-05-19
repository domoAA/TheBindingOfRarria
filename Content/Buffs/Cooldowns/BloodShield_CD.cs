using Terraria.Localization;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Buffs;

public class BloodShield_CD : ModBuff
{
    public override string Texture => ContentPath + "Buffs/Cooldowns/" + Name;
    public override LocalizedText Description => Language.GetText("Mods.TheBindingOfRarria.Buffs.Cooldown.Description");

}