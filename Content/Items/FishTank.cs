using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Projectiles;

namespace TheBindingOfRarria.Content.Items;

public class FishTank : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.height = 36;
        Item.width = 34;
        Item.rare = ItemRarityID.Green;
        Item.value = Item.sellPrice(0, 0, 70, 7);
    }

    public override void UpdateAccessory(Player player, bool hideVisual) => player.GetModPlayer<FishTankPlayer>().IsTuna = true;
}

public class FishTankPlayer : ModPlayer
{
    public bool IsTuna;

    public override void ResetEffects()
    {
        if (Player.timeSinceLastDashStarted <= 1 && Main.myPlayer == Player.whoAmI && IsTuna)
            Projectile.NewProjectile(Player.GetSource_FromThis(), new Vector2(Player.Center.X + (Player.direction * Player.velocity.X), Player.Center.Y), new Vector2(Player.velocity.X, Player.velocity.Y - 2f), ModContent.ProjectileType<FlippityFloppity>(), 20, 2, Main.myPlayer);
        IsTuna = false;
    }

    public override void CatchFish(FishingAttempt attempt, ref int itemDrop, ref int npcSpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition)
    {
        bool inWater = !attempt.inLava && !attempt.inHoney;

                // Weird, idk why you'd want compatability for that.
            // Only made the ocean require 1k water instead of location for Skyblock players
        if (Main.rand.NextBool(25) && inWater && attempt.heightLevel == 1 && attempt.waterTilesCount >= 1000 && attempt.veryrare)
        {
            itemDrop = ModContent.ItemType<FishTank>();
            return;
        }
    }
}