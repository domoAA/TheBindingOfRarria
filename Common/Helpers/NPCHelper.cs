using System.Collections.Generic;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;

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
                npc.velocity /= 0.7f; 
                counter++;
            }
            else if (Slowed.Item1 == TheBindingOfRarria.State.Fast)
                npc.velocity *= 0.7f;

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
                npc.velocity *= 0.7f;
            else if (Slowed.Item1 == TheBindingOfRarria.State.Fast)
                npc.velocity /= 0.7f;
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

    public static NPC[] FindClosestNPCs(Vector2 searchPos, float maxDistance, int numTargets, Func<NPC, bool> priority, Func<NPC, bool> condition)
    {
        var targets = new SortedSet<KeyValuePair<float, NPC>>(Comparer<KeyValuePair<float, NPC>>.Create((dist, npc) => dist.Key.CompareTo(npc.Key)));
        float dist = maxDistance * maxDistance;

        for (int i = 0; i < Main.maxNPCs; i++)
        {
            NPC target = Main.npc[i];

            if (target == null || !target.active)
                continue;

            if (target.CanBeChasedBy())
            {
                float length = Vector2.DistanceSquared(target.Center, searchPos);

                if (length < dist && condition(target))
                {
                    if (priority(target))
                        targets.Add(new KeyValuePair<float, NPC>(length, target));
                    else targets.Add(new KeyValuePair<float, NPC>(length, target));

                    if (targets.Count > numTargets)
                        targets.Remove(targets.Last());
                }
            }
        }

        if (targets.Count == 0)
            return null;

        NPC[] closestTargets = targets.Select(value => value.Value).ToArray();

        return closestTargets.Length > 0 ? closestTargets : null;
    }

    public static int GetTargetIndex(Vector2 search, float distance = 200f)
    {
        int index = -1;

        if (FindClosestNPCs(search, distance, 1, priority: (NPC n) => n.boss, (NPC n) => true) != null)
        {
            foreach (NPC npc in FindClosestNPCs(search, distance, 1, priority: (NPC n) => n.boss, (NPC n) => true))
            {
                if (npc != null && npc.CanBeChasedBy())
                {
                    index = npc.whoAmI;
                    break;
                }
            }
        }

        return index;
    }

    public static int[] GetTargetIndices(Vector2 search, int count, float distance = 200f)
    {
        List<int> indices = new(count);

        if (FindClosestNPCs(search, distance, count, priority: (NPC n) => n.boss, (NPC n) => true) != null)
        {
            foreach (NPC npc in FindClosestNPCs(search, distance, count, priority: (NPC n) => n.boss, (NPC n) => true))
            {
                if (npc != null && npc.CanBeChasedBy())
                {
                    if (indices.Count >= count)
                        break;

                    indices.Add(npc.whoAmI);
                }
            }
        }

        return [..indices];
    }
}
