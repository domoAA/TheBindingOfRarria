using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Common.Helpers;
using TheBindingOfRarria.Content.Projectiles;

namespace TheBindingOfRarria.Content.Items;

public class DarkArts : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.width = 22;
        Item.height = 32;
        Item.accessory = true;
        Item.rare = ItemRarityID.Expert;
        Item.value = Item.sellPrice(0, 6);
        Item.expert = true;

    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<DarkDashPlayer>().HasDarkArts = true;
    }
}

public class DarkDashPlayer : ModPlayer
{
    public bool HasDarkArts = false;
    public const int DashCooldown = 300;
    public const int DashDuration = 30;
    public int DashDir = -1;
    public int counter = 0;
    public int keyTimer = 0;

    public override void HideDrawLayers(PlayerDrawSet drawInfo)
    {
        if (counter > 0)
        {
            foreach (var layer in PlayerDrawLayerLoader.Layers)
            {
                layer.Hide();
            }
        }
        base.HideDrawLayers(drawInfo);
    }
    public override void ResetEffects()
    {
        if (!HasDarkArts)
        {
            counter = Math.Min(counter, 0);
        }

        if (counter > 0 && (Player.grapCount > 0 || Player.controlJump || Player.controlUseItem || Player.controlUseTile))
        {
            counter = 0;
        }

        if (counter < 0 || (counter == 0 && keyTimer == 0))
            DashDir = -1;

        HasDarkArts = false;
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
        keyTimer = Math.Max(keyTimer - 1, 0);

        if (counter > 0)
        {
            Player.velocity = new Vector2(0, 10 - Math.Max(0, 3 - counter / 3) * 4).RotatedBy(DashDir * PiOver2);
            if (Player.velocity.Y == 0)
                Player.velocity.Y = 0.0001f;

            Player.gravity = 0;
            Player.legFrame.Y = 560;
            Player.legFrameCounter = 0;
            Player.bodyFrameCounter = 0;

            Player.immune = true;
            Player.SetImmuneTimeForAllTypes(counter);


            counter--;

            if (counter == 0)
                counter = -DashCooldown;
        }
        else if (counter < 0)
            counter++;
    }

    public override void ProcessTriggers(TriggersSet triggersSet)
    {
        if (counter == 0 && HasDarkArts && Main.myPlayer == Player.whoAmI)
        {
            if (Player.controlDown && Player.releaseDown)
            {
                keyTimer = 15;
                if (DashDir != 0)
                    DashDir = 0;

                else if (keyTimer > 0)
                    counter = DashDuration;
            }
            else if (Player.controlUp && Player.releaseUp)
            {
                keyTimer = 15;
                if (DashDir != 2)
                    DashDir = 2;

                else if (keyTimer > 0)
                    counter = DashDuration;
            }
            else if (Player.controlRight && Player.releaseRight)
            {
                keyTimer = 15;
                if (DashDir != 3)
                    DashDir = 3;

                else if (keyTimer > 0)
                    counter = DashDuration;
            }
            else if (Player.controlLeft && Player.releaseLeft)
            {
                keyTimer = 15;
                if (DashDir != 1)
                    DashDir = 1;

                else if (keyTimer > 0)
                    counter = DashDuration;
            }
            else
                counter = 0;



            if (counter > 0)
            {
                Player.SpawnProjectileIfNotSpawned(ModContent.ProjectileType<DarkDash>(), Player.GetSource_FromThis("Dark dash"));

                Player.mount.Dismount(Player);
                Player.RemoveAllGrapplingHooks();
            }
        }
    }

    public bool CanUseDash()
    {
        return HasDarkArts
            && Player.dashType == DashID.None
            && !Player.setSolar
            && !Player.mount.Active;
    }
}

public class DarkArtsNPCShop : GlobalNPC
{
    public override void ModifyShop(NPCShop shop)
    {
        if (shop.NpcType == NPCID.Wizard)
        {
            shop.Add(new Item(ModContent.ItemType<DarkArts>()), Condition.InGraveyard, Condition.InExpertMode);
        }
    }
}
