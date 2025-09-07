using Terraria.GameContent.ItemDropRules;

namespace TheBindingOfRarria.Content.Items;

public class PollipPouch : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.width = 38;
        Item.height = 36;
        Item.value = Item.sellPrice(0, 5, 30);
        Item.rare = ItemRarityID.Yellow;
    }

    public override void UpdateAccessory(Player player, bool hideVisual) => player.GetModPlayer<PollipPlayer>().NotCoral = true;

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ModContent.ItemType<DevilsTrumpet>())
            .AddIngredient(ItemID.Coral, 10)
            .AddTile(TileID.TinkerersWorkbench)
            .Register();
    }
}

public class PollipPlayer : ModPlayer
{
    public bool NotCoral = false;

    public override void ResetEffects() => NotCoral = false;

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (NotCoral && Main.rand.NextBool(10))
        {
            target.AddBuff(BuffID.Venom, 300);
        }
    }
}