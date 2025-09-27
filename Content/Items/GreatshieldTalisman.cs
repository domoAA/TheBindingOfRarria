using System;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace TheBindingOfRarria.Content.Items;

public class GreatshieldTalisman : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
    {
        if (equippedItem.type == ModContent.ItemType<ShieldTalisman>() && incomingItem.type == Type || incomingItem.type == ModContent.ItemType<ShieldTalisman>() && equippedItem.type == Type)
            return false;

        return base.CanAccessoryBeEquippedWith(equippedItem, incomingItem, player);
    }

    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.height = 36;
        Item.width = 24;
        Item.value = Item.sellPrice(0, 5);
        Item.rare = ItemRarityID.Expert;
        Item.expert = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<GreatshieldTalismanPlayer>().GreatshieldCrest = true;
        player.noKnockback = true;
    }
}

public class GreatshieldItemNPCShop : GlobalNPC
{
    public override void ModifyActiveShop(NPC npc, string shopName, Item[] items)
    {
        // tavernkeep
        if (npc.type == NPCID.DD2Bartender && Main.expertMode && NPC.downedMechBossAny)
        {
            var index = Array.FindIndex(items, e => e == null);

            if (index != -1)
                items[index] = new Item(ModContent.ItemType<GreatshieldTalisman>())
                {
                    shopCustomPrice = 15,
                    shopSpecialCurrency = CustomCurrencyID.DefenderMedals

                };

        }
    }
}

public class GreatshieldTalismanPlayer : ModPlayer
{
    public bool GreatshieldCrest = false;

    public override void ResetEffects() => GreatshieldCrest = false;

    public override void PostUpdateEquips()
    {
        if (!Player.hasRaisableShield)
            GreatshieldCrest = false;
    }

    public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
    {
        if (GreatshieldCrest)
        {
            modifiers.FinalDamage *= 0.8f;
        }
    }
}