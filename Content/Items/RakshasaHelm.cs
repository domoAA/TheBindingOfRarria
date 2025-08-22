using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TheBindingOfRarria.Common.Config;

namespace TheBindingOfRarria.Content.Items;

[AutoloadEquip(EquipType.Head)]
public class RakshasaHelm : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetStaticDefaults()
    {
        ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;
    }

    public override void SetDefaults()
    {
        Item.width = 22;
        Item.height = 18;
        Item.defense = 17;
        Item.lifeRegen = 2;
        Item.rare = ItemRarityID.LightRed;
        Item.value = Item.sellPrice(0, 4, 0, 0);
    }

    public override void UpdateEquip(Player player)
    {
        player.GetDamage(DamageClass.Generic) += 0.09f;
    }

    public override bool IsArmorSet(Item head, Item body, Item legs)
    {
        return head.type == Item.type && body.type == ModContent.ItemType<RakshasaPlatemail>() && legs.type == ModContent.ItemType<RakshasaGreaves>();
    }

    public override void UpdateArmorSet(Player player)
    {
        player.setBonus = Language.GetTextValue($"Mods.TheBindingOfRarria.Items.{Name}.SetBonus");

        player.GetModPlayer<RakshasaPlayer>().Rakshasa = true;
    }

    public override void ArmorSetShadows(Player player)
    {
        player.armorEffectDrawOutlines = true;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.AdamantiteHelmet)
            .AddIngredient(ItemID.CrimsonHelmet)
            .AddIngredient(ItemID.SoulofNight, 10)
            .AddIngredient(ItemID.BloodWater, 4)
            .AddTile(TileID.AdamantiteForge)
            .Register();
    }
}

public class RakshasaPlayer : ModPlayer
{
    public bool Rakshasa = false;

    public int counter = 0;

    public override void ResetEffects()
    {
        if (!Rakshasa)
            counter = 0;

        Rakshasa = false;
    }

    public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
    {
        if (Rakshasa)
        {
            counter++;

            if (counter == 6)
            {
                counter = 0;
                modifiers.SetCrit();
            }
        }
    }
}