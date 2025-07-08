using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items;

public class SoulCatcher : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.height = 30;
        Item.width = 30;
        Item.rare = ItemRarityID.Green;
        Item.value = Item.buyPrice(0, 0, 60);
    }

    public override void UpdateAccessory(Player player, bool hideVisual) => player.GetModPlayer<SoulPlayer>().IsSoul = true;
    
    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ModContent.ItemType<PaleOre>(), 24)
            .AddIngredient(ItemID.ManaCrystal)
            .AddTile(TileID.Furnaces)
            .Register();
    }
}

public class SoulPlayer : ModPlayer
{
    public bool IsSoul;
    private bool SoulTook;

    public override void ResetEffects()
    {
        if (SoulTook && Player.ItemAnimationEndingOrEnded)
            SoulTook = false;

        IsSoul = false;
    }

    public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (IsSoul && !SoulTook)
        {
            SoulTook = true;
            Player.statMana = Math.Min(Player.statMana + 9, Player.statManaMax2);
            Player.ManaEffect(6);
        }
    }
}