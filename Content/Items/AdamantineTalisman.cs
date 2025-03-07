
namespace TheBindingOfRarria.Content.Items
{
    public class AdamantineTalisman : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.height = 30;
            Item.width = 26;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.buyPrice(0, 4, 0, 4);
        }
        public override void UpdateAccessory(Player player, bool hideVisual) => player.GetModPlayer<LowLuckRollPlayer>().Talisman = true;
        
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.BlackPearl)
                .AddIngredient(ItemID.WhitePearl)
                .AddIngredient(ItemID.AdamantiteOre, 20)
                .AddIngredient(ModContent.ItemType<PaleOre>(), 20)
                .AddIngredient(ItemID.Diamond, 10)
                .AddTile(TileID.AdamantiteForge)
                .Register();

            base.AddRecipes();
        }
    }
    public class LowLuckRollPlayer : ModPlayer
    {
        public bool Talisman = false;
        public static (float original, int rolled) LuckRoll = (0, 0);
        public override void ResetEffects() => Talisman = false;
        
        public override void Load()
        {
            On_Main.DamageVar_float_int_float += RegisterLuckRoll;

            On_Player.Hurt_PlayerDeathReason_int_int_refHurtInfo_bool_bool_int_bool_float_float_float += UseLowLuckRoll;
        }

        private double UseLowLuckRoll(On_Player.orig_Hurt_PlayerDeathReason_int_int_refHurtInfo_bool_bool_int_bool_float_float_float orig, Player self, Terraria.DataStructures.PlayerDeathReason damageSource, int Damage, int hitDirection, out Player.HurtInfo info, bool pvp, bool quiet, int cooldownCounter, bool dodgeable, float armorPenetration, float scalingArmorPenetration, float knockback)
        {
            if (Damage % LuckRoll.rolled == 0 && self.GetModPlayer<LowLuckRollPlayer>().Talisman)
            {
                Damage = Math.Max(1, (int)Math.Round(LuckRoll.original * (Damage / LuckRoll.rolled) * (1 - Main.DefaultDamageVariationPercent * 0.01f)));
                LuckRoll = (0, 0);
            }
            return orig(self, damageSource, Damage, hitDirection, out info, pvp, quiet, cooldownCounter, dodgeable, armorPenetration, scalingArmorPenetration, knockback);
        }

        private int RegisterLuckRoll(On_Main.orig_DamageVar_float_int_float orig, float dmg, int percent, float luck)
        {
            var result = orig(dmg, percent, luck);

            LuckRoll.original = dmg;
            LuckRoll.rolled = result;
            
            return result;
        }
    }
}