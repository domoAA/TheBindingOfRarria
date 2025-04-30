using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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
        Item.value = Item.buyPrice(0, 5);
        Item.expert = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        var p = player.GetModPlayer<GiantPlayer>();

        p.HasGirdleOfGiantStrength = true;
        var activeBonuses = p.activeBonuses;

        for (int i = activeBonuses.Count - 1; i >= 0; i--)
        {
            activeBonuses[i].TimeLeft--;
            player.statLifeMax2 += activeBonuses[i].Amount;

            if (activeBonuses[i].TimeLeft <= 0)
            {
                player.statLifeMax2 -= activeBonuses[i].Amount;
                activeBonuses.RemoveAt(i);
            }
        }
    }
}

public class GiantItemNPCShop : GlobalNPC
{
    public override void ModifyShop(NPCShop shop)
    {
        if (shop.NpcType == NPCID.TravellingMerchant)
        {
            shop.Add(new Item(ModContent.ItemType<GirdleOfGiantStrength>()), Condition.InExpertMode);
        }
    }
}

public class GiantPlayer : ModPlayer
{
    public bool HasGirdleOfGiantStrength = false;
    public readonly List<LifeBonus> activeBonuses = [];

    public class LifeBonus
    {
        public int Amount;
        public int TimeLeft;

        public LifeBonus(int amount, int duration)
        {
            Amount = amount;
            TimeLeft = duration;
        }
    }

    public override void Load()
    {
        base.Load();
        Terraria.On_Player.Heal += On_Player_Heal;
    }

    private void On_Player_Heal(On_Player.orig_Heal orig, Player self, int amount)
    {
        if (self.GetModPlayer<p>().HasGirdleOfGiantStrength)
        {
            int bonus = amount / 2;
            if (bonus > 0)
            {
                self.statLifeMax2 += bonus;
                self.GetModPlayer<p>().activeBonuses.Add(new LifeBonus(bonus, 300));
            }
        }
        orig(self, amount);
    }


    public override void ResetEffects()
    {
        HasGirdleOfGiantStrength = false;
    }
}