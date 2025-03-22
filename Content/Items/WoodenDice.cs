
namespace TheBindingOfRarria.Content.Items
{
    public class WoodenDice : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 26;
            Item.height = 28;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.buyPrice(0, 0, 7, 20);
        }
        public override void UpdateAccessory(Player player, bool hideVisual) => player.GetModPlayer<DiceyPlayer>().Dice = true;

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Wood)
                .AddIngredient(ItemID.BlackInk)
                .AddIngredient(ItemID.LuckPotion)
                .AddTile(TileID.WorkBenches)
                .Register();

            base.AddRecipes();
        }
    }
    public class DiceyPlayer : NewRollPlayer
    {
        public bool Dice = false;
        public static void DiceReroll(Player self, ref int Damage)
        {
            if (LuckRoll.rolled != 0 && Damage % LuckRoll.rolled == 0 && self.GetModPlayer<DiceyPlayer>().Dice)
            {
                var bound = self.GetModPlayer<NewRollPlayer>().Talisman ? 100 : 100 + Main.DefaultDamageVariationPercent;

                var roll = Math.Max(1, CustomRangeDamageVar(Damage, 100 - Main.DefaultDamageVariationPercent, bound));

                if (roll > Damage)
                    Damage = roll;
            }
        }
    }
}