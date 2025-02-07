using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using TheBindingOfRarria.Content.Projectiles;

namespace TheBindingOfRarria.Content.Items
{
    public class Censer : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
            var draw = new DrawAnimationVertical(8, 4);
            Main.RegisterItemAnimation(Item.type, draw);
        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 36;
            Item.accessory = true;
            Item.value = Item.buyPrice(0, 9);
            Item.rare = ItemRarityID.LightPurple;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.SpawnProjectileIfNotSpawned(ModContent.ProjectileType<SlowingAura>(), player.Center, player.GetSource_Accessory(Item));
        }
        public override void AddRecipes()
        {
            Recipe.Create(Item.type)
                .AddIngredient(ItemID.FastClock)
                .AddIngredient(ItemID.PutridScent)
                .AddIngredient(ItemID.SoulofLight, 10)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();

            base.AddRecipes();
        }
    }
}