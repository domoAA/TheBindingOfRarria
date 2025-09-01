
using TheBindingOfRarria.Common.Graphics.Particles;

namespace TheBindingOfRarria.Common.Graphics.Particles;

public struct ParticleData
{
    public Vector2 Position;
    public Vector2 Velocity;
    public Vector2 Squish;
    public float LifeTimer;
    public float MaxTime;
    public float Scale;
    public float Rotation;
    public float Opacity;
    public Color DrawColor;
    public float[] ai;

    public ParticleFadeData fadeData;

    public ParticleBehaviour ParticleBehaviour;
    public ParticleDrawType ParticleDrawType;
    public ParticleTextureType ParticleTextureType;

    public readonly bool Active => LifeTimer < MaxTime;
}
