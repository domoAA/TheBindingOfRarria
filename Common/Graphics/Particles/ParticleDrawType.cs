using System;

namespace TheBindingOfRarria.Common.Graphics.Particles;

[Flags]
public enum ParticleDrawType
{
    Additive = 0,
    AlphaBlend = 1 << 0,
    PixelatedAdditive = 1 << 1,
    PixelatedAlphaBlend = 1 << 2,
}
