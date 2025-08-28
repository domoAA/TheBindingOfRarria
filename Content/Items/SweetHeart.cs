using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items;

public class SweetHeart : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.height = 24;
        Item.width = 26;
        Item.rare = ItemRarityID.LightRed;
        Item.value = Item.sellPrice(0, 1, 33, 33);
    }

    public override void UpdateAccessory(Player player, bool hideVisual) => player.GetModPlayer<SweetPlayer>().Sweetie = true;
    
    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.CrimsonHeart)
            .AddIngredient(ItemID.LifeCrystal, 2)
            .AddIngredient(ItemID.SoulofLight, 8)
            .AddTile(TileID.CookingPots)
            .Register();
    }
}

public class SweetPlayer : ModPlayer
{
    public bool Sweetie = false;

    public override void ResetEffects() => Sweetie = false;
    
    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        base.OnHitNPC(target, hit, damageDone);

        if (!Sweetie || Player.lifeSteal <= 0 || !target.canGhostHeal || target.immortal || target.lifeMax <= 5)
            return;

        Player.Heal(damageDone / 10);
        Player.lifeSteal -= damageDone * 2;
    }

    public override void UpdateBadLifeRegen()
    {
        if (Sweetie)
            Player.lifeRegen -= 4;
    }
}

public class CrateLootSweetHeart : GlobalItem
{
    public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
    {
        if (item.type == ItemID.CrimsonFishingCrateHard)
        {
            IItemDropRule rule = ItemDropRule.Common(ModContent.ItemType<SweetHeart>(), 6);
            itemLoot.Add(rule);
        }
    }
}