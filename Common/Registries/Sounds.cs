using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Common.Registries;

public class Sounds : ModSystem
{
    private const string prefix = "TheBindingOfRarria/Assets/Sounds/";

    public static SoundStyle AdaptedSound { get; internal set; }
    public static SoundStyle WheelCreak { get; internal set; }
    public static SoundStyle[] DespairTrigger { get; internal set; }

    public override void Load()
    {
        if (Main.dedServ)
            return;

        AdaptedSound = new(prefix + "ModifiedMahoragaWheel");
        WheelCreak = new(prefix + "ModifiedMahoragaWheelCreak");
        DespairTrigger = [new SoundStyle(prefix + "DespairTrigger0"), new SoundStyle(prefix + "DespairTrigger1")];
    }
}