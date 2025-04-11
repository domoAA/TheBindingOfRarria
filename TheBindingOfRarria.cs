using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using System.IO;
using TheBindingOfRarria.Content.Projectiles;
using TheBindingOfRarria.Common;
using TheBindingOfRarria.Content.Items;

namespace TheBindingOfRarria;

public class TheBindingOfRarria : Mod
{
    public static List<int> FishID = [];
    public static Dictionary<int, Asset<Texture2D>> FishTextures = [];

    public static SoundStyle AdaptedSound = new SoundStyle("TheBindingOfRarria/Common/Assets/ModifiedMahoragaWheel");
    public static SoundStyle WheelCreak = new SoundStyle("TheBindingOfRarria/Common/Assets/ModifiedMahoragaWheelCreak");
    public override void Load()
    {
        if (Main.netMode != NetmodeID.Server)
        {
            
            // Fish texture List
            for (int i = 2297; i <= 2321; i++)
            {
                FishID.Add(i);
            }
            for (int j = 2450; j <= 2488; j++)
            {
                FishID.Add(j);
            }
            FishID.Add(2290);
            FishID.Add(4401);
            FishID.Add(4402);

            foreach (var fish in FishID)
            {
                FishTextures.Add(fish, TextureAssets.Item[fish]);
            }
        }
    }

    public override void Unload()
    {
        if (Main.netMode != NetmodeID.Server)
        {
            FishID = null;
            FishTextures = null;
        }
    }

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

            if (entityType)
            {
                foreach (Projectile p in Main.ActiveProjectiles)
                {
                    if (p.identity == id)
                    {
                        p.GetGlobalProjectile<SlowedGlobalProjectile>().Slowed = ((State)slow, duration);
                    }
                }
            }
            else
            {
                foreach (NPC n in Main.ActiveNPCs)
                {
                    if (n.whoAmI == id)
                    {
                        n.GetGlobalNPC<NPCExtensions.SlowedGlobalNPC>().Slowed = ((State)slow, duration);
                    }
                }
            }
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
