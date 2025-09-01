using System;

namespace TheBindingOfRarria.Common.Graphics.Particles;

/// <summary>
/// Default: uses ai[0] for velocity multiplication
/// </summary>
[Flags]
public enum ParticleBehaviour
{
    Default = 0, //just move, has controllable speed (ai[0] is the modifier)
    later = 1 << 0,
}

