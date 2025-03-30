namespace TheBindingOfRarria.Content.Items;

public class BrilliantBehemoth : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 50;
        Item.height = 50;
        Item.value = Item.buyPrice(gold: 22);
        Item.rare = ItemRarityID.Lime;
        Item.accessory = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<BrilliantBehemothPlayer>().Active = true;
    }
}

public class BrilliantBehemothPlayer : ModPlayer
{
    private const float ExplosionRadius = 8f * 16f;
    private const float ExplosionDamageMult = 0.2f;
    private const float ExplosionKnockbackMult = 0.2f;

    public bool Active;

    public override void ResetEffects()
    {
        Active = false;
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (!Active)
        {
            return;
        }

        foreach (var npc in Main.ActiveNPCs)
        {
            if (npc.WithinRange(target.Center, ExplosionRadius) && npc.whoAmI != target.whoAmI)
            {
                //int direction = float.Sign(target.DirectionTo(npc.Center).X);
                //npc.SimpleStrikeNPC((int)(hit.Damage * ExplosionDamageMult), direction, knockBack: hit.Knockback * ExplosionKnockbackMult);
            }
        }

        // Fiery dust explosion
        for (int i = 0; i < 15; i++)
        {
            Dust fireDust = Dust.NewDustDirect(target.position, target.width, target.height, DustID.Torch);
            fireDust.velocity = Main.rand.NextVector2Circular(10f, 10f);
            fireDust.scale = Main.rand.NextFloat(1.3f, 2f);
            fireDust.noGravity = true;
        }

        // Smoke explosion
        for (int i = 0; i < 8; i++)
        {
            Dust fireDust = Dust.NewDustDirect(target.position, target.width, target.height, DustID.Smoke);
            fireDust.velocity = Main.rand.NextVector2Circular(10f, 10f);
            fireDust.noGravity = true;
        }

        // Fiery dust on the enemy
        for (int i = 0; i < 3; i++)
        {
            Dust fireDust = Dust.NewDustDirect(target.position, target.width, target.height, DustID.Torch);
            fireDust.scale = Main.rand.NextFloat(1.8f, 2.5f);
            fireDust.noGravity = true;
            fireDust.alpha = 120;
        }
    }
}

public class BrilliantBehemothGlobalNPC : GlobalNPC
{
    public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
    {
        return entity.type == NPCID.Demolitionist;
    }

    public override void ModifyShop(NPCShop shop)
    {
        shop.InsertAfter(ItemID.Dynamite, ModContent.ItemType<BrilliantBehemoth>(), Condition.DownedGolem);
    }
}