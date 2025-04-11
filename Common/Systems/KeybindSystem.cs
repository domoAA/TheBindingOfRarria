using Microsoft.Xna.Framework.Input;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Common.Config;

public class KeybindSystem : ModSystem
{
        // public static ModKeybind StonedKey { get; private set; }
    public static ModKeybind ZaWardoKey { get; private set; }
    public static ModKeybind LightBeamKey { get; private set; }
    public static ModKeybind BloodDripKey { get; private set; }
    public static ModKeybind AbsorbingKey { get; private set; }
    public static ModKeybind CrystalDashKey { get; private set; }

    public override void Load()
    {
            // StonedKey = KeybindLoader.RegisterKeybind(Mod, "StonedInvul", Keys.LeftShift);
        ZaWardoKey = KeybindLoader.RegisterKeybind(Mod, "BrokenWatch", Keys.P);
        LightBeamKey = KeybindLoader.RegisterKeybind(Mod, "Revelation", Keys.L);
        BloodDripKey = KeybindLoader.RegisterKeybind(Mod, "CursedBlood", Keys.K);
        AbsorbingKey = KeybindLoader.RegisterKeybind(Mod, "AbsorbingLiquid", Keys.O);
        CrystalDashKey = KeybindLoader.RegisterKeybind(Mod, "CrystalHeart", Keys.V);
    }

    public override void Unload()
    {
            // StonedKey = null;
        ZaWardoKey = null;
        LightBeamKey = null;
        BloodDripKey = null;
        AbsorbingKey = null;
        CrystalDashKey = null;
    }
}
