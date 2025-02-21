using System.ComponentModel;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace TheBindingOfRarria.Common.Config
{
    public class ClientConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;
        [Header("Configuration")]
        [DefaultValue(false)]
        public bool FallingBLossomEmotionChanceInInventory;
    }
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