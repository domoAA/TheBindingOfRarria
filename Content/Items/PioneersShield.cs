using TheBindingOfRarria.Content.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace TheBindingOfRarria.Content.Items
{
    // Copied over from other file.
    [AutoloadEquip(EquipType.Shield)]
    public class PioneersShield : ModItem
    {
        public override void SetDefaults()
        {
            // Copied over from other file.
            Item.width = 24;
            Item.height = 30;
            Item.accessory = true;
            Item.defense = 4;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.buyPrice(0, 2);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // Copied over from other file.
            //  player.buffImmune[BuffID.Slow] = true;
            // player.moveSpeed += 0.07f;
            player.noKnockback = true;

            player.GetModPlayer<PioneerPlayer>().IsPioneer = true;
        }

        public override void AddRecipes()
        {
            Recipe.Create(Item.type)
            .AddIngredient(ItemID.ThePlan)
            .AddRecipeGroup("GoldBar", 10)
            .AddIngredient(ItemID.CobaltShield)
            .AddTile(TileID.MythrilAnvil)
            .Register();
            // Recipe idea: Gold for color, Planned cuz Slow/Confused Immune, Cobalt for knockback/shield

            // Copied over from other file.
            Recipe.Create(Item.type)
                .AddIngredient(ItemID.CobaltShield)
                .AddIngredient(ItemID.GoldBar, 13)
                .AddIngredient(ItemID.SoulofFlight, 7)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
        // Which recipe you freaking wit?
    }

    public class PioneerPlayer : ModPlayer
    {
        int[] SlowingDebuffs = [31, 32, 33, 46, 47, 67, 149, 156, 164, 194, 197];
        private bool ImmuneAndBuff(int currentBuff)
        {
            foreach (int arrayBuff in SlowingDebuffs)
                if (Player.HasBuff(arrayBuff) || (Main.debuff[currentBuff] && currentBuff != BuffID.Sunflower && (Main.GetBuffTooltip(Player, currentBuff).Contains("Move") || Main.GetBuffTooltip(Player, currentBuff).Contains("move"))))
                {
                    bool WhichOne = Player.HasBuff(arrayBuff) ? Player.buffImmune[arrayBuff] = true : Player.buffImmune[currentBuff] = true;
                    return WhichOne;
                }
            return false;

        }
        public bool IsPioneer;
        public override void ResetEffects()
        {
            IsPioneer = false;
            base.ResetEffects();

        }

        public override void UpdateEquips()
        {
            if (IsPioneer)
                foreach (int buff in Player.buffType)
                {
                    if (ImmuneAndBuff(buff))
                    {
                        Player.AddBuff(ModContent.BuffType<ShieldSpeed>(), 300);
                    }

                }
        }
    }

    public class GoldBarGroup : ModSystem
    {
        public override void AddRecipeGroups()
        {
            // Because why is this not a thing!!!!!!
            // For the very niche time when you get platinum from the extractinator in a gold world
            RecipeGroup AnyGoldBar = new RecipeGroup(() => $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.GoldBar)}", ItemID.GoldBar, ItemID.PlatinumBar);
            RecipeGroup.RegisterGroup(nameof(ItemID.GoldBar), AnyGoldBar);

        }
    }
}