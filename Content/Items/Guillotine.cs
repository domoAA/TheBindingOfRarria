using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using System;

namespace TheBindingOfRarria.Content.Items;

public class Guillotine : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.height = 36;
        Item.width = 32;
        Item.value = Item.sellPrice(0, 1);
        Item.rare = ItemRarityID.Master;
        Item.master = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<GuillotinePlayer>().Executioner = true;
    }
}

public class GuillotineItemNPCShop : GlobalNPC
{
    public override void ModifyActiveShop(NPC npc, string shopName, Item[] items)
    {
        if (npc.type == NPCID.Merchant && Main.masterMode)
        {

            var index = Array.FindLastIndex(items, i => i is not null && (i.type == ItemID.IronAnvil || i.type == ItemID.LeadAnvil));
            if (index == -1)
                return;

            for (int i = items.Length - 1; i > index; i--)
            {
                items[i] = items[i - 1];
            }

            items[index + 1] = new Item(ModContent.ItemType<Guillotine>());
        }
    }
}

public class GuillotinePlayer : ModPlayer
{
    public bool Executioner = false;

    public override void ResetEffects() => Executioner = false;

    public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
    {
        if (Executioner && target.life >= target.lifeMax * 0.75f)
        {
            modifiers.FinalDamage *= 1.25f;
        }
    }
}