using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using System;
using System.Linq;
using TheBindingOfRarria.Common.Registries;
using Terraria.Audio;

namespace TheBindingOfRarria.Content.Items;

public class DeathsBell : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.height = 32;
        Item.width = 22;
        Item.rare = ItemRarityID.LightRed;
        Item.value = Item.sellPrice(0, 4);
    }

    public int counter = 0;

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        counter++;

        if (counter > 1800)
        {
            var target = Main.npc.FirstOrDefault(t => t.active && !t.friendly && !t.boss && t.aiStyle != NPCAIStyleID.DD2MysteriousPortal && t.lifeMax > 5 && t.rarity == 0 && t.Center.DistanceSQ(player.Center) < 800 * 800);
            if (target is not null)
            {
                counter = 0;
                target.life = 0;
                SoundEngine.PlaySound(Sounds.Bell, player.Center);
            }
        }
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.Bell)
            .AddIngredient(ModContent.ItemType<PaleOre>(), 13)
            .AddIngredient(ItemID.SoulofNight, 7)
            .AddIngredient(ItemID.CursedFlame, 5)
            .AddTile(TileID.DemonAltar)
            .Register();
    }
}