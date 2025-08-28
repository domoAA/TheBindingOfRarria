using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Common.Helpers;
using TheBindingOfRarria.Content.Projectiles;

namespace TheBindingOfRarria.Content.Items;

public class KingdomStone : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.width = 28;
        Item.height = 28;
        Item.rare = ItemRarityID.Expert;
        Item.value = Item.sellPrice(0, 2);
        Item.expert = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.statManaMax2 += 60;
        player.SpawnProjectileIfNotSpawned(ModContent.ProjectileType<MagicSphere>(), player.GetProjectileSource_Accessory(Item));
    }
}

public class ManaStoneDropNPC : GlobalNPC
{
    public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => entity.aiStyle == NPCAIStyleID.Caster;

    public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
    {
        if (npc.type == NPCID.DarkCaster || npc.type == NPCID.GoblinSorcerer || npc.type == NPCID.Tim)
            npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsExpert(), ModContent.ItemType<KingdomStone>(), npc.type == NPCID.Tim ? 3 : 30));

        base.ModifyNPCLoot(npc, npcLoot);
    }
}

public class ManaShieldPLayer : ModPlayer
{
    public float power = 0;

    public override void PostUpdateEquips()
    {
        power = (Player.statManaMax2 - Player.statMana) / (float)Player.statManaMax2;
    }

    public override void ModifyHurt(ref Player.HurtModifiers modifiers)
    {
        if (Player.OwnsProjectile(ModContent.ProjectileType<MagicSphere>()))
        {
            modifiers.FinalDamage *= (1 - 0.2f * power);
        }
    }
}
