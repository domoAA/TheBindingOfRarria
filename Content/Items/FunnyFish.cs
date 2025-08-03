using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Projectiles;

namespace TheBindingOfRarria.Content.Items;

public class FunnyFish : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.width = 30;
        Item.height = 30;
        Item.rare = ItemRarityID.Green;
        Item.value = Item.buyPrice(0, 0, 30, 70);
    }

    public override void UpdateAccessory(Player player, bool hideVisual) => player.GetModPlayer<FunnyPlayer>().IsFunny = true;
}

public class FunnyPlayer : ModPlayer
{
    public bool IsFunny;

    public override void ResetEffects() => IsFunny = false;

    public override void CatchFish(FishingAttempt attempt, ref int itemDrop, ref int npcSpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition)
    {
        bool inWater = !attempt.inLava && !attempt.inHoney;

                // Again odd compat.
            // Only made the ocean require 1k water instead of location for Skyblock players
        if (Main.rand.NextBool(25) && inWater && attempt.heightLevel == 1 && attempt.waterTilesCount >= 1000 && attempt.rare)
        {
            itemDrop = ModContent.ItemType<FunnyFish>();
            return;
        }
    }

    public override void ModifyShootStats(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
    {
        if (IsFunny && type == ProjectileID.Bullet)
        {
            type = ModContent.ProjectileType<FishBullet>();
            damage += 3;
        }
    }
}