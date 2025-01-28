using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items
{
    [AutoloadEquip(EquipType.Face)]
    public class SuspiciousLookingMask : ModItem
    {
        public override void SetDefaults()
        {
            Item.height = 30;
            Item.width = 28;
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.buyPrice(0, 3);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<CrazyPlayer>().CritIncrease = 0;
            for (int i = 0; i < player.buffType.Length; i++)
            {
                if (Main.debuff[player.buffType[i]])
                    player.GetModPlayer<CrazyPlayer>().CritIncrease += 0.06f;
            }
        }
        public override void AddRecipes()
        {
            Recipe.Create(Item.type)
                .AddIngredient(ItemID.Skull)
                .AddIngredient(ItemID.Vitamins)
                .AddIngredient(ItemID.SoulofNight, 20)
                .AddTile(TileID.ImbuingStation)
                .Register();

            base.AddRecipes();
        }
    }
    public class CrazyPlayer : ModPlayer
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