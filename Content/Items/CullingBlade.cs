using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items;

public class CullingBlade : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.height = 28;
        Item.width = 30;
        Item.rare = ItemRarityID.Expert;
        Item.expert = true;
        Item.value = Item.buyPrice(0, 1, 50);
    }

    public override void UpdateAccessory(Player player, bool hideVisual) => player.GetModPlayer<CullPlayer>().PlayedTheseGamesBefore = Item;

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.DeathSickle)
            .AddIngredient(ItemID.Sickle)
            .AddIngredient(ItemID.SoulofMight, 10)
            .AddIngredient(ItemID.SoulofFright, 10)
            .AddIngredient(ItemID.SoulofNight, 10)
            .AddCondition(Condition.NearShimmer)
            .AddCondition(Condition.InExpertMode)
            .Register();
    }
}

public class CullPlayer : ModPlayer
{
        // God kain...
        // What? It's a ref to Culling Games, google it
    public Item PlayedTheseGamesBefore = null;

    private int killCounter = 0;

    public override void ResetEffects() => PlayedTheseGamesBefore = null;

    public override void PostUpdate() => killCounter = killCounter > 0 ? killCounter - 1 : 0;

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (target.CountsAsACritter || target.immortal)
            return;

        if (PlayedTheseGamesBefore != null)
        {
            Player.Heal(1);
            Item.NewItem(Player.GetSource_Accessory(PlayedTheseGamesBefore), target.Center, ItemID.CopperCoin, (int)(Math.Max(0, Player.luck) * 10 + 1));
        }

        if (target.life <= 0 && Main.expertMode)
        {
            killCounter += 25;
            if (killCounter >= 1000)
            {
                killCounter = 0;
                Item.NewItem(target.GetSource_Death("Cull drop"), target.Center, ModContent.ItemType<CullingBlade>());
            }
        }
    }
}