using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using TheBindingOfRarria.Content.Projectiles;
using Terraria.DataStructures;
using Terraria.Localization;

namespace TheBindingOfRarria.Content.Items;

public class TheLastOffering : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.width = 32;
        Item.height = 34;
        Item.accessory = true;
        Item.rare = ItemRarityID.Orange;
        Item.value = Item.sellPrice(0, 1);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<LastOfferingPlayer>().Skul = Item;
    }
}

public class LastOfferingPlayer : ModPlayer
{
    public Item Skul = null;

    public override void ResetEffects()
    {
        Skul = null;
    }

    public void OnEnemyKill()
    {
        if (Skul != null)
        {
            Projectile.NewProjectile(Player.GetSource_Accessory(Skul, "TheLastOffering homing missile"), Player.Center, new Vector2(0, -10), ModContent.ProjectileType<HomingMissile>(), 20, 2, ai0: -1, ai1: 2);
        }
    }

    public override void CatchFish(FishingAttempt attempt, ref int itemDrop, ref int npcSpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition)
    {
        if (attempt.veryrare && Main.rand.NextBool(6) && Player.ZoneDungeon)
        {
            npcSpawn = -1;
            sonar.Color = new Color(255, 200, 150);
            sonar.Text = Language.GetTextValue("Mods.TheBindingOfRarria.Items.TheLastOffering.DisplayName");
            itemDrop = ModContent.ItemType<TheLastOffering>();
        }
    }
}

public class LastOfferingGlobalNPC : GlobalNPC
{
    public override void OnKill(NPC npc)
    {
        if (!npc.CountsAsACritter && !npc.friendly)
        {
            foreach (Player player in Main.player)
            {
                if (player.active && !player.dead && player.GetModPlayer<LastOfferingPlayer>().Skul != null)
                {
                    float distanceSquared = Vector2.DistanceSquared(player.Center, npc.Center);
                    if (distanceSquared <= 600f * 600f)
                    {
                        player.GetModPlayer<LastOfferingPlayer>().OnEnemyKill();
                    }
                }
            }
        }
    }
}