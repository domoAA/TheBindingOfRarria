using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TheBindingOfRarria.Common.Helpers;
using TheBindingOfRarria.Content.Dusts;

namespace TheBindingOfRarria.Content.Items;

public class FallingBlossomEmotion : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.width = 26;
        Item.height = 20;
        Item.accessory = true;
        Item.rare = ItemRarityID.Green;
        Item.value = Item.sellPrice(0, 1, 70);
    }

    public override void UpdateAccessory(Player player, bool hideVisual) => player.GetModPlayer<NatureDodgePlayer>().IsFromAGreatClan = true;

        // Again helper method for this.
    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        float chance = Main.LocalPlayer.GetModPlayer<NatureDodgePlayer>().chance;

        string text = string.Format(Language.GetTextValue($"Mods.TheBindingOfRarria.Items.{Name}.Tooltip"), $"{(int)(chance * 100)}%");

        int index = tooltips.FindIndex(line => line.Name == "Tooltip0");
        if (index != -1)
        {
            text = text.Remove(text.LastIndexOf($"\n"));
            text = text.Remove(text.LastIndexOf($"\n"));
            tooltips[index].Text = text;
        }
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.AnkletoftheWind)
            .AddIngredient(ItemID.Wood, 50)
            .AddIngredient(ItemID.JungleSpores, 12)
            .AddIngredient(ItemID.Vine, 6)
            .AddTile(TileID.WorkBenches)
            .Register();
    }
}

public class NatureDodgePlayer : ModPlayer
{
    public float chance = 0f;
    public bool IsFromAGreatClan = false;
    public bool blocked = false;
    public Vector2 direction = Vector2.UnitY;
    public Vector2 position = Vector2.Zero;

    public override void ResetEffects() => IsFromAGreatClan = false;
    
    public override void PostUpdate()
    {
        chance = Math.Min(Math.Max(0.07f + (Player.moveSpeed - 1f) / 5, 0.07f), 0.21f);

        if (blocked)
        {
            position.SpawnDust(ModContent.DustType<PixellatedDustE98>(), 1.6f, 0.36f, Color.LightSeaGreen, 7, 25, 0.7f, direction.ToRotation() + PiOver2);
            blocked = false;
            direction = Vector2.UnitX;
            position = Vector2.Zero;
        }
    }
    public override bool FreeDodge(Player.HurtInfo info)
    {
        if (Main.rand.NextFloat() < chance && IsFromAGreatClan)
        {
            Player.immune = true;
            Player.immuneTime = 70;
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                ModPacket packet = ModContent.GetInstance<TheBindingOfRarria>().GetPacket();
                packet.Write((int)TheBindingOfRarria.PacketTypes.DustSpawn);
                packet.WriteVector2(position);
                packet.WriteVector2(direction);
                packet.Send();
            }
            else
            {
                blocked = true;
            }
            return true;
        }
        else
            return base.FreeDodge(info);
    }

    public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
    {
        if (IsFromAGreatClan)
        {
            position = Player.Center + Player.Center.DirectionTo(npc.Center) * Player.Hitbox.Size() / 2;
            direction = npc.Center.DirectionTo(Player.Center);

            if (direction == Vector2.Zero)
                direction = Vector2.UnitX;
        }
        else
        {
            base.ModifyHitByNPC(npc, ref modifiers);
        }
    }

    public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
    {
        if (IsFromAGreatClan)
        {
            position = Player.Center + Player.Center.DirectionTo(proj.Center) * Player.Hitbox.Size() / 2;
            direction = proj.velocity;

            if (direction == Vector2.Zero)
                direction = Vector2.UnitX;
        }
        else
        {
            base.ModifyHitByProjectile(proj, ref modifiers);
        }
    }
}