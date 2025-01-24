using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using TheBindingOfRarria.Content.Projectiles;

namespace TheBindingOfRarria.Content.Items
{
    public class GolemsHeart : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 24;
            Item.accessory = true;
            Item.defense = 2;
            Item.rare = ItemRarityID.Expert;
            Item.value = Item.buyPrice(0, 6, 66);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.SpawnProjectileIfNotSpawned(ModContent.ProjectileType<RepellingPulse>(), player.GetSource_Accessory(Item));
        }
        public override int ChoosePrefix(UnifiedRandom rand)
        {
            var chance = rand.NextFloat() / 2;
            if (chance < 0.1f)
                return PrefixID.Warding;
            else if (chance < 0.2f)
                return PrefixID.Armored;
            else if (chance < 0.3f)
                return PrefixID.Hard;
            else 
                return base.ChoosePrefix(rand);
        }
    }
    public class PulsingGolem : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            return entity.type == NPCID.GraniteGolem;
        }
        public Projectile HeartBeat = null;
        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            if (!Main.expertMode || Main.netMode == NetmodeID.MultiplayerClient)
                return;

            HeartBeat = Main.projectile[Projectile.NewProjectile(npc.GetSource_FromThis(), npc.Center, Vector2.Zero, ModContent.ProjectileType<RepellingPulse>(), 0, 0, Main.myPlayer)];
            HeartBeat.hostile = true;

            base.OnSpawn(npc, source);
        }
        public override void PostAI(NPC npc)
        {
            if (HeartBeat != null && HeartBeat.active)
            {
                HeartBeat.timeLeft = 3;
                HeartBeat.Center = npc.Center;
                if (npc.ai[2] < 0)
                {
                    npc.reflectsProjectiles = true;
                    npc.ReflectProjectiles(npc.getRect());
                    HeartBeat.ai[0] *= 0.96f;
                }
                else
                    npc.reflectsProjectiles = false;
            }
        }
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.GraniteGolem)
            {
                npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsExpert(), ModContent.ItemType<GolemsHeart>(), 100)); // apparently mm drop??? bestiary
            }
            base.ModifyNPCLoot(npc, npcLoot);
        }
    }
}