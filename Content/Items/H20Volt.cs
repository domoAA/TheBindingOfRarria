using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Common.Helpers;
using TheBindingOfRarria.Common.Registries;
using TheBindingOfRarria.Content.Projectiles;

namespace TheBindingOfRarria.Content.Items;

public class H20Volt : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.width = 28;
        Item.height = 36;
        Item.accessory = true;
        Item.rare = ItemRarityID.Green;
        Item.value = Item.sellPrice(silver: 60);
    }

    public override void UpdateEquip(Player player)
    {
        if (player.TryGetModPlayer<H2VPlayer>(out var modPlayer))
            modPlayer.Equipped = true;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ModContent.ItemType<Fulgurbloom>(), 4)
            .AddIngredient(ItemID.CopperBar, 2)
            .AddTile(TileID.WorkBenches)
            .Register();
    }
}

public class H2VPlayer : ModPlayer
{
    public bool Equipped;

    public int counter = 0;

    public override void ResetEffects() => Equipped = false;

    public override void PostUpdateEquips()
    {
        if (!Equipped)
            return;

        counter++;

        if (counter >= 0)
        {
            counter = -30;
            int count = 4;

            int[] npcs = Helper.GetTargetIndices(Player.Center, count, 350f);

            if (npcs != null)
            {
                for (int i = 0; i < npcs.Length; i++)
                {
                    NPC npc = Main.npc[npcs[i]];

                    if (npc == null)
                        continue;

                    Vector2 start = Player.Center;
                    Vector2 end = npc.Center;

                    Helper.NewProjectileBetter(Player.GetSource_FromAI(), start, Vector2.Normalize(end - start) * 10f, ModContent.ProjectileType<LightningBolt>(), 7, 0f, Main.myPlayer, self =>
                    {
                        LightningBolt.SetPositions(start, end, self, npc.whoAmI);
                    });
                }
            }
        }
    }
}
