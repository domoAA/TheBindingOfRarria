using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace TheBindingOfRarria.Content.Projectiles;

public class ReflectableGlobalProjectile : GlobalProjectile
{
    public override bool InstancePerEntity => true;

    public float ReflectableSeed = 0;
    public bool Reflected = false;

    public override void OnSpawn(Projectile projectile, IEntitySource source)
    {
        if (Main.myPlayer == projectile.owner)
            ReflectableSeed = Main.rand.NextFloat();
    }

    public override void SendExtraAI(Projectile projectile, BitWriter bitWriter, BinaryWriter binaryWriter)
    {
        binaryWriter.Write(ReflectableSeed);
    }

    public override void ReceiveExtraAI(Projectile projectile, BitReader bitReader, BinaryReader binaryReader)
    {
        ReflectableSeed = binaryReader.ReadSingle();
    }

    public override void PostAI(Projectile projectile)
    {
        if (Reflected)
        {
            projectile.GetReflected();
            Reflected = false;
        }
    }
}
