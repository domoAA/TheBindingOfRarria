using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using TheBindingOfRarria.Common.Helpers;
using TheBindingOfRarria.Common.Systems;
using TheBindingOfRarria.Content.Buffs;
using TheBindingOfRarria.Content.Buffs.Debuffs;
using TheBindingOfRarria.Content.Items;

namespace TheBindingOfRarria.Content.Projectiles;

public class GhizaWheelSaw : ModProjectile
{
    public override string Texture => ContentPath + "Projectiles/" + Name;

    public float Radius { get; private set; } = 36;

    public override void SetDefaults()
    {
        //for this example, width/height wont matter too much
        Projectile.width = Projectile.height = 72;

        //custom swords should not break on hit or go through tiles
        Projectile.penetrate = -1;
        Projectile.ownerHitCheck = true;
        Projectile.tileCollide = false;
        Projectile.ignoreWater = true;

        //ensure the projectile only hits once
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = 10;

        Projectile.timeLeft = 55;
        Projectile.scale = 1f;
        Projectile.friendly = true;
        Projectile.DamageType = DamageClass.MeleeNoSpeed;
    }

    public override bool? CanCutTiles()
    {
        return false;
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (!hit.Crit)
            return;

        var owner = Main.player[Projectile.owner];

        var power = 2;
        owner.GetModPlayer<TemporaryLifePlayer>().bonuses.Add(new LifeBonus(power, 1200, p => !p.active, "Ghiza"));

        if (target.active)
            target.AddBuff(ModContent.BuffType<Siphoned>(), 1200);

        var percent = target.life / (float)target.lifeMax;

        target.lifeMax -= power;
        target.life = (int)Math.Floor(percent * target.lifeMax);

        target.GetGlobalNPC<SiphonedNPC>().SiphonedLife += power;
    }

    public override void AI()
    {
        Projectile.CenteredOnPlayer();
        Projectile.Center += Main.MouseWorld.DirectionFrom(Main.player[Projectile.owner].Center) * 72;


        Projectile.rotation -= 0.3f;


        var owner = Main.player[Projectile.owner];
        var scale = owner.GetAdjustedItemScale(owner.HeldItem);
        Radius = 36 * scale;
    }

    public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
    {
        float point = 0f;

        Vector2 length = new Vector2(Radius / 2).RotatedBy(Projectile.rotation);

        if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center + length * 0.05f, Projectile.Center + length, 20, ref point))
            return true;

        return false;
    }

    public override bool PreDraw(ref Color lightColor)
    {
        var owner = Main.player[Projectile.owner];


        var texture = TextureAssets.Projectile[Type];

        var origin = new Vector2(33, 31);

        Main.spriteBatch.Draw(texture.Value, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, origin, Projectile.scale * Radius / 36f, SpriteEffects.None, 0);

        texture = TextureAssets.Item[ModContent.ItemType<GhizaWheel>()];

        var rot = Projectile.Center.DirectionFrom(owner.Center).ToRotation() + PiOver4 * 1.1f;

        Main.spriteBatch.Draw(texture.Value, (Projectile.Center - owner.Center) / 3.5f + owner.Center - Main.screenPosition, null, lightColor, rot, texture.Size() / 2, Projectile.scale, SpriteEffects.None, 0);

        return false;
    }
}