using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using TheBindingOfRarria.Common.Helpers;

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
        var p = player.GetModPlayer<NecromanPlayer>();
        p.Necroman = true;

        if (p.SummonedMinions.Count == 0)
        {
            var summoned = Array.FindAll(Main.projectile, proj => proj.active && proj.friendly && proj.minion && proj.owner == player.whoAmI).ToList();

            foreach (var item in player.inventory)
            {
                if (item.DamageType == DamageClass.Summon && item.buffType != 0 && !player.HasBuff(item.buffType) && !summoned.Any(proj => proj.type == item.shoot))
                {
                    p.SummonedMinions.Add(item.shoot);
                    player.AddBuff(item.buffType, 2);

                    if (Main.myPlayer == player.whoAmI)
                    {
                        player.maxMinions += 3;
                        var projectile = Projectile.NewProjectileDirect(player.GetSource_ItemUse(item, "Necromancer's Tome summon"), player.Center, player.velocity, item.shoot, item.damage, item.knockBack);
                        projectile.originalDamage = item.damage;
                    }
                    if (p.SummonedMinions.Count == 2)
                        break;
                }
            }
        }
    }
}

public class NecromanPlayer : ModPlayer
{
    public HashSet<int> SummonedMinions = [];

    public bool Necroman = false;

    public override void ResetEffects()
    {
        if (!Necroman)
        {
            foreach (var minion in SummonedMinions)
            {
                if (Player.OwnsProjectile(minion))
                    Array.Find(Main.projectile, p => p.active && p.type == minion && p.owner == Player.whoAmI).Kill();
                
                SummonedMinions.Remove(minion);
            }
        }

        Necroman = false;
    }
}

public class NecroProjectileGlobal : GlobalProjectile
{
    public override bool InstancePerEntity => true;

    public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => entity.minion;
    public override void OnSpawn(Projectile projectile, IEntitySource source)
    {
        if (source != null && source.Context != null && source.Context == "Necromancer's Tome summon")
        {
            Necro = true;
        }
    }

    public bool Necro = false;

    public float oldSlot = 0;

    public override void SendExtraAI(Projectile projectile, BitWriter bitWriter, BinaryWriter binaryWriter)
    {
        bitWriter.WriteBit(Necro);
    }

    public override void ReceiveExtraAI(Projectile projectile, BitReader bitReader, BinaryReader binaryReader)
    {
        var flag = bitReader.ReadBit();
        if (!Necro)
            Necro = flag;
    }

    public override bool PreDraw(Projectile projectile, ref Color lightColor)
    {
        if (Necro)
            lightColor = lightColor.MultiplyRGBA(Color.Gray);

        return base.PreDraw(projectile, ref lightColor);
    }

    public override bool PreAI(Projectile projectile)
    {
        if (!Main.player[projectile.owner].GetModPlayer<NecromanPlayer>().Necroman)
            Necro = false;

        if (oldSlot == 0)
            oldSlot = projectile.minionSlots;

        if (Necro)
            projectile.minionSlots = 0;

        else if (oldSlot != 0)
            projectile.minionSlots = oldSlot;

        
        return base.PreAI(projectile);
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