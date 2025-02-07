using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Buffs;

namespace TheBindingOfRarria.Content.Items
{
    public class HolyMantle : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 34;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<ProtectedPlayer>().IsProtected = true;
        }
        public override void AddRecipes()
        {
            Recipe.Create(Item.type)
                .AddIngredient(ItemID.CrossNecklace)
                .AddIngredient(ItemID.ShimmerCloak)
                .AddIngredient(ItemID.SoulofLight, 3)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();

            base.AddRecipes();
        }
    }
    public class ProtectedPlayer : ModPlayer
    {
        public bool IsProtected = false;
        public int CD = 0;
        public override void ResetEffects()
        {
            IsProtected = false;
        }
        public override void PostUpdateEquips()
        {
            if (!Player.HasBuff(ModContent.BuffType<HolyProtection>()))
            CD--;

            if (CD <= 0 && IsProtected){
                Player.AddBuff(ModContent.BuffType<HolyProtection>(), 2);
                CD = 3000; }
        }
        public override bool FreeDodge(Player.HurtInfo info)
        {
            var should = Player.HasBuff(ModContent.BuffType<HolyProtection>());
            if (should) {
                Player.immune = true;
                int time = Player.longInvince ? 150 : 90;
                Player.SetImmuneTimeForAllTypes(time);
                Player.ClearBuff(ModContent.BuffType<HolyProtection>());
                return true; }

            return base.FreeDodge(info);
        }
    }
}