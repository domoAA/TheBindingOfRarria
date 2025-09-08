namespace TheBindingOfRarria.Content.Items;

public class WeightedShots : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.height = 40;
        Item.width = 34;
        Item.accessory = true;
        Item.rare = ItemRarityID.Pink;
        Item.value = Item.sellPrice(0, 3, 60, 0);
    }
    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<WeightedShotsPlayer>().Bruh = true;
        player.GetDamage(DamageClass.Ranged) += 0.2f;
    }
}

public class WeightedShotsPlayer : ModPlayer
{
    public bool Bruh = false;
    public override void ResetEffects()
    {
        Bruh = false;
    }
}

public class ElephantItemShop : GlobalNPC
{
    public override void ModifyShop(NPCShop shop)
    {
        if (shop.NpcType == NPCID.BestiaryGirl)
        {
            shop.Add(new Item(ModContent.ItemType<WeightedShots>()), Condition.DownedMechBossAny);
        }
    }
}

public class BruhGravityProjectile : GlobalProjectile
{
    public override bool InstancePerEntity => true;

    public bool Moving = false;

    public override bool ShouldUpdatePosition(Projectile projectile)
    {
        var should = base.ShouldUpdatePosition(projectile);
        Moving = should;

        return should;
    }

    public override void PostAI(Projectile projectile)
    {
        var owner = Main.player[projectile.owner];
        if (Moving && projectile.DamageType == DamageClass.Ranged && owner is not null && owner.TryGetModPlayer<WeightedShotsPlayer>(out var player) && player.Bruh)
        {
            projectile.velocity.Y += 0.1f;
        }
    }
}