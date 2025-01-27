using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items
{
    public class LoupesForWeakness : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.height = 24;
            Item.width = 32;
            Item.value = Item.buyPrice(0, 12);
            Item.rare = ItemRarityID.Yellow;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<RatioPlayer>().CritIncrease = 0.12f;
            player.GetCritChance(DamageClass.Generic) += 9;
            player.scope = true;
        }
        public override void AddRecipes()
        {
            Recipe.Create(Item.type)
                .AddIngredient(ItemID.SniperScope)
                .AddIngredient(ItemID.HoneyedGoggles)
                .AddIngredient(ItemID.SoulofSight, 20)
                .AddTile(TileID.Anvils)
                .Register();

            base.AddRecipes();
        }
    }
    public class RatioPlayer : ModPlayer
    {
        public float CritIncrease = 0f;
        public override void ResetEffects()
        {
            CritIncrease = 0;
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.CritDamage += CritIncrease;
            base.ModifyHitNPC(target, ref modifiers);
        }
    }
}