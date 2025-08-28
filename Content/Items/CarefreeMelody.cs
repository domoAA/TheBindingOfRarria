using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Projectiles;

namespace TheBindingOfRarria.Content.Items;

public class CarefreeMelody : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.width = 34;
        Item.height = 30;
        Item.rare = ItemRarityID.Expert;
        Item.value = Item.sellPrice(0, 2);
        Item.expert = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual) => player.GetModPlayer<GrimmTroupeBanisherPlayer>().Melody = Item;

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.WormScarf)
            .AddIngredient(ItemID.Bell)
            .AddIngredient(ItemID.LivingFireBlock, 20)
            .AddIngredient(ModContent.ItemType<PaleOre>(), 30)
            .AddTile(TileID.TinkerersWorkbench)
            .Register();

        CreateRecipe()
            .AddIngredient(ItemID.BrainOfConfusion)
            .AddIngredient(ItemID.Bell)
            .AddIngredient(ItemID.LivingFireBlock, 20)
            .AddIngredient(ModContent.ItemType<PaleOre>(), 30)
            .AddTile(TileID.TinkerersWorkbench)
            .Register();
    }

    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        int index = tooltips.FindIndex(t => t.Name == "Tooltip0");
        if (index != -1)
        {
            string text = string.Format(Language.GetTextValue($"Mods.TheBindingOfRarria.Items.{Name}.Tooltip"), $"{(int)(Main.LocalPlayer.GetModPlayer<GrimmTroupeBanisherPlayer>().Chance * 100)}");

            text = text[..text.LastIndexOf($"\n")];
            text = text[..text.LastIndexOf($"\n")];
            tooltips[index].Text = text;
        }
    }
}

public class GrimmTroupeBanisherPlayer : ModPlayer
{
    public float Chance = 0;
    public Item Melody = null;

    public override void ResetEffects() => Melody = null;

    public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
    {
        base.OnHitByNPC(npc, hurtInfo);
        if (Melody != null)
        {
            Chance += 0.1f;
        }
    }

    public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
    {
        base.OnHitByProjectile(proj, hurtInfo);
        if (Melody != null)
        {
            Chance += 0.1f;
        }
    }

    public override bool FreeDodge(Player.HurtInfo info)
    {
        if (Main.rand.NextFloat() < Chance && Melody != null && info.CooldownCounter != ImmunityCooldownID.TileContactDamage)
        {
            Chance = 0;
            Player.immune = true;
            Player.immuneTime = 70;
            Projectile.NewProjectile(Player.GetSource_Accessory_OnHurt(Melody, info.DamageSource), Player.Center, Vector2.Zero, ModContent.ProjectileType<CarefreePower>(), 0, 0, Player.whoAmI);
            return true;
        }
        else
            return base.FreeDodge(info);
    }
}