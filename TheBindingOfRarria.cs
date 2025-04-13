using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using System.IO;
using TheBindingOfRarria.Content.Projectiles;
using TheBindingOfRarria.Content.Items;
using static TheBindingOfRarria.Common.Helpers.Helper;

namespace TheBindingOfRarria;

public class TheBindingOfRarria : Mod
{
        // fine
    public enum PacketTypes : int
    {
        ProjectileReflect,
        EntitySlow,
        DustSpawn,
        Default
    }

        // move to other file
    public enum State
    {
        Default,
        Slow,
        Fast
    }

        // rewrite this mess
    public override void HandlePacket(BinaryReader reader, int whoAmI)
    {
        var type = reader.ReadInt32();
        if (type == (int)PacketTypes.ProjectileReflect)
        {
            int id = reader.ReadInt32();

            foreach (var proj in Main.ActiveProjectiles)
            {
                if (proj.identity == id)
                    proj.GetReflected();
            }
            if (Main.netMode == NetmodeID.Server)
            {
                ModPacket packet = GetPacket();
                packet.Write(type);
                packet.Write(id);
                packet.Send();
            }
        }
        else if (type == (int)PacketTypes.EntitySlow)
        {
            int slow = reader.ReadInt32();
            int duration = reader.ReadInt32();
            bool entityType = reader.ReadBoolean();
            int id = reader.ReadInt32();

            if (Main.netMode == NetmodeID.Server)
            {
                ModPacket packet = GetPacket();
                packet.Write(type);
                packet.Write(slow);
                packet.Write(duration);
                packet.Write(entityType);
                packet.Write(id);
                packet.Send();
            }

                // guh
            if (entityType)
            {
                foreach (Projectile p in Main.ActiveProjectiles)
                    if (p.identity == id)
                        p.GetGlobalProjectile<SlowedGlobalProjectile>().Slowed = ((State)slow, duration);
            }
            else
                foreach (NPC n in Main.ActiveNPCs)
                    if (n.whoAmI == id)
                        n.GetGlobalNPC<SlowedGlobalNPC>().Slowed = ((State)slow, duration);
            return;
        }
        else if (type == (int)PacketTypes.DustSpawn)
        {
            Vector2 position = reader.ReadVector2();
            Vector2 direction = reader.ReadVector2();
            if (Main.netMode == NetmodeID.Server)
            {
                ModPacket packet = GetPacket();
                packet.Write(type);
                packet.WriteVector2(position);
                packet.WriteVector2(direction);
                packet.Send();
            }
            else
            {
                NatureDodgePlayer natureplayer = Main.LocalPlayer.GetModPlayer<NatureDodgePlayer>();

                natureplayer.blocked = true;
                natureplayer.position = position;
                natureplayer.direction = direction;
            }
            return;
        }
    }
}
