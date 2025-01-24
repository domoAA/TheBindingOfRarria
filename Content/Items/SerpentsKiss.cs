using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items
{
    public class SerpentsKiss : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 20;
            Item.accessory = true;
            Item.value = Item.buyPrice(0, 1);
            Item.rare = ItemRarityID.Orange;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<KissedPlayer>().IsKissed = true;
        }
        public override void AddRecipes()
        {
            Recipe.Create(Item.type)
                .AddIngredient(ItemID.LovePotion)
                .AddIngredient(ItemID.FlaskofPoison)
                .AddTile(TileID.ImbuingStation)
                .Register();

            Recipe.Create(Item.type)
                .AddIngredient(ItemID.LifeCrystal)
                .AddIngredient(ItemID.FlaskofPoison)
                .AddTile(TileID.ImbuingStation)
                .Register();

            base.AddRecipes();
        }
    }
    public class KissedPlayer : ModPlayer
    {
        public bool IsKissed = false;
        public override void ResetEffects()
        {
            IsKissed = false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (IsKissed)
                target.AddBuff(BuffID.Poisoned, 120);
            base.OnHitNPC(target, hit, damageDone);
        }
        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            if (IsKissed)
                npc.AddBuff(BuffID.Poisoned, 360);
            base.OnHitByNPC(npc, hurtInfo);
        }
    }
}