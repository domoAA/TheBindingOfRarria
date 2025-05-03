using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Projectiles;

namespace TheBindingOfRarria.Content.Items;

public class CryptBloom : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.width = 30;
        Item.height = 28;
        Item.accessory = true;
        Item.rare = ItemRarityID.Green;
        Item.value = Item.buyPrice(0, 1);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<CryptBloomPlayer>().HasCryptBloom = true;
    }
}
public class CryptBloomPlayer : ModPlayer
{
    public bool HasCryptBloom = false;
    public int killCounter = 0;

    public override void ResetEffects()
    {
        HasCryptBloom = false;
    }

    public void OnEnemyKill(Vector2 npcCenter)
    {
        if (HasCryptBloom)
        {
            killCounter++;
            if (killCounter >= 10)
            {
                Projectile.NewProjectile(Player.GetSource_FromThis(), npcCenter, Vector2.Zero, ModContent.ProjectileType<CryptAura>(), 0, 0, Player.whoAmI);
                killCounter = 0;
            }
        }
    }
}

public class CryptBloomGlobalNPC : GlobalNPC
{
    public override void OnKill(NPC npc)
    {
        if (!npc.CountsAsACritter && !npc.friendly)
        {
            foreach (Player player in Main.player)
            {
                if (player.active && !player.dead && player.GetModPlayer<CryptBloomPlayer>().HasCryptBloom)
                {
                    float distanceSquared = Vector2.DistanceSquared(player.Center, npc.Center);
                    if (distanceSquared <= 800f * 800f)
                    {
                        player.GetModPlayer<CryptBloomPlayer>().OnEnemyKill(npc.Center);
                    }
                }
            }
        }
    }
}

public class DeathFlowerNPCShop : GlobalNPC
{
    public override void ModifyShop(NPCShop shop)
    {
        if (shop.NpcType == NPCID.Dryad)
        {
            shop.Add(new Item(ModContent.ItemType<CryptBloom>()), Condition.InGraveyard);
        }
    }
}
