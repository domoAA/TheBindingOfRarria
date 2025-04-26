using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items;

public class ShamanStone : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.height = 30;
        Item.width = 30;
        Item.value = Item.buyPrice(0, 6);
        Item.rare = ItemRarityID.Master;
        Item.master = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetDamage(DamageClass.Magic) += 0.1f;
        player.GetModPlayer<ShamanSnailPlayer>().ShamanStone = true;
    }
}

public class WallOfFleshBagLoot : GlobalItem
{
    public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
    {
        if (item.type == ItemID.WallOfFleshBossBag)
        {
            IItemDropRule rule = new ItemDropWithConditionRule(ModContent.ItemType<ShamanStone>(), 10, 1, 1, new Conditions.IsMasterMode());
            itemLoot.Add(rule);
        }
    }
}

public class ShamanSnailPlayer : ModPlayer
{
    public bool ShamanStone = false;

    public override void ResetEffects() => ShamanStone = false;
}

public class ShamanSnailGlobalProjectile : GlobalProjectile 
{
    public override bool InstancePerEntity => true;
    private bool CastByShaman = false;

    public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => entity.friendly && entity.DamageType == DamageClass.Magic;
    
    public override bool PreAI(Projectile projectile)
    {
        if (!CastByShaman && Main.player[projectile.owner].GetModPlayer<ShamanSnailPlayer>().ShamanStone)
        {
            CastByShaman = true;
            projectile.Resize((int)(projectile.width * 1.5f), (int)(projectile.height * 1.5f));
            projectile.scale *= 1.5f;
        }

        return base.PreAI(projectile);
    }
}