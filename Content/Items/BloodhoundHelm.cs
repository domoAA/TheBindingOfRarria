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
public class BloodhoundHelm : ModItem
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
        Item.value = Item.sellPrice(0, 2, 0, 0);
        Item.master = true;
    }

    public override void UpdateEquip(Player player)
    {
        player.GetAttackSpeed(DamageClass.Melee) += 0.05f;
    }

    public override bool IsArmorSet(Item head, Item body, Item legs)
    {
        return head.type == Item.type && body.type == ModContent.ItemType<BloodhoundPlatemail>() && legs.type == ModContent.ItemType<BloodhoundGreaves>();
    }

    public override void UpdateArmorSet(Player player)
    {
        string key = KeybindSystem.BloodhoundDashKey.GetAssignedKeys().FirstOrDefault();
        if (key == "" || key == null)
            key = "C";
        player.setBonus = string.Format(Language.GetTextValue($"Mods.TheBindingOfRarria.Items.{Name}.SetBonus"), key);
        player.GetModPlayer<BloodhoundDashPlayer>().Bloodhound = true;
    }

    public override void ArmorSetShadows(Player player)
    {
        player.armorEffectDrawShadow = true;
    }
}

public class BloodhoundItemsShop : GlobalNPC
{
    public override void ModifyActiveShop(NPC npc, string shopName, Item[] items)
    {
        if (npc.type == NPCID.TravellingMerchant && Main.masterMode)
        {
            var index = -1;

            for (int a = 0; a < 3; a++)
                for (int i = items.Length - 1; i > index + 1; i--)
                    items[i] = items[i - 1];

            items[index + 1] = new Item(ModContent.ItemType<BloodhoundHelm>());
            items[index + 2] = new Item(ModContent.ItemType<BloodhoundPlatemail>());
            items[index + 3] = new Item(ModContent.ItemType<BloodhoundGreaves>());
        }
    }
}

public class BloodhoundDashPlayer : ModPlayer
{
    public bool Bloodhound = false;
    public const int DashCooldown = 420;
    public const int DashDuration = 10;
    public float DashDir = -1;
    public int counter = 0;

    public override void ResetEffects()
    {
        if (!Bloodhound)
        {
            counter = Math.Min(counter, 0);
        }

        if (counter <= 0)
            DashDir = 0;

        Bloodhound = false;
    }

    public override bool CanBeHitByNPC(NPC npc, ref int cooldownSlot)
    {
        if (counter > 0)
            return false;

        return base.CanBeHitByNPC(npc, ref cooldownSlot);
    }

    public override bool CanBeHitByProjectile(Projectile proj)
    {
        if (counter > 0)
            return false;

        return base.CanBeHitByProjectile(proj);
    }

    public override void PreUpdateMovement()
    {
        if (counter > 0)
        {
            Player.velocity = new Vector2(29 - Math.Max(0, 6 - counter) * 5, 0).RotatedBy(DashDir);
            if (Player.velocity.Y == 0)
                Player.velocity.Y = 0.0001f;

            Player.gravity = 0;
            Player.legFrame.Y = 560;
            Player.legFrameCounter = 0;
            Player.bodyFrameCounter = 0;


            counter--;

            if (counter == 0)
                counter = -DashCooldown;
        }
        else if (counter < 0)
        {
            counter++;
            if (counter == 0)
                SoundEngine.PlaySound(SoundID.MaxMana with { Pitch = 0.3f }, Player.Center);
        }
    }

    public override void ProcessTriggers(TriggersSet triggersSet)
    {
        if (counter == 0 && Bloodhound && Main.myPlayer == Player.whoAmI)
        {
            if (KeybindSystem.BloodhoundDashKey.JustPressed || (KeybindSystem.BloodhoundDashKey.GetAssignedKeys().FirstOrDefault() == null && Main.keyState.IsKeyDown(Keys.C)))
            {
                counter = DashDuration;
                DashDir = Player.Center.DirectionTo(Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY)).ToRotation();
            }
            else
                counter = 0;

            if (counter > 0)
            {
                Player.mount.Dismount(Player);
                Player.RemoveAllGrapplingHooks();
            }
        }
    }

    public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
    {
        if (counter <= 0)
            return;

        if (drawInfo.shadow == 0)
        {
            a = 100;
            r *= 200 / 255f;
            g *= 200 / 255f;
            b *= 200 / 255f;
        }
    }
}