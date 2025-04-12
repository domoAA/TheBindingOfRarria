using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items;

[AutoloadEquip(EquipType.Face)]
public class SuspiciousLookingMask : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.height = 30;
        Item.width = 28;
        Item.accessory = true;
        Item.rare = ItemRarityID.Expert;
        Item.value = Item.buyPrice(0, 3);
        Item.expert = true;
    }

        // Use a hashset.
    public Dictionary<int, int> immunities = [];

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<CrazyPlayer>().Insanity = Item;

        foreach (var buff in player.buffType) {
            if (Main.debuff[buff] && !immunities.ContainsKey(buff))
                immunities.Add(buff, 300); }

        foreach (var immunity in immunities)
        {
            immunities[immunity.Key] -= 1;
            if (immunity.Value < -600 || (!player.HasBuff(immunity.Key) && immunity.Value > 0))
                immunities.Remove(immunity.Key);
            else if (immunity.Value < 0)
                player.buffImmune[immunity.Key] = true;
        }
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.BoneHelm)
            .AddIngredient(ModContent.ItemType<MedicalIceBag>())
            .AddIngredient(ItemID.Vitamins)
            .AddIngredient(ItemID.SoulofNight, 20)
            .AddTile(TileID.ImbuingStation)
            .Register();
    }
}

public class CrazyPlayer : ModPlayer
{
    public Item Insanity = null;

    public override void ResetEffects() => Insanity = null;
    
    public void OnHitByAnything(Player.HurtInfo info, Vector2 target)
    {
        for (int i = Main.rand.Next(1, 3); i > 0; i--)
        {
            Vector2 offset = new(Main.screenWidth * Main.rand.NextFloat(0.2f, 0.8f), Main.screenHeight * Main.rand.NextFloat(0.2f, 0.8f));
            Vector2 position = Main.screenPosition + offset;
            position += position.DirectionTo(target) * (position.Distance(target) / 2 - 50);

            Projectile.NewProjectile(Player.GetSource_Accessory_OnHurt(Insanity, info.DamageSource), position, position.DirectionTo(target) * 6, ProjectileID.InsanityShadowFriendly, info.SourceDamage / 5 + 5, 3, Player.whoAmI);
        }
    }

    public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
    {
        if (Insanity != null)
            OnHitByAnything(hurtInfo, npc.Center);

        base.OnHitByNPC(npc, hurtInfo);
    }

    public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
    {
        if (Insanity != null)
        {
            Vector2 position = Player.Center;
            float distance = 800 * 800;

            foreach (var target in Main.ActiveNPCs)
            {
                if (!target.friendly && target.Center.DistanceSQ(Player.Center) < distance)
                {
                    distance = target.Center.DistanceSQ(Player.Center);
                    position = target.Center;
                }
            }

            OnHitByAnything(hurtInfo, position);
        }

        base.OnHitByProjectile(proj, hurtInfo);
    }
}