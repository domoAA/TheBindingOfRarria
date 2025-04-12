using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Dusts;

namespace TheBindingOfRarria.Content.Items;

[AutoloadEquip(EquipType.Waist)]
public class FocusCrystal : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.width = 22;
        Item.height = 22;
        Item.value = Item.sellPrice(silver: 15);
        Item.rare = ItemRarityID.Green;
        Item.accessory = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<FocusCrystalPlayer>().Active = true;
        player.GetModPlayer<FocusCrystalPlayer>().Visuals = !hideVisual;
    }
}

public class FocusCrystalPlayer : ModPlayer
{
    private const float Range = 12f * 16f;
    private const float DamageMultiplier = 1.2f;

    public bool Active;
    public bool Visuals;

    public override void ResetEffects()
    {
        Active = false;
        Visuals = false;
    }

    public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
    {
        if (!Active || !target.WithinRange(Player.Center, Range))
            return;

        modifiers.SourceDamage *= DamageMultiplier;

        for (int i = 0; i < 4; i++)
        {
            Dust newDust = Dust.NewDustDirect(target.position, target.width, target.height, ModContent.DustType<FocusCrystalAuraDust>());
            newDust.velocity = Main.rand.NextVector2Circular(5f, 5f);
            newDust.scale = Main.rand.NextFloat(1f, 1.5f);
        }
    }

    public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
    {
        if (!Visuals)
            return;

        for (int i = 0; i < 3; i++)
        {
            Vector2 dustPositionOffset = Main.rand.NextVector2CircularEdge(Range, Range);
            Vector2 dustPosition = drawInfo.drawPlayer.MountedCenter + dustPositionOffset;
            Point dustTileCoordinates = dustPosition.ToTileCoordinates();

            Tile tile = Framing.GetTileSafely(dustTileCoordinates);
            if (tile.HasTile && WorldGen.SolidTile(dustTileCoordinates.X, dustTileCoordinates.Y))
                continue;

            Dust newDust = Dust.NewDustPerfect(dustPosition, ModContent.DustType<FocusCrystalAuraDust>());

                // 1/2 chance of being edge dust or zoomy dust
            Vector2 dustVelocity = Main.rand.NextBool() ? Vector2.Zero : dustPosition.DirectionTo(drawInfo.drawPlayer.MountedCenter) * Main.rand.NextFloat(0.4f, 1.8f);
            newDust.velocity = dustVelocity;
            newDust.alpha = 100;
            newDust.customData = drawInfo.drawPlayer;

            drawInfo.DustCache.Add(newDust.dustIndex);
        }
    }
}

public class FocusCrystalGlobalProjectile : GlobalProjectile
{
    public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => entity.type == ProjectileID.Geode;

    public override void OnKill(Projectile projectile, int timeLeft)
    {
        if (Main.myPlayer != projectile.owner || Main.rand.NextBool(29, 30))
            return;

        int itemIndex = Item.NewItem(projectile.GetSource_Loot(), projectile.Hitbox, ModContent.ItemType<FocusCrystal>());
        if (Main.netMode == NetmodeID.MultiplayerClient)
            NetMessage.SendData(MessageID.SyncItem, number: itemIndex, number2: 1f);
    }
}