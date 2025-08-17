using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Common.Systems;
using TheBindingOfRarria.Content.Buffs.Debuffs;

namespace TheBindingOfRarria.Content.Items;
public class SiphonBullets : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.width = 18;
        Item.height = 36;
        Item.rare = ItemRarityID.Lime;
        Item.value = Item.sellPrice(0, 6, 40, 0);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<SiphonPlayer>().VenusTrap = true;
        player.GetModPlayer<SiphonPlayer>().duration--;
    }
}

public class PlanteraBagLoot : GlobalItem
{
    public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
    {
        if (item.type == ItemID.PlanteraBossBag)
        {
            IItemDropRule rule = new ItemDropWithConditionRule(ModContent.ItemType<SiphonBullets>(), 5, 1, 1, new Conditions.IsExpert());
            itemLoot.Add(rule);
        }
    }
}

public class PlanteraLootNPC : GlobalNPC
{
    public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
    {
        if (npc.type == NPCID.Plantera)
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SiphonBullets>(), 5));

        base.ModifyNPCLoot(npc, npcLoot);
    }
}

public class SiphonPlayer : ModPlayer
{
    public bool VenusTrap = false;
    public int duration = 0;

    public override void ResetEffects()
    {
        if (!VenusTrap)
            duration = 0;

        VenusTrap = false;
    }

    public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (VenusTrap && BulletGlobalProjectile.Bullet.Contains(proj.type) && target.lifeMax > 5 && !target.immortal)
        {
            duration = 1200;
            Player.GetModPlayer<TemporaryLifePlayer>().bonuses.Add(new LifeBonus(1, 1200, p => !p.GetModPlayer<SiphonPlayer>().VenusTrap || p.GetModPlayer<SiphonPlayer>().duration <= 0));
            target.AddBuff(ModContent.BuffType<Siphoned>(), 1200);
            
            if (target.life >= target.lifeMax)
                target.life--;
            target.lifeMax--;
            target.GetGlobalNPC<SiphonedNPC>().SiphonedLife++;
        }
    }
}

public class SiphonedNPC : GlobalNPC
{
    public override bool InstancePerEntity => true;

    public int SiphonedLife = 0;

    public override void PostAI(NPC npc)
    {
        if (SiphonedLife > 0 && !npc.HasBuff(ModContent.BuffType<Siphoned>()))
        {
            npc.lifeMax += SiphonedLife;

            SiphonedLife = 0;
        }
    }
}