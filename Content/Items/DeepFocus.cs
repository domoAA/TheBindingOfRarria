using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace TheBindingOfRarria.Content.Items
{
    public class DeepFocus : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.height = 30;
            Item.width = 30;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.buyPrice(0, 0, 80);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<FocusedPlayer>().IsFocused = true;

            player.potionDelayTime = (int)(player.potionDelayTime * 1.2f);
            player.mushroomDelayTime = (int)(player.mushroomDelayTime * 1.2f);
            player.restorationDelayTime = (int)(player.restorationDelayTime * 1.2f);
        }
        public override void AddRecipes()
        {
            Recipe.Create(Item.type)
                .AddIngredient(ItemID.ArcaneCrystal)
                .AddIngredient(ItemID.StarinaBottle)
                .AddIngredient(ModContent.ItemType<PaleOre>(), 18)
                .AddIngredient(ItemID.Amethyst, 22)
                .AddTile(TileID.Solidifier)
                .Register();

            base.AddRecipes();
        }
    }
    public class FocusedPlayer : ModPlayer
    {
        public bool IsFocused;
        public override void ResetEffects()
        {
            IsFocused = false;
        }

        public override void GetHealLife(Item item, bool quickHeal, ref int healValue)
        {
            if (IsFocused)
            {
                healValue *= 2;
            }
        }
    }
    public class DeepFocusDropGem : GlobalTile
    {
        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (type == TileID.Amethyst && Main.rand.NextFloat() < 0.01f)
            {
                Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), new Point(i, j).ToWorldCoordinates(), ModContent.ItemType<DeepFocus>(), 1, false, Main.rand.Next(0, PrefixID.Count));
                noItem = true;
            }
            else
                base.KillTile(i, j, type, ref fail, ref effectOnly, ref noItem);
        }
    }
}