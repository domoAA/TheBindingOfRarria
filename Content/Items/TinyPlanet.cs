using Terraria;
using Terraria.ModLoader;
using static TheBindingOfRarria.Content.Items.PlanetPlayer;

namespace TheBindingOfRarria.Content.Items
{
    public class TinyPlanet : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 28;
            Item.height = 28;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PlanetPlayer>().planet = Planets.Earth;
        }
    }
    public class PlanetPlayer : ModPlayer
    {
        public enum Planets
        {
            None,
            Earth,
            Saturnus
        }
        public Planets planet = Planets.None;
        public override void ResetEffects()
        {
            planet = Planets.None;
        }
    }
}