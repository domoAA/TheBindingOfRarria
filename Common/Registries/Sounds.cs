using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Common.Registries;

public class Sounds : ModSystem
{
    private const string prefix = "TheBindingOfRarria/Assets/Sounds/";

    public static SoundStyle AdaptedSound { get; internal set; }

    public static SoundStyle WheelCreak { get; internal set; }

    public static SoundStyle Bell { get; internal set; }

    public static SoundStyle[] DespairTrigger { get; internal set; }

    public override void Load()
    {
        if (Main.dedServ)
            return;

        Bell = new(prefix + "BellSound");
        AdaptedSound = new(prefix + "ModifiedMahoragaWheel");
        WheelCreak = new(prefix + "ModifiedMahoragaWheelCreak");
        DespairTrigger = [new SoundStyle(prefix + "DespairTrigger0"), new SoundStyle(prefix + "DespairTrigger1")];

        //DespairTrigger = new SoundStyle(prefix + "DespairTrigger") with {Variants = [0, 1]}; is another way,..,,.
    }
}