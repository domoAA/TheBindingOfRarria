
using System;
using System.IO;
using System.Linq;
using Terraria.Audio;
using Terraria.ModLoader.IO;
using TheBindingOfRarria.Common.Helpers;
using TheBindingOfRarria.Content.Projectiles;

namespace TheBindingOfRarria.Content.Items;

public class NightriderFlail : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetStaticDefaults()
    {
        // This line will make the damage shown in the tooltip twice the actual Item.damage. This multiplier is used to adjust for the dynamic damage capabilities of the projectile.
        // When thrown directly at enemies, the flail projectile will deal double Item.damage, matching the tooltip, but deals normal damage in other modes.
        ItemID.Sets.ToolTipDamageMultiplier[Type] = 2f;
    }

    public override void SetDefaults()
    {
        Item.useStyle = ItemUseStyleID.Shoot; 
        Item.useAnimation = 45;
        Item.useTime = 45; 
        Item.knockBack = 5.5f;
        Item.width = 46; 
        Item.height = 38; 
        Item.damage = 40;
        Item.noUseGraphic = true; 
        Item.shoot = ModContent.ProjectileType<TripleFlail>();
        Item.shootSpeed = 13f; 
        Item.UseSound = SoundID.Item1; 
        Item.rare = ItemRarityID.LightRed; 
        Item.value = Item.sellPrice(gold: 1, silver: 50); 
        Item.DamageType = DamageClass.MeleeNoSpeed; 
        Item.channel = true;
        Item.noMelee = true; 
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        for (int i = 0; i < 3; i++) 
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback);
        return false;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.BallOHurt)
            .AddIngredient(ItemID.SoulofNight, 15)
            .AddTile(TileID.MythrilAnvil)
            .Register();
    }
}