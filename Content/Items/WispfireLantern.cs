using System;
using TheBindingOfRarria.Common;
using TheBindingOfRarria.Content.Projectiles;

namespace TheBindingOfRarria.Content.Items;

public class WispfireLantern : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.width = 26;
        Item.height = 28;
        Item.rare = ItemRarityID.Expert;
        Item.value = Item.sellPrice(0, 0, 7, 20);
        Item.expert = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<WispfirePlayer>().Bonfire = true;
        player.manaCost -= 0.05f;
    }

}

public class WispfireItemNPCShop : GlobalNPC
{
    public override void ModifyActiveShop(NPC npc, string shopName, Item[] items)
    {
        // tavernkeep
        if (npc.type == NPCID.DD2Bartender && Main.expertMode)
        {
            var index = Array.FindIndex(items, e => e == null);

            if (index != -1)
                items[index] = new Item(ModContent.ItemType<WispfireLantern>())
                {
                    shopCustomPrice = 8,
                    shopSpecialCurrency = CustomCurrencyID.DefenderMedals

                };

        }
    }
}

public class WispfirePlayer : ModPlayer
{
    public bool Bonfire = false;

    public override void ResetEffects()
    {
        Bonfire = false;
    }

    public override void OnConsumeMana(Item item, int manaConsumed)
    {
        if (Main.myPlayer != Player.whoAmI || !Bonfire)
            return;

        if ((Player.statManaMax2 - Player.statMana) / (float)Player.statManaMax2 * 8 + 1 > Player.ownedProjectileCounts[ModContent.ProjectileType<Wispfire>()])
            Projectile.NewProjectile(Player.GetProjectileSource_Item(item), Player.Center, Vector2.Zero, ModContent.ProjectileType<Wispfire>(), 0, 0);
    }
}