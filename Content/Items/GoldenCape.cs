
namespace TheBindingOfRarria.Content.Items;

[AutoloadEquip(EquipType.Back, EquipType.Front)]
public class GoldenCape : ModItem
{
    public override void SetDefaults()
    {
        Item.height = 30;
        Item.width = 30;
        Item.accessory = true;
        Item.value = Item.buyPrice(0, 3);
        Item.rare = ItemRarityID.Green;
    }
    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.noFallDmg = true;
        player.GetModPlayer<SpikeImmunePlayer>().SpikeImmune = true;
    }
}
public class SpikeImmunePlayer : ModPlayer
{
    public bool SpikeImmune;
    public bool Immune = false;
    public override void ResetEffects() => SpikeImmune = false;
    
    public override bool FreeDodge(Player.HurtInfo info)
    {
        if (Immune)
        {
            Player.immune = true;
            Player.immuneTime = 40;
            Immune = false;
            return true;
        }
        return base.FreeDodge(info);
    }
    public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
    {
        if (SpikeImmune)
        {
            if (proj.type == ProjectileID.Boulder || proj.type == ProjectileID.PoisonDartTrap || proj.type == ProjectileID.SpearTrap || proj.type == ProjectileID.SpikyBallTrap || proj.type == ProjectileID.GeyserTrap || proj.type == ProjectileID.PoisonDart || proj.type == ProjectileID.FlamethrowerTrap || proj.type == ProjectileID.Explosives || proj.type == ProjectileID.GasTrap || proj.type == ProjectileID.FlamesTrap || proj.type == ProjectileID.TNTBarrel || proj.type == ProjectileID.LifeCrystalBoulder || proj.type == ProjectileID.RollingCactus || proj.type == ProjectileID.MiniBoulder)
                Immune = true;
        }
    }
    public override bool ImmuneTo(PlayerDeathReason damageSource, int cooldownCounter, bool dodgeable)
    {
        if (damageSource.SourceOtherIndex == 3 && SpikeImmune)
            return true;

        return false;
    }
}
public class CrateLootGoldenCape : GlobalItem
{
    public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
    {
        if (item.type == ItemID.OasisCrateHard || item.type == ItemID.OasisCrate)
        {
            var rule = ItemDropRule.Common(ModContent.ItemType<GoldenCape>(), 12, 1, 1);
            itemLoot.Add(rule);
        }
    }
}
public class GoldenCapeChestLoot : ModSystem
{
    public override void PostWorldGen()
    {
        for (int chestIndex = 0; chestIndex < Main.maxChests; chestIndex++)
        {
            Chest chest = Main.chest[chestIndex];
            if (chest == null)
            {
                continue;
            }
            Tile chestTile = Main.tile[chest.x, chest.y];
            if (chestTile.TileType == TileID.Containers && chestTile.TileFrameX == 10 * 36)
            {
                if (WorldGen.genRand.NextFloat() > 0.5f || !chest.item.Any(bast => bast.type == ItemID.CatBast))
                    continue;

                for (int inventoryIndex = 0; inventoryIndex < Chest.maxItems; inventoryIndex++)
                {
                    if (chest.item[inventoryIndex].type == ItemID.None)
                    {
                        chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<GoldenCape>());
                        break;
                    }
                }
            }
        }
    }
}