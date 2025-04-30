using System;
using Terraria;
using Terraria.ID;
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
        if (CommuninstPig && Player.statMana > ManaCost) {
            float MagicDamagePercent = (Player.GetTotalDamage(DamageClass.Magic).Additive - 1) * 100;
            float MagicDamageSqrt = MathF.Sqrt(MagicDamagePercent);
            float DamageBlock = 1 - ((MagicDamageSqrt / 100) + 0.10f);
            modifiers.FinalDamage *= DamageBlock;
            Player.statMana -= ManaCost;
        }
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