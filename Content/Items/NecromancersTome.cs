using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

public class NecromancersTome : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.height = 28;
        Item.width = 28;
        Item.value = Item.buyPrice(0, 2, 68);
        Item.rare = ItemRarityID.Expert;
        Item.expert = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<NecromancersTomePlayer>().HasNecromancersTome = true;

        ref var minions = ref player.GetModPlayer<NecromancersTomePlayer>().SpawnedMinions;

        if (minions.Count < 1)
        {
            foreach (var item in player.inventory)
            {
                if (item.DamageType == DamageClass.Summon && item.buffType != 0 && !player.HasBuff(item.buffType) && !minions.Contains(item))
                {
                    player.maxMinions += 1;
                    minions.Add(item);
                    player.AddBuff(item.buffType, 2);

                    if (Main.myPlayer == player.whoAmI)
                    {
                        var projectile = Projectile.NewProjectileDirect(player.GetSource_ItemUse(item, "Necromancers tome accessory"), player.Center, player.velocity, item.shoot, item.damage, item.knockBack);
                        projectile.originalDamage = item.damage;
                    }
                    if (minions.Count == 2)
                        break;
                }
            }
        }
        else
        {
            foreach (Item item in minions)
            {
                player.maxMinions += 1;
                if (!player.HasBuff(item.buffType))
                {
                    player.AddBuff(item.buffType, 2);

                    if (Main.myPlayer == player.whoAmI)
                    {
                        var projectile = Projectile.NewProjectileDirect(player.GetSource_ItemUse(item, "Necromancers tome accessory"), player.Center, player.velocity, item.shoot, item.damage, item.knockBack);
                        projectile.originalDamage = item.damage;
                    }
                }
            }
        }
    }
}

public class NecromancersTomePlayer : ModPlayer
{
    public bool HasNecromancersTome = false;
    public List<Item> SpawnedMinions = [];

    public override void ResetEffects()
    {
        if (!HasNecromancersTome)
        {
            foreach (Item item in SpawnedMinions)
            {
                if (Player.HasBuff(item.buffType))
                    Player.ClearBuff(item.buffType);
            }
            SpawnedMinions.Clear();
        }

        HasNecromancersTome = false;
    }
}

public class NecromancerDropNPC : GlobalNPC
{
    public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
    {
        if (npc.type == NPCID.Necromancer || npc.type == NPCID.NecromancerArmored)
            npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsExpert(), ModContent.ItemType<NecromancersTome>(), 7));

        base.ModifyNPCLoot(npc, npcLoot);
    }
}