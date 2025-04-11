
using System;
using Terraria.ID;

namespace TheBindingOfRarria;

public class TheBindingOfRarria : Mod
{
    public static Asset<Texture2D> BeamBody;
    public static Asset<Texture2D> BeamEnd;
    public static Dictionary<string, Asset<Texture2D>> BloodStorage = [];

    public static List<int> FishID = [];
    public static Dictionary<int, Asset<Texture2D>> FishTextures = [];

    public static SoundStyle AdaptedSound = new SoundStyle("TheBindingOfRarria/Common/Assets/ModifiedMahoragaWheel");
    public static SoundStyle WheelCreak = new SoundStyle("TheBindingOfRarria/Common/Assets/ModifiedMahoragaWheelCreak");
    public override void Load()
    {
        if (Main.netMode != NetmodeID.Server)
        {
            BloodStorage["orb"] = (ModContent.Request<Texture2D>("TheBindingOfRarria/Common/Assets/BloodOrb"));
            BloodStorage["orbsmol"] = (ModContent.Request<Texture2D>("TheBindingOfRarria/Common/Assets/BloodOrbSmol"));
            BloodStorage["cd"] = (ModContent.Request<Texture2D>("TheBindingOfRarria/Common/Assets/CDTex"));
            BloodStorage["cdfiller"] = (ModContent.Request<Texture2D>("TheBindingOfRarria/Common/Assets/CDFiller"));

            BeamEnd = ModContent.Request<Texture2D>("TheBindingOfRarria/Common/Assets/BeamEnd");
            BeamBody = ModContent.Request<Texture2D>("TheBindingOfRarria/Common/Assets/BeamBody");


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
    public override void PostSetupContent()
    {
        //foreach (var fish in FishID)
        //{
            //FishTextures.Add(TextureAssets.Item[fish]);
        //}
    }
    public override void Unload()
    {
        if (Main.netMode != NetmodeID.Server)
        {
            BloodStorage = null;
            BeamEnd = null;
            BeamBody = null;

            FishID = null;
            FishTextures = null;
        }
    }
    public enum PacketTypes
    {
        ProjectileReflect,
        EntitySlow,
        DustSpawn,
        Default
    }
    public enum State
    {
        Default,
        Slow,
        Fast
    }
    public override void HandlePacket(BinaryReader reader, int whoAmI)
    {
        var type = reader.ReadInt32();
        if (type == ((int)PacketTypes.ProjectileReflect))
        {
            var id = reader.ReadInt32();

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
        else if (type == ((int)PacketTypes.EntitySlow))
        {
            var slow = reader.ReadInt32();
            var duration = reader.ReadInt32();
            var entityType = reader.ReadBoolean();
            var id = reader.ReadInt32();

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
                foreach (var proj in Main.ActiveProjectiles)
                {
                    if (proj.identity == id)
                    {
                        proj.GetGlobalProjectile<SlowedGlobalProjectile>().Slowed = ((State)slow, duration);
                    }
                }
            }
            else
            {
                foreach (var npc in Main.ActiveNPCs)
                {
                    if (npc.whoAmI == id)
                    {
                        npc.GetGlobalNPC<NPCExtensions.SlowedGlobalNPC>().Slowed = ((State)slow, duration);
                    }
                }
            }
            return;
        }
        else if (type == ((int)PacketTypes.DustSpawn))
        {
            var position = reader.ReadVector2();
            var direction = reader.ReadVector2();
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
                Main.LocalPlayer.GetModPlayer<NatureDodgePlayer>().blocked = true;
                Main.LocalPlayer.GetModPlayer<NatureDodgePlayer>().position = position;
                Main.LocalPlayer.GetModPlayer<NatureDodgePlayer>().direction = direction;
            }
            return;
        }
    }
}
