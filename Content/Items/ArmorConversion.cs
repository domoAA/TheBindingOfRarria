using Terraria;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items
{
    public class ArmorConversion : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 32;
            Item.accessory = true;
        }
        public int CD = 300;
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statLifeMax2 += (player.statLifeMax2 / 100) * player.statDefense;
            player.statDefense -= player.statDefense;
            CD--;
            if (CD <= 0) {
                player.Heal(player.statLifeMax2 / 100);
                CD = 300; }
        }
    }
}