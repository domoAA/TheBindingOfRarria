using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Buffs;
using TheBindingOfRarria.Content.Projectiles;
using Microsoft.Xna.Framework;

namespace TheBindingOfRarria.Content.Items
{
    public class HolyMantle : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 34;
            Item.accessory = true;
            Item.value = Item.buyPrice(0, 5);
            Item.rare = ItemRarityID.LightRed;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<ProtectedPlayer>().protection = Item;
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
        public Item protection = null;
        public int CD = 0;
        public override void ResetEffects()
        {
            protection = null;
        }
        public override void PostUpdateEquips()
        {
            if (!Player.HasBuff(ModContent.BuffType<HolyProtection>()))
            CD--;

            if (CD <= 0 && protection != null)
            {
                Player.AddBuff(ModContent.BuffType<HolyProtection>(), 2);
                CD = 3600; }
        }
        public override bool FreeDodge(Player.HurtInfo info)
        {
            if (Player.HasBuff(ModContent.BuffType<HolyProtection>())) {
                Player.immune = true;
                int time = Player.longInvince ? 150 : 90;
                Player.SetImmuneTimeForAllTypes(time);
                Player.ClearBuff(ModContent.BuffType<HolyProtection>());
                Projectile.NewProjectile(Player.GetSource_Accessory_OnHurt(protection, info.DamageSource), Player.Center, Vector2.Zero, ModContent.ProjectileType<HolyMantleBurst>(), 0, 0, Player.whoAmI);
                return true; }
            else 
                return base.FreeDodge(info);
        }
    }
}