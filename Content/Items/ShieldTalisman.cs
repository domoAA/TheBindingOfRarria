using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using System;

namespace TheBindingOfRarria.Content.Items;

public class ShieldTalisman : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
    {
        if (equippedItem.type == ModContent.ItemType<GreatshieldTalisman>() && incomingItem.type == Type || incomingItem.type == ModContent.ItemType<GreatshieldTalisman>() && equippedItem.type == Type)
            return false;

        return base.CanAccessoryBeEquippedWith(equippedItem, incomingItem, player);
    }

    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.height = 36;
        Item.width = 24;
        Item.value = Item.sellPrice(0, 1);
        Item.rare = ItemRarityID.Orange;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<ShieldTalismanPlayer>().ShieldCrest = true;
    }
}

public class ShieldItemNPCShop : GlobalNPC
{
    public override void ModifyActiveShop(NPC npc, string shopName, Item[] items)
    {
        // tavernkeep
        if (npc.type == NPCID.DD2Bartender)
        {
            var index = Array.FindIndex(items, e => e == null);

            if (index != -1)
                items[index] = new Item(ModContent.ItemType<ShieldTalisman>())
                {
                    shopCustomPrice = 5,
                    shopSpecialCurrency = CustomCurrencyID.DefenderMedals

                };

        }
    }
}

public class ShieldTalismanPlayer : ModPlayer
{
    public bool ShieldCrest = false;

    public override void ResetEffects() => ShieldCrest = false;

    public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
    {
        if (ShieldCrest)
        {
            var difference = npc.Center - Player.Center;
            if (difference.X * Player.direction > Player.width / 2 && Math.Abs(npc.width) / Math.Abs(npc.height) < Math.Abs(difference.X) / Math.Abs(difference.Y))
            {
                modifiers.FinalDamage *= 0.9f;
            }
        }
    }
}