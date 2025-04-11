using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace TheBindingOfRarria.Common.Config;

public class ClientConfig : ModConfig
{
        // If you have time, steal either mine or scalars system for live updating dragable config.

    public override ConfigScope Mode => ConfigScope.ClientSide;

    [DefaultValue(false)]
    public bool SanguineGiftUIDraggable;
    
    [DefaultValue(typeof(Vector2), "0.78125, 0.13888")]
    public Vector2 SanguineGiftUIPosition;

    [DefaultValue(typeof(Color), "0, 150, 74, 150")]
    public Color SanguineGiftUIColor;
}