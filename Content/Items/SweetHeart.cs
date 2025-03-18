
namespace TheBindingOfRarria.Content.Items
{
    public class SweetHeart : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.height = 24;
            Item.width = 26;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.buyPrice(0, 1, 33, 33);
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

            base.AddRecipes();
        }
    }
    public class SweetPlayer : ModPlayer
    {
        public bool Sweetie = false;
        public override void ResetEffects() => Sweetie = false;
        
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
            if (!Sweetie || Player.lifeSteal <= 0 || !target.canGhostHeal)
                return;

            Player.Heal(1 + damageDone / 10);
            Player.lifeSteal -= damageDone * 2;
        }
        public override void UpdateBadLifeRegen()
        {
            if (Sweetie)
                Player.lifeRegen -= 4;
            base.UpdateBadLifeRegen();
        }
    }
    public class CrateLootSweetHeart : GlobalItem
    {
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            if (item.type == ItemID.CrimsonFishingCrateHard)
            {
                var rule = ItemDropRule.Common(ModContent.ItemType<SweetHeart>(), 6);
                itemLoot.Add(rule);
            }
        }
    }
}