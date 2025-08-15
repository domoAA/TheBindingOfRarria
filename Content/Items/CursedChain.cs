using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items;

[AutoloadEquip(EquipType.Neck)]
public class CursedChain : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.width = 28;
        Item.height = 32;
        Item.accessory = true;
        Item.value = Item.sellPrice(0, 9);
        Item.rare = ItemRarityID.LightPurple;
    }

    public override void UpdateAccessory(Player player, bool hideVisual) => player.GetModPlayer<RiskyHealPlayer>().HasChoker = true;
    
    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.CrossNecklace)
            .AddIngredient(ItemID.SoulofMight, 20)
            .AddIngredient(ItemID.SoulofNight, 10)
            .AddIngredient(ItemID.CursedFlame, 10)
            .AddIngredient(ItemID.GoldBar, 10)
            .AddIngredient(ItemID.Shackle)
            .AddTile(TileID.MythrilAnvil)
            .Register();
    }
}

public class RiskyHealPlayer : ModPlayer
{
        // me-core
        // Huh?
    public bool HasChoker = false;
    private int totalHeal = 0;
    private float currentHeal = 0.24f;
    private int CD = 60;

    public override void ResetEffects() => HasChoker = false;
    
    public override void PostUpdateEquips()
    {
        if (!HasChoker)
            totalHeal = 0;

        if (totalHeal != 0)
            CD--;
        else
            CD = 60;

        if (CD <= 0) {
            Player.Heal((int)(totalHeal * currentHeal));

            bool IsLast = currentHeal <= 0.09f;
            currentHeal -= 0.03f;

            if (IsLast)
            {
                totalHeal = 0;
                currentHeal = 0.24f;
            }
            CD = 60; }
    }

    public void ModifyHitByAnything(ref Player.HurtModifiers modifiers)
    {
        if (HasChoker)
            modifiers.FinalDamage *= 1.5f;
    }

    public override void ModifyHurt(ref Player.HurtModifiers modifiers) => ModifyHitByAnything(ref modifiers);

    public override void UpdateDead() => totalHeal = 0;

    public override void OnHurt(Player.HurtInfo info) => OnHitByAnything(info);

    public void OnHitByAnything(Player.HurtInfo hurtInfo) 
    {
        if (HasChoker)
            totalHeal += (int)(hurtInfo.Damage * 0.5f); 
    }
}