using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Common.Helpers;

public static partial class Helper
{
    public class SlowedGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public (TheBindingOfRarria.State, int) Slowed = (TheBindingOfRarria.State.Default, 0);

        public int counter = 0;

        public override bool PreAI(NPC npc)
        {
            if (Slowed.Item1 == TheBindingOfRarria.State.Slow) 
            { 
                npc.velocity *= 0.95f; 
                counter++;
            }
            else if (Slowed.Item1 == TheBindingOfRarria.State.Fast)
                npc.velocity *= 1.05f;

            if (counter >= 3)
            {
                counter = 0;
                return false;
            }
            return base.PreAI(npc);
        }

        public override void PostAI(NPC npc)
        {
            if (Slowed.Item1 == TheBindingOfRarria.State.Slow)
                npc.velocity /= 0.95f;
            else if (Slowed.Item1 == TheBindingOfRarria.State.Fast)
                npc.velocity /= 1.05f;
        }

        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (Slowed.Item1 == TheBindingOfRarria.State.Slow)
                drawColor.A = 100;

            Slowed.Item2--;
            if (Slowed.Item2 <= 0)
                Slowed.Item1 = TheBindingOfRarria.State.Default;

            base.DrawEffects(npc, ref drawColor);
        }
    }
    public static void GetSlowed(this NPC npc, TheBindingOfRarria.State state, int duration)
    {
        if (npc.GetGlobalNPC<SlowedGlobalNPC>().Slowed.Item1 == state)
            return;

        if (Main.netMode == NetmodeID.MultiplayerClient)
        {
            ModPacket packet = ModContent.GetInstance<TheBindingOfRarria>().GetPacket();
            packet.Write((int)TheBindingOfRarria.PacketTypes.EntitySlow);
            packet.Write((int)state);
            packet.Write(duration);
            packet.Write(false);
            packet.Write(npc.whoAmI);
            packet.Send();
        }
        else
        {
            npc.GetGlobalNPC<SlowedGlobalNPC>().Slowed = (state, duration);
        }
    }
}
