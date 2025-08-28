using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Projectiles;

namespace TheBindingOfRarria.Content.Items;

public class CompanionJar : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.width = 32;
        Item.height = 32;
        Item.accessory = true;
        Item.rare = ItemRarityID.Green;
        Item.value = Item.sellPrice(0, 1);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<PotTalismanPlayer>().HasSillyJar = true;
    }
}
public class PotTalismanPlayer : ModPlayer
{
    public bool HasSillyJar = false;
    public int killCounter = 0;

    public override void ResetEffects()
    {
        HasSillyJar = false;
    }
}

public class PotBreakTile : GlobalTile
{
    public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
    {
        if (type == TileID.Pots && (Main.LocalPlayer.Center - new Point(i, j).ToWorldCoordinates()).LengthSquared() < 800 * 800 && Main.LocalPlayer.GetModPlayer<PotTalismanPlayer>().HasSillyJar && Main.tile[i + 1, j + 1].TileType == TileID.Pots)
        {
            if (Main.LocalPlayer.ownedProjectileCounts[ModContent.ProjectileType<PotMinion>()] < 5)
            {
                Projectile.NewProjectile(Main.LocalPlayer.GetProjectileSource_TileInteraction(i, j), Main.LocalPlayer.Center - new Vector2(0, 100).RotatedBy(float.Sin(Main.GlobalTimeWrappedHourly)), Vector2.Zero, ModContent.ProjectileType<PotMinion>(), 0, 0);
            } 
        }
    }
}