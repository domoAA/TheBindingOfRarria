

using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using TheBindingOfRarria.Common.Helpers;
using TheBindingOfRarria.Content.Projectiles;

namespace TheBindingOfRarria.Content.Items;

public class ToothAndNail : ModItem
{
    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.height = 24;
        Item.width = 22;
    }
    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<ToothAndNailPlayer>().Tooth = Item;
    }
}
public class ToothAndNailPlayer : ModPlayer
{
    public Item Tooth = null;
    public override void ResetEffects()
    {
        Tooth = null;
    }
    public override bool CanBeHitByNPC(NPC npc, ref int cooldownSlot)
    {
        if (Player.OwnsProjectile(ModContent.ProjectileType<BlockBoulder>()))
            return false;

        return base.CanBeHitByNPC(npc, ref cooldownSlot);
    }
    public override bool CanBeHitByProjectile(Projectile proj)
    {
        if (Player.OwnsProjectile(ModContent.ProjectileType<BlockBoulder>()))
            return false;

        return base.CanBeHitByProjectile(proj);
    }
    public override void OnHurt(Player.HurtInfo info)
    {
        if (Tooth != null && Main.myPlayer == Player.whoAmI)
        {
            if (Main.rand.NextFloat() < 0.1f)
            {
                Projectile.NewProjectile(Player.GetSource_Accessory_OnHurt(Tooth, info.DamageSource, "ToothAndNail boulder"), Player.Center, Vector2.Zero, ModContent.ProjectileType<BlockBoulder>(), 20, 5, Player.whoAmI);
            }
            else
            {
                var total = Main.rand.Next(3, 7);
                var rot = Main.rand.NextFloat(TwoPi);

                for (int i = 0; i < total; i++)
                {
                    Projectile.NewProjectile(Player.GetSource_Accessory_OnHurt(Tooth, info.DamageSource, "ToothAndNail nails"), Player.Center, new Vector2(8).RotatedBy(rot + TwoPi / total * i), ProjectileID.Nail, 40, 3, Player.whoAmI);
                }
            }
        }
    }
}
public class NailCustomisation : GlobalProjectile
{
    public override bool InstancePerEntity => true;
    public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => entity.type == ProjectileID.Nail;
    public override void OnSpawn(Projectile projectile, IEntitySource source)
    {
        if (source != null && source.Context != null && source.Context == "ToothAndNail nails")
        {
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.scale *= 1.3f;
            projectile.penetrate = 1;
            SpawnUpdate = true;
        }
    }
    public bool SpawnUpdate = false;
    public override void SendExtraAI(Projectile projectile, BitWriter bitWriter, BinaryWriter binaryWriter)
    {
        bitWriter.WriteBit(SpawnUpdate);
        if (SpawnUpdate)
        {
            binaryWriter.Write(projectile.scale);
            projectile.penetrate = 1;
        }
        SpawnUpdate = false;
    }
    public override void ReceiveExtraAI(Projectile projectile, BitReader bitReader, BinaryReader binaryReader)
    {
        SpawnUpdate = bitReader.ReadBit();
        if (SpawnUpdate)
        {
            projectile.scale = binaryReader.ReadSingle();
            projectile.friendly = true;
            projectile.hostile = false;
            SpawnUpdate = false;
        }
    }
    public override bool PreDraw(Projectile projectile, ref Color lightColor)
    {
        if (projectile.friendly)
            lightColor.R = 0;

        return base.PreDraw(projectile, ref lightColor);
    }
}