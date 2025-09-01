using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Projectiles;

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

    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        float value = 25 * (Main.LocalPlayer.GetModPlayer<ShieldTalismanPlayer>().crit / 100f + 1);

        string text = string.Format(Language.GetTextValue($"Mods.TheBindingOfRarria.Items.{Name}.Tooltip"), $"{(int)value}");

        int index = tooltips.FindIndex(line => line.Name == "Tooltip0");
        if (index != -1)
        {
            text = text[..text.IndexOf($"\n")];
            tooltips[index].Text = text;
        }
    }
}

public class ShieldItemNPCShop : GlobalNPC
{
    public override void ModifyActiveShop(NPC npc, string shopName, Item[] items)
    {
        // tavernkeep
        if (npc.type == NPCID.DD2Bartender)
        {
            var index = Array.FindIndex(items, e => e != null && e.type == ModContent.ItemType<GreatshieldTalisman>());

            if (index != -1)
            {
                if (items[index + 1] == null)
                    items[index + 1] = new Item(ModContent.ItemType<ShieldTalisman>())
                    {
                        shopCustomPrice = 5,
                        shopSpecialCurrency = CustomCurrencyID.DefenderMedals

                    };
                else
                {
                    for (int i = 9; i > index + 1; i--)
                    {
                        items[i] = items[i - 1];
                    }

                    items[index + 1] = new Item(ModContent.ItemType<ShieldTalisman>())
                    {
                        shopCustomPrice = 5,
                        shopSpecialCurrency = CustomCurrencyID.DefenderMedals

                    };
                }
            }
            else if (Array.FindIndex(items, e => e == null) != -1)
            {
                for (int i = 9; i > index + 1; i--)
                {
                    items[i] = items[i - 1];
                }

                items[index + 1] = new Item(ModContent.ItemType<ShieldTalisman>())
                {
                    shopCustomPrice = 5,
                    shopSpecialCurrency = CustomCurrencyID.DefenderMedals

                };
            }
        }
    }
}

public class ShieldTalismanPlayer : ModPlayer
{
    public bool ShieldCrest = false;

    public int crit = 0;

    public override void ResetEffects() => ShieldCrest = false;

    public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
    {
        if (ShieldCrest && Main.rand.Next(100) < (int)(25 * (Main.LocalPlayer.GetModPlayer<ShieldTalismanPlayer>().crit / 100f + 1)))
        {
            modifiers.FinalDamage *= 0.5f;

            Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Vector2.Zero, ModContent.ProjectileType<ShieldSphere>(), 0, 0);
        }
    }

    public override void PostUpdate()
    {
        crit = (int)Player.GetCritChance(DamageClass.Generic);
    }
}