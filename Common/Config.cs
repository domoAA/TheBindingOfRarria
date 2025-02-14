using Terraria.ModLoader;

namespace TheBindingOfRarria.Common.Config
{
    public class KeybindSystem : ModSystem
    {
        public static ModKeybind StonedKey { get; private set; }
        public static ModKeybind ZaWardoKey { get; private set; }
        public override void Load()
        {
            //StonedKey = KeybindLoader.RegisterKeybind(Mod, "StonedInvul", Microsoft.Xna.Framework.Input.Keys.LeftShift);
            ZaWardoKey = KeybindLoader.RegisterKeybind(Mod, "BrokenWatch", Microsoft.Xna.Framework.Input.Keys.P);
            base.Load();
        }
        public override void Unload()
        {
            //StonedKey = null;
            ZaWardoKey = null;
            base.Unload();
        }
    }
}