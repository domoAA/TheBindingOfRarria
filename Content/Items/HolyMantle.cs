using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Buffs;
using TheBindingOfRarria.Content.Projectiles;

namespace TheBindingOfRarria.Content.Items;

public class HolyMantle : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.width = 28;
        Item.height = 34;
        Item.accessory = true;
        Item.value = Item.buyPrice(0, 5);
        Item.rare = ItemRarityID.LightRed;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<ProtectedPlayer>().protection = Item;
        if (player.GetModPlayer<ProtectedPlayer>().counter > 0)
            player.AddBuff(ModContent.BuffType<HolyProtection_CD>(), player.GetModPlayer<ProtectedPlayer>().counter);
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.CrossNecklace)
            .AddIngredient(ItemID.ShimmerCloak)
            .AddIngredient(ItemID.SoulofLight, 3)
            .AddTile(TileID.TinkerersWorkbench)
            .Register();
    }
}
public class ProtectedPlayer : ModPlayer
{
    public Item protection = null;

    public int counter = 0;

    public override void ResetEffects()
    {
        if ((protection == null || counter <= 0) && Player.HasBuff(ModContent.BuffType<HolyProtection_CD>()))
            Player.ClearBuff(ModContent.BuffType<HolyProtection_CD>());

        protection = null;
    }

    public override void PostUpdateEquips()
    {
        if (protection != null) {
            if (counter <= 0)
            {
                Player.AddBuff(ModContent.BuffType<HolyProtection>(), 2);
            }
            counter--;
        }
    }

    public override bool FreeDodge(Player.HurtInfo info)
    {
        if (Player.HasBuff(ModContent.BuffType<HolyProtection>())) {
            Player.immune = true;
            int time = Player.longInvince ? 150 : 90;
            Player.SetImmuneTimeForAllTypes(time);
            Player.ClearBuff(ModContent.BuffType<HolyProtection>());
            counter = 3600;
            Projectile.NewProjectile(Player.GetSource_Accessory_OnHurt(protection, info.DamageSource), Player.Center, Vector2.Zero, ModContent.ProjectileType<HolyMantleBurst>(), 0, 0, Player.whoAmI);
            return true; }
        else 
            return base.FreeDodge(info);
    }
}