using Terraria;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Projectiles;
using static TheBindingOfRarria.Content.Items.PlanetPlayer;

namespace TheBindingOfRarria.Content.Items
{
    public class Saturnus : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PlanetPlayer>().planet = Planets.Saturnus;
            player.SpawnProjectileIfNotSpawned(ModContent.ProjectileType<SaturnusRing>());
        }
    }
}