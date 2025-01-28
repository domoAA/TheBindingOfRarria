using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items
{
    public class CombustiveToxin : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.height = 32;
            Item.width = 28;
            Item.value = Item.buyPrice(0, 0, 66, 66);
            Item.rare = ItemRarityID.Green;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<CombustivePlayer>().Combust = true;
        }
        public override void AddRecipes()
        {
            Recipe.Create(Item.type)
                .AddIngredient(ItemID.Bottle)
                .AddIngredient(ItemID.FlaskofFire, 5)
                .AddIngredient(ItemID.FlaskofPoison, 3)
                .AddTile(TileID.Anvils)
                .Register();

            base.AddRecipes();
        }
    }
    public class CombustivePlayer : ModPlayer
    {
        public bool Combust = false;
        public override void ResetEffects()
        {
            Combust = false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!Combust)
                return;

            if (target.buffType.Length <= 0)
                target.AddBuff(BuffID.Poisoned, 180);

            base.OnHitNPC(target, hit, damageDone);
        }
    }
}