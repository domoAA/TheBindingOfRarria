using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.Graphics.Renderers;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TheBindingOfRarria.Common.Config;
using TheBindingOfRarria.Common.Helpers;
using TheBindingOfRarria.Common.Registries;
using TheBindingOfRarria.Content.Dusts;
using TheBindingOfRarria.Content.Projectiles;

namespace TheBindingOfRarria.Content.Items;

[AutoloadEquip(EquipType.Head)]
public class GodrickSoldierHelm : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetStaticDefaults()
    {
        ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;
    }

    public override void SetDefaults()
    {
        Item.width = 20;
        Item.height = 18;
        Item.defense = 5;
        Item.value = Item.sellPrice(0, 1, 50, 0);
    }

    public override void UpdateEquip(Player player)
    {

    }

    public override bool IsArmorSet(Item head, Item body, Item legs)
    {
        return head.type == Item.type && body.type == ModContent.ItemType<GodrickSoldierTabard>() && legs.type == ModContent.ItemType<GodrickSoldierBoots>();
    }

    public override void UpdateArmorSet(Player player)
    {
        player.setBonus = Language.GetTextValue($"Mods.TheBindingOfRarria.Items.{Name}.SetBonus");
        player.GetModPlayer<GodrickSoldierSetPlayer>().FinalBoss = true;
    }
}

public partial class GodrickSoldierItemsNPCShop : GlobalNPC
{
    public override void ModifyActiveShop(NPC npc, string shopName, Item[] items)
    {
        // tavernkeep
        if (npc.type == NPCID.DD2Bartender)
        {
            var index = 10;
            if (items[index] != null)
                index = Array.FindIndex(items, e => e == null);

            if (index != -1)
                items[index] = new Item(ModContent.ItemType<GodrickSoldierHelm>())
                {
                    shopCustomPrice = 6,
                    shopSpecialCurrency = CustomCurrencyID.DefenderMedals

                };

            AddTabard(ref items);
            AddBoots(ref items);
        }
    }
}


public class GodrickSoldierSetPlayer : ModPlayer
{
    public bool FinalBoss = false;

    public override void UpdateLifeRegen()
    {
        if (FinalBoss)
        {
            Player.lifeRegen += Player.numMinions;
            Player.lifeRegen += Main.projectile.Count(p => p.active && p.owner == Player.whoAmI && p.sentry);
        }
    }
}
