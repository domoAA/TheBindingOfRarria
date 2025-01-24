using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Projectiles;

namespace TheBindingOfRarria.Content.Items
{
    public class BrokenWatch : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.height = 24;
            Item.width = 22;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.buyPrice(0, 5);
        }
        public int counter =600;
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (counter <= 0)
            {
                counter = 600 + Main.rand.Next(-300, 301);
                if (Main.rand.NextBool())
                {
                    foreach (var target in Main.ActiveNPCs)
                    {
                        if (target == null)
                            continue;

                        target.velocity *= 0.7f;
                        target.GetGlobalNPC<NPCExtensions.SlowedGlobalNPC>().Slowed = (true, 300);
                    }
                    foreach (var proj in Main.ActiveProjectiles)
                    {
                        if (proj == null)
                            continue;

                        proj.velocity *= 0.7f;
                        proj.GetGlobalProjectile<SlowedGlobalProjectile>().Slowed = (true, 300);
                    }
                }
                else
                {
                    foreach (var target in Main.ActiveNPCs)
                    {
                        if (target == null)
                            continue;

                        target.velocity *= 1.3f;
                    }
                    foreach (var proj in Main.ActiveProjectiles)
                    {
                        if (proj == null || proj.owner == player.whoAmI)
                            continue;

                        proj.velocity *= 1.3f;
                    }
                }
            }
            else counter--;
        }
        public override void AddRecipes()
        {
            Recipe.Create(Item.type)
                .AddIngredient(ItemID.FastClock)
                .AddIngredient(ItemID.StoneBlock)
                .AddTile(TileID.MythrilAnvil)
                .Register();

            base.AddRecipes();
        }
    }
}