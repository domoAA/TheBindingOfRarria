using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using TheBindingOfRarria.Content.Projectiles;
using Terraria.ID;

namespace TheBindingOfRarria.Content.Items
{
    public class CarefreeMelody : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 34;
            Item.height = 30;
            Item.rare = ItemRarityID.Expert;
            Item.value = Item.buyPrice(0, 2);
            Item.expert = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<GrimmTroupeBanisherPlayer>().Melody = Item;
        }
        public override void AddRecipes()
        {
            Recipe.Create(Item.type)
                .AddIngredient(ItemID.DontStarveShaderItem)
                .AddIngredient(ItemID.WormScarf)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();

            Recipe.Create(Item.type)
                .AddIngredient(ItemID.DontStarveShaderItem)
                .AddIngredient(ItemID.BrainOfConfusion)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
            base.AddRecipes();
        }
    }
    public class GrimmTroupeBanisherPlayer : ModPlayer
    {
        public int Hits = 0;
        public Item Melody = null;
        public override void ResetEffects()
        {
            Melody = null;
        }
        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            base.OnHitByNPC(npc, hurtInfo);
            if (Melody != null)
                Hits++;
        }
        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
        {
            base.OnHitByProjectile(proj, hurtInfo);
            if (Melody != null)
                Hits++;
        }
        public override bool FreeDodge(Player.HurtInfo info)
        {
            if (Hits >= 5 && Melody != null)
            {
                Hits = 0;
                Player.immune = true;
                Player.immuneTime = 70;
                Projectile.NewProjectile(Player.GetSource_Accessory_OnHurt(Melody, info.DamageSource), Player.Center, Vector2.Zero, ModContent.ProjectileType<CarefreePower>(), 0, 0, Player.whoAmI);
                return true;
            }
            else
                return base.FreeDodge(info);
        }
    }
}