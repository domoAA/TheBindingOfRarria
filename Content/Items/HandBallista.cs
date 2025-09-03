
using System.IO;
using Terraria.ModLoader.IO;
using TheBindingOfRarria.Common.Helpers;
using TheBindingOfRarria.Content.Projectiles;

namespace TheBindingOfRarria.Content.Items;

public class HandBallista : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.height = 100;
        Item.width = 100;
        Item.damage = 135;
        Item.DamageType = DamageClass.Ranged;
        Item.noUseGraphic = true;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.noMelee = true;
        Item.useTime = 65;
        Item.useAnimation = 65;
        Item.useTurn = false;
        Item.value = Item.sellPrice(gold: 1, silver: 60);
        Item.useAmmo = AmmoID.Arrow;
        Item.rare = ItemRarityID.Pink;
        Item.knockBack = 5f;
    }

    public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
    {
        if (type == ProjectileID.WoodenArrowFriendly)
            type = ProjectileID.DD2BallistraProj;
    }

    public override void UseAnimation(Player player)
    {
        var mousePos = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
        var proj = Projectile.NewProjectileDirect(player.GetSource_ItemUse_WithPotentialAmmo(Item, AmmoID.Arrow), player.Center + player.Center.DirectionTo(mousePos) * 50 , player.Center.DirectionTo(mousePos), ModContent.ProjectileType<HandBallistaProj>(), 0, 0);
        
    }
}

public class GiantBallistaBolt : GlobalProjectile
{
    public override bool InstancePerEntity => true;

    public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
    {
        return entity.type == ProjectileID.DD2BallistraProj;
    }

    public bool Giant = false;

    public override void SendExtraAI(Projectile projectile, BitWriter bitWriter, BinaryWriter binaryWriter)
    {
        bitWriter.WriteBit(Giant);
    }

    public override void ReceiveExtraAI(Projectile projectile, BitReader bitReader, BinaryReader binaryReader)
    {
        Giant = bitReader.ReadBit();
    }
}