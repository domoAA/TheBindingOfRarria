using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Common.Systems;

namespace TheBindingOfRarria.Content.Items;

public class MagicMushroom : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.width = 24;
        Item.height = 28;
        Item.rare = ItemRarityID.Master;
        Item.value = Item.sellPrice(0, 4);
        Item.master = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        var p = player.GetModPlayer<RedMushPlayer>();
        p.FunGuy = true;
    }
}

public class RedMushPlayer : ModPlayer
{
    public float Growth = 1;

    public bool FunGuy = false;

    public override void ResetEffects()
    {
        FunGuy = false;
    }

    public override void PostUpdateEquips()
    {
        if (FunGuy)
        {
            Player.statLifeMax2 *= 11 / 10;
            Player.GetDamage(DamageClass.Generic) *= 1.1f;

            if (Collision.IsClearSpotTest(Player.position - new Vector2(0f, (Growth - 0.9f) * 40) + Player.velocity, 16f, (int)(Player.width * (Growth + 0.04f)), (int)(Player.height * (Growth + 0.04f)), fallThrough: true, fall2: true))
                Growth =
                Math.Min(
                    1.5f,
                    Growth + 0.04f
                    );

        }
        else
            Growth = Math.Max(1, Growth - 0.03f);

        if (Growth != 1)
            ResizedPlayerUtils.SetScale(Player, Growth);
    }
}

public class RedMushItemNPCShop : GlobalNPC
{
    public override void ModifyShop(NPCShop shop)
    {
        if (shop.NpcType == NPCID.Truffle)
        {
           shop.Add(new Item(ModContent.ItemType<MagicMushroom>()), Condition.InMasterMode);
        }
    }
}