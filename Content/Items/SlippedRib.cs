

namespace TheBindingOfRarria.Content.Items
{
    public class SlippedRib : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 24;
            Item.accessory = true;
            Item.rare = ItemRarityID.Expert;
            Item.value = Item.buyPrice(0, 2);
            Item.expert = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual) => player.SpawnProjectileIfNotSpawned(ModContent.ProjectileType<ReflectiveRib>(), player.GetSource_Accessory(Item), new Microsoft.Xna.Framework.Vector2(player.Center.X, player.Center.Y - 40));
        
    }
    public class BoneLootNPC : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.UndeadMiner || npc.type == NPCID.Skeleton || npc.type == NPCID.SporeSkeleton || npc.type == NPCID.SmallMisassembledSkeleton || npc.type == NPCID.BigMisassembledSkeleton || npc.type == NPCID.MisassembledSkeleton || npc.type == NPCID.PantlessSkeleton || npc.type == NPCID.SmallPantlessSkeleton || npc.type == NPCID.BigPantlessSkeleton || npc.type == NPCID.SkeletonTopHat || npc.type == NPCID.HeadacheSkeleton || npc.type == NPCID.SmallHeadacheSkeleton || npc.type == NPCID.BigHeadacheSkeleton || npc.type == NPCID.SkeletonAlien || npc.type == NPCID.SkeletonAstonaut || npc.type == NPCID.BigSkeleton || npc.type == NPCID.SmallSkeleton || npc.type == NPCID.BoneThrowingSkeleton || npc.type == NPCID.AngryBonesBig || npc.type == NPCID.AngryBonesBigHelmet || npc.type == NPCID.AngryBonesBigMuscle || npc.type == NPCID.ShortBones || npc.type == NPCID.UndeadViking || npc.type == NPCID.AngryBones)
            {
                npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsExpert(), ModContent.ItemType<SlippedRib>(), 667));
            }
            else if (npc.type == NPCID.ArmoredViking || npc.type == NPCID.BoneLee || npc.type == NPCID.ArmoredSkeleton || npc.type == NPCID.SkeletonSniper || npc.type == NPCID.TacticalSkeleton || npc.type == NPCID.SkeletonCommando || npc.type == NPCID.SkeletonArcher)
            {
                npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsExpert(), ModContent.ItemType<SlippedRib>(), 167));
            }
            base.ModifyNPCLoot(npc, npcLoot);
        }
    }
}