
using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace TheBindingOfRarria.Common.Config
{
    public class KeybindSystem : ModSystem
    {
        //public static ModKeybind StonedKey { get; private set; }
        public static ModKeybind ZaWardoKey { get; private set; }
        public static ModKeybind LightBeamKey { get; private set; }
        public static ModKeybind BloodDripKey { get; private set; }
        public static ModKeybind AbsorbingKey { get; private set; }
        public static ModKeybind CrystalDashKey { get; private set; }
        public override void Load()
        {
            //StonedKey = KeybindLoader.RegisterKeybind(Mod, "StonedInvul", Keys.LeftShift);
            ZaWardoKey = KeybindLoader.RegisterKeybind(Mod, "BrokenWatch", Keys.P);
            LightBeamKey = KeybindLoader.RegisterKeybind(Mod, "Revelation", Keys.L);
            BloodDripKey = KeybindLoader.RegisterKeybind(Mod, "CursedBlood", Keys.K);
            AbsorbingKey = KeybindLoader.RegisterKeybind(Mod, "CursedBlood", Keys.O);
            CrystalDashKey = KeybindLoader.RegisterKeybind(Mod, "CursedBlood", Keys.V);

            base.Load();
        }
        public override void Unload()
        {
            //StonedKey = null;
            ZaWardoKey = null;
            LightBeamKey = null;
            BloodDripKey = null;
            AbsorbingKey = null;
            CrystalDashKey = null;
            base.Unload();
        }
    }
    public class ClientConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;
        [DefaultValue(false)]
        public bool SanguineGiftUIDraggable; 
        [DefaultValue(typeof(Vector2), "0.78125, 0.13888")]
        public Vector2 SanguineGiftUIPosition;
        [DefaultValue(typeof(Color), "0, 150, 74, 150")]
        public Color SanguineGiftUIColor;
    }
}