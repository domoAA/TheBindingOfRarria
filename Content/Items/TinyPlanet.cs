using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using TheBindingOfRarria.Content.Projectiles;

namespace TheBindingOfRarria.Content.Items;

public class TinyPlanet : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.width = 28;
        Item.height = 28;
        Item.rare = ItemRarityID.Orange;
        Item.value = Item.sellPrice(0, 0, 77, 49);
    }

    public override void UpdateAccessory(Player player, bool hideVisual) => player.GetModPlayer<PlanetPlayer>().planet = true;
    
    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.Meteorite, 100)
            .AddIngredient(ItemID.Hellstone, 100)
            .AddIngredient(ItemID.IceBlock, 200)
            .AddTile(TileID.SkyMill)
            .Register();
    }
}

public class PlanetPlayer : ModPlayer
{
    public bool planet = false;

    public override void ResetEffects() => planet = false;
}

public class OrbitingGlobalProjectile : GlobalProjectile
{
    public override bool InstancePerEntity => true;

    private bool orbit = false;

        // individual direction
    private int rotation = 1;

        // tracking speed
    private float speed = 0.9f;
    private float IndividualOffset = 0;

    public override void OnSpawn(Projectile projectile, IEntitySource source)
    {
        if (!projectile.CanBeReflected() && projectile.type != ModContent.ProjectileType<FlyingKunai>())
            orbit = false;

        else if (Main.player[projectile.owner].GetModPlayer<PlanetPlayer>().planet)
        {
            projectile.tileCollide = false;
            projectile.timeLeft = 300;

            orbit = true;
            projectile.damage = (int)(0.85f * projectile.damage);
            IndividualOffset = Main.rand.NextFloat() - 0.5f;
            rotation = Main.rand.NextBool() ? 1 : -1;
        }
    }

    public override void SendExtraAI(Projectile projectile, BitWriter bitWriter, BinaryWriter binaryWriter)
    {
        bitWriter.WriteBit(orbit);

        if (orbit)
        {
            binaryWriter.Write(IndividualOffset);
            binaryWriter.Write(rotation);
        }
    }

    public override void ReceiveExtraAI(Projectile projectile, BitReader bitReader, BinaryReader binaryReader)
    {
        orbit = bitReader.ReadBit();

        if (orbit)
        {
            IndividualOffset = binaryReader.ReadSingle();
            rotation = binaryReader.ReadInt32();
        }
    }

    public override void PostAI(Projectile projectile)
    {
        if (orbit)
        {
            projectile.tileCollide = false;

            Player owner = Main.player[projectile.owner];

            float r = projectile.Center.Distance(owner.Center);

            Vector2 velocity = projectile.velocity.RotatedBy(rotation);
            velocity.Normalize();

            if (r < 32)
                return;

            projectile.velocity += velocity * projectile.velocity.LengthSquared() / r;

            Vector2 gravity = 0.01f * projectile.Center.DirectionTo(owner.Center) * r;

            float offset = projectile.Center.DirectionTo(owner.Center).ToRotation() + rotation * PiOver2 - projectile.velocity.ToRotation();

            projectile.velocity = projectile.velocity.RotatedBy(offset);

                // making sure it stays at a certain distance
            if (r > 270 + IndividualOffset * 128)
                projectile.velocity += gravity / 2;

            else if (r < 250 + IndividualOffset * 128)
                projectile.velocity -= gravity;

            else
                projectile.velocity *= 1.01f;

                // acceleration
            speed += speed < 0.98f ? 0.001f : 0;

            projectile.velocity *= speed;
            projectile.velocity = projectile.velocity.SafeNormalize(Vector2.Zero) * Lerp(projectile.velocity.Length(), 8, 0.2f);
        }
    }
}