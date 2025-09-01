using System;

namespace TheBindingOfRarria.Common.Graphics.Particles;

[Flags]
public enum ParticleTextureType
{
    FadedGlowyBall = 0,
    BrightGlowyBall = 1 << 0,
}
