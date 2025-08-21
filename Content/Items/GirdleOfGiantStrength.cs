using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Common.Systems;

namespace TheBindingOfRarria.Content.Items;

public class GirdleOfGiantStrength : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.width = 26;
        Item.height = 24;
        Item.accessory = true;
        Item.rare = ItemRarityID.Expert;
        Item.value = Item.sellPrice(0, 2);
        Item.expert = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<GiantPlayer>().HasGirdleOfGiantStrength = true;
    }
}

public class GiantItemNPCShop : GlobalNPC
{
    public override void ModifyActiveShop(NPC npc, string shopName, Item[] items)
    {
        // tavernkeep
        if (npc.type == NPCID.DD2Bartender && Main.expertMode)
        {
            var index = Array.FindIndex(items, e => e == null);

            if (index != -1)
                items[index] = new Item(ModContent.ItemType<GirdleOfGiantStrength>())
                {
                    shopCustomPrice = 12,
                    shopSpecialCurrency = CustomCurrencyID.DefenderMedals

                };

        }
    }
}

public class GiantPlayer : ModPlayer
{
    public bool HasGirdleOfGiantStrength = false;

    public bool Healed = false;

    public override void ResetEffects()
    {
        HasGirdleOfGiantStrength = false;
    }

    public override void Load()
    {
        base.Load();
        Terraria.On_Player.Heal += On_Player_Heal;
        On_Player.HealEffect += On_Player_HealEffect;
    }

    private void On_Player_HealEffect(On_Player.orig_HealEffect orig, Player self, int healAmount, bool broadcast)
    {
        if (!Healed && self.GetModPlayer<GiantPlayer>().HasGirdleOfGiantStrength)
        {
            int bonus = healAmount / 2;
            if (bonus > 0)
            {
                self.statLifeMax2 += bonus;
                self.GetModPlayer<TemporaryLifePlayer>().bonuses.Add(new LifeBonus(bonus, 600, cond => !CheckGiantStrength(self)));
            }
        }

        Healed = false;
        orig(self, healAmount, broadcast);
    }

    private void On_Player_Heal(On_Player.orig_Heal orig, Player self, int amount)
    {
        if (self.GetModPlayer<GiantPlayer>().HasGirdleOfGiantStrength)
        {
            int bonus = amount / 2;
            if (bonus > 0)
            {
                self.statLifeMax2 += bonus;
                self.GetModPlayer<TemporaryLifePlayer>().bonuses.Add(new LifeBonus(bonus, 600, cond => !CheckGiantStrength(self)));
            }
        }

        Healed = true;
        orig(self, amount);
    }

    public static bool CheckGiantStrength(Player player)
    {
        return player.GetModPlayer<GiantPlayer>().HasGirdleOfGiantStrength;
    }
}