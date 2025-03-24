
using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace TheBindingOfRarria.Common.Config
{
    public class KeybindSystem : ModSystem
    {
        //public static ModKeybind StonedKey { get; private set; }
        public static ModKeybind ZaWardoKey { get; private set; }
        public static ModKeybind LightBeamKey { get; private set; }
        public override void Load()
        {
            //StonedKey = KeybindLoader.RegisterKeybind(Mod, "StonedInvul", Microsoft.Xna.Framework.Input.Keys.LeftShift);
            ZaWardoKey = KeybindLoader.RegisterKeybind(Mod, "BrokenWatch", Microsoft.Xna.Framework.Input.Keys.P);
            LightBeamKey = KeybindLoader.RegisterKeybind(Mod, "Revelation", Microsoft.Xna.Framework.Input.Keys.L);
            base.Load();
        }
        public override void Unload()
        {
            //StonedKey = null;
            ZaWardoKey = null;
            LightBeamKey = null;
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