
namespace TheBindingOfRarria
{
    public class TheBindingOfRarria : Mod
    {
        public static Asset<Texture2D> BeamBody;
        public static Asset<Texture2D> BeamEnd;
        public static SoundStyle AdaptedSound = new SoundStyle("TheBindingOfRarria/Common/Assets/mahoragawheel");
        public static SoundStyle AdaptedSoundLoud = new SoundStyle("TheBindingOfRarria/Common/Assets/mahoragawheel (1)");
        public override void Load()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                BeamEnd = ModContent.Request<Texture2D>("TheBindingOfRarria/Common/Assets/BeamEnd");
                BeamBody = ModContent.Request<Texture2D>("TheBindingOfRarria/Common/Assets/BeamBody");
            }
        }
        public override void Unload()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                BeamEnd = null;
                BeamBody = null;
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
}
