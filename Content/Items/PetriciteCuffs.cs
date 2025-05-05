using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items;

public class PetriciteCuffs : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.height = 26;
        Item.width = 28;
        Item.value = Item.buyPrice(0, 3, 14);
        Item.rare = ItemRarityID.Master;
        Item.expert = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<PetriciteCuffsPlayer>().CommuninstPig = true;
    }

    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        float chance = PetriciteCuffsPlayer.CalculatePetriciteBlock(Main.LocalPlayer);

        string text = string.Format(Language.GetTextValue($"Mods.TheBindingOfRarria.Items.{Name}.Tooltip"), $"{(int)(chance * 100)}");

        int index = tooltips.FindIndex(line => line.Name == "Tooltip0");
        if (index != -1)
        {
            text = text.Remove(text.LastIndexOf($"\n"));
            text = text.Remove(text.LastIndexOf($"\n"));
            tooltips[index].Text = text;
        }
    }
}
public class PetriciteCuffsPlayer : ModPlayer
{
    public bool CommuninstPig = false;

    public override void ResetEffects()
    {
        CommuninstPig = false;
    }

    public void ModifyHitByAnything(ref Player.HurtModifiers modifiers)
    {
        int ManaCost = (int)(20 * Player.manaCost);
        if (CommuninstPig && Player.statMana >= ManaCost) {
            var blocked = CalculatePetriciteBlock(Player);

            modifiers.FinalDamage *= (1 - blocked);
            Player.statMana -= ManaCost;
        }
    }

    public static float CalculatePetriciteBlock(Player player)
    {
        var value = 0.1f;

        float MagicDamagePercent = (player.GetTotalDamage(DamageClass.Magic).Additive - 1) * 100;
        float MagicDamageSqrt = MathF.Sqrt(MagicDamagePercent);

        value += (MagicDamageSqrt / 100);

        return value;
    }

    public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
    {
        ModifyHitByAnything(ref modifiers);
    }

    public override void ModifyHitByProjectile(Projectile projectile, ref Player.HurtModifiers modifiers)
    {
        ModifyHitByAnything(ref modifiers);
    }
}
public partial class DoranItemsNPCShop : GlobalNPC
{
    public void PetriCuffs(NPCShop shop)
    {
        if (shop.TryGetEntry(ItemID.IronAnvil, out var entry))
            shop.InsertAfter(entry, new Item(ModContent.ItemType<PetriciteCuffs>()), Condition.InMasterMode);

        else if (shop.TryGetEntry(ItemID.LeadAnvil, out entry))
            shop.InsertAfter(entry, new Item(ModContent.ItemType<PetriciteCuffs>()), Condition.InMasterMode);

    }
}