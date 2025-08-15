
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using System;
using System.Linq;

namespace TheBindingOfRarria.Content.Items;

public class BloodBag : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.width = 20;
        Item.height = 32;
        Item.accessory = true;
        Item.lifeRegen = 2;
        Item.value = Item.sellPrice(0, 2);
        Item.expert = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<BloodBagPlayer>().HasIVBag = true;
    }
}

public partial class NurseItemsShop : GlobalNPC
{
    public void AddBloodBag(NPC npc, ref Item[] items)
    {
        if (npc.type == NPCID.Merchant && Main.expertMode && Main.npc.Any(target => target.type == NPCID.Nurse && target.Center.DistanceSQ(npc.Center) <= 800 * 800))
        {

            var index = Array.FindLastIndex(items, i => i is not null && (i.master || i.type == ItemID.IronAnvil || i.type == ItemID.LeadAnvil));
            if (index == -1)
                return;

            for (int i = items.Length - 1; i > index; i--)
            {
                items[i] = items[i - 1];
            }

            items[index + 1] = new Item(ModContent.ItemType<BloodBag>());
        }
    }
}

public class BloodBagPlayer : ModPlayer
{
    public (int timer, int blood) counter = (0, 0);

    public bool HasIVBag = false;

    public override void Load()
    {
        On_Player.UpdateLifeRegen += On_Player_UpdateLifeRegen;
    }

    public override void Unload()
    {
        On_Player.UpdateLifeRegen -= On_Player_UpdateLifeRegen;
    }

    private void On_Player_UpdateLifeRegen(On_Player.orig_UpdateLifeRegen orig, Player self)
    {
        var player = self.GetModPlayer<BloodBagPlayer>();

        if (player.HasIVBag && self.lifeRegen > 0 && self.lifeRegenCount >= 0 && self.lifeRegenCount < player.counter.timer) 
        {
            if (self.statLife >= self.statLifeMax2)
                player.counter.blood += (player.counter.timer - self.lifeRegenCount) / 60;

            else if (player.counter.blood > 0 && self.statLife < self.statLifeMax2 / 2)
            {
                self.statLife += 2;
                player.counter.blood -= 2;
            }

            player.counter.blood = Math.Clamp(player.counter.blood, 0, self.statLifeMax2 / 3);
        }

        player.counter.timer = self.lifeRegenCount;

        orig(self);


        if (!player.HasIVBag)
            player.counter = (0, 0);

        player.HasIVBag = false;
    }
}