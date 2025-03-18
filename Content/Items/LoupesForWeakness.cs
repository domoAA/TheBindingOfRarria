
namespace TheBindingOfRarria.Content.Items
{
    public class LoupesForWeakness : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.height = 24;
            Item.width = 32;
            Item.value = Item.buyPrice(0, 1);
            Item.rare = ItemRarityID.Pink;
        }
        public override void UpdateAccessory(Player player, bool hideVisual) => player.GetModPlayer<RatioPlayer>().Nanamin = true;
        
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.HoneyedGoggles)
                .AddIngredient(ItemID.SoulofSight, 20)
                .AddTile(TileID.MythrilAnvil)
                .Register();

            base.AddRecipes();
        }
    }
    public class RatioPlayer : ModPlayer
    {
        public bool Nanamin = false;
        public override void ResetEffects() => Nanamin = false;
        
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Nanamin)
                modifiers.Defense *= 0.75f;
        }
    }
}