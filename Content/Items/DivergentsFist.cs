using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Projectiles;

namespace TheBindingOfRarria.Content.Items;

public class DivergentsFist : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.width = 26;
        Item.height = 32;
        Item.rare = ItemRarityID.Pink;
        Item.value = Item.sellPrice(0, 3);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<YujiItemPlayer>().counter--;
        player.GetModPlayer<YujiItemPlayer>().Fist = Item;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ModContent.ItemType<CursedBlood>())
            .AddIngredient(ItemID.PowerGlove)
            .AddIngredient(ItemID.SoulofFright, 4)
            .AddTile(TileID.Anvils)
            .Register();
    }
}

public class YujiItemPlayer : ModPlayer
{
    public Item Fist = null;
    public int counter = 0;

    public override void ResetEffects() => Fist = null;

    public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (Fist != null && counter <= 0)
        {
            counter = 180;
            Vector2 offset = target.Center.DirectionTo(Player.Center) * target.Hitbox.Size() * 0.5f;

            if (Main.myPlayer != Player.whoAmI)
                return;

            Projectile.NewProjectile(Player.GetSource_Accessory(Fist, "Yuji fist attack"), target.Center + offset, -offset, ModContent.ProjectileType<CEFist>(), Player.statManaMax2 / 10, 5, Player.whoAmI, target.whoAmI, offset.X, offset.Y);
        }
    }
}