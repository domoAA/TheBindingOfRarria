using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items;

public class DarkArts : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.DefaultToAccessory(22, 32);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<DarkArtsPlayer>().HasDarkArts = true;
    }
}

public partial class DarkArtsPlayer : ModPlayer
{
    public bool HasDarkArts = false;
    public const int DashDown = 0;
    public const int DashUp = 1;
    public const int DashRight = 2;
    public const int DashLeft = 3;
    public const int DashCooldown = 300;
    public const int DashDuration = 35;
    public int DashDir = -1;
    public int DashDelay = 0;
    public int DashTimer = 0;

    public override void ResetEffects()
    {
        HasDarkArts = false;
        if (Player.controlDown && Player.releaseDown && Player.doubleTapCardinalTimer[DashDown] < 15)
        {
            DashDir = DashDown;
        }
        else if (Player.controlUp && Player.releaseUp && Player.doubleTapCardinalTimer[DashUp] < 15)
        {
            DashDir = DashUp;
        }
        else if (Player.controlRight && Player.releaseRight && Player.doubleTapCardinalTimer[DashRight] < 15 && Player.doubleTapCardinalTimer[DashLeft] == 0)
        {
            DashDir = DashRight;
        }
        else if (Player.controlLeft && Player.releaseLeft && Player.doubleTapCardinalTimer[DashLeft] < 15 && Player.doubleTapCardinalTimer[DashRight] == 0)
        {
            DashDir = DashLeft;
        }
        else
        {
            DashDir = -1;
        }
    }

    public override void HideDrawLayers(PlayerDrawSet drawInfo)
    {
        if (DashTimer > 0)
        {
            foreach (var layer in PlayerDrawLayerLoader.Layers)
            {
                layer.Hide();
            }
        }
        base.HideDrawLayers(drawInfo);
    }

    public override void PreUpdateMovement()
    {
        if (CanUseDash() && DashDir != -1 && DashDelay == 0)
        {
            DoADash();
        }
        if (DashDelay > 0)
            DashDelay--;
        if (DashTimer > 0)
        {
            DashTimer--;
            MidDashEffects();
        }
    }

    public void MidDashEffects()
    {
        if (DashTimer % 2 == 0)
        {
            Vector2 offset = new Vector2(Main.rand.NextFloat(-10f, 10f), Main.rand.NextFloat(-30f, 30f));
            Dust dust = Dust.NewDustPerfect(Player.Center + offset, DustID.Asphalt, Vector2.Zero, 0, Color.Black, 3f);
            dust.noGravity = true;
            dust.velocity = Vector2.Zero;
        }
    }

    public void DoADash()
    {
        int dashSpeed = 11;
        int totalDirections = 8;
        Vector2[] possibleVelocities = new Vector2[totalDirections];
        for (int i = 0; i < totalDirections; i++)
            possibleVelocities[i] = -Vector2.UnitY.RotatedBy(MathHelper.TwoPi * i / totalDirections) * dashSpeed;

        switch (DashDir)
        {
            case DashUp:
                Player.velocity = possibleVelocities[0];
                break;
            case DashLeft:
                Player.velocity = new Vector2(possibleVelocities[6].X, Player.velocity.Y);
                break;
            case -1:
                break;
            case DashRight:
                Player.velocity = new Vector2(possibleVelocities[2].X, Player.velocity.Y);
                break;
            case DashDown:
                Player.velocity = possibleVelocities[4];
                break;
        }

        Point upwardTilePoint = (Player.Center + new Vector2(MathHelper.Clamp((int)DashDir, -1f, 1f) * Player.width / 2 + 2, Player.gravDir * -Player.height / 2f + Player.gravDir * 2f)).ToTileCoordinates();
        Point aheadTilePoint = (Player.Center + new Vector2(MathHelper.Clamp((int)DashDir, -1f, 1f) * Player.width / 2 + 2, 0f)).ToTileCoordinates();
        if (WorldGen.SolidOrSlopedTile(upwardTilePoint.X, upwardTilePoint.Y) || WorldGen.SolidOrSlopedTile(aheadTilePoint.X, aheadTilePoint.Y))
            Player.velocity.X /= 2f;

        DashDelay = DashCooldown;
        DashTimer = DashDuration;
        Player.immune = true;
        Player.immuneTime = DashDuration;
    }

    private bool CanUseDash()
    {
        return HasDarkArts
            && Player.dashType == DashID.None
            && !Player.setSolar
            && !Player.mount.Active;
    }
}