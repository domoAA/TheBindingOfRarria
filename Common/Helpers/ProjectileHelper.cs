using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Projectiles;

namespace TheBindingOfRarria.Common.Helpers;

public static partial class Helper
{
    public static void NewProjectileBetter(IEntitySource source, Vector2 pos, Vector2 vel, int type, float damage, float knockBack, int owner, Action<Projectile> modification = null)
    {
        if (Main.netMode == NetmodeID.MultiplayerClient && (owner < 0 || owner > 255))
            return;

        if (Main.myPlayer != owner)
            return;

        var projectile = Projectile.NewProjectileDirect(
            source, pos, vel,
            type, (int)damage,
            knockBack, owner
        );

        if (modification != null)
        {
            modification(projectile);
            projectile.netUpdate = true;
        }
    }

    public static T As<T>(this Projectile index) where T : ModProjectile
    {
        if (index.ModProjectile is T modProjectile)
            return modProjectile as T;
        else
        {
            index.Kill();
            return null;
        }
    }

    public static bool ReflectCheck(Projectile target, Predicate<Projectile> predicate) => predicate(target);

    public static bool ReflectCheck(this Projectile projectile, Projectile target) => new Predicate<Projectile>(
        proj =>
        proj.velocity.LengthSquared() > 1
        && proj.GetGlobalProjectile<ReflectableGlobalProjectile>().ReflectableSeed > 0
        && proj.type != projectile.type
        && proj.hostile
        && projectile.Colliding(proj.getRect(), projectile.getRect()))(target);

    public static void ReflectProjectiles(this Projectile projectile, float chance = 1f)
    {
        foreach (var proj in Main.ActiveProjectiles)
        {
            Projectile target = null;
            if (projectile.ReflectCheck(proj))
                target = proj;

            if (target != null)
            {
                bool reflected = target.GetGlobalProjectile<ReflectableGlobalProjectile>().ReflectableSeed < chance;

                projectile.GetGlobalProjectile<ReflectableGlobalProjectile>().ReflectableSeed = 0;

                if (!reflected)
                    continue;

                target.GetReflected();

                if (projectile.type == ModContent.ProjectileType<CircleOfLight>())
                    projectile.ai[0] = 1;
            }
        }
    }

    public static void GetReflected(this Projectile projectile)
    {
        projectile.damage *= Main.masterMode ? 6 : Main.expertMode ? 4 : 2;
        projectile.velocity = -projectile.velocity;
        projectile.hostile = false;
        projectile.friendly = true;
    }

    public static void GetSlowed(this Projectile projectile, TheBindingOfRarria.State state, int duration)
    {
        if (projectile.GetGlobalProjectile<SlowedGlobalProjectile>().Slowed.Item1 == state)
            return;

        if (Main.netMode == NetmodeID.MultiplayerClient)
        {
            ModPacket packet = ModContent.GetInstance<TheBindingOfRarria>().GetPacket();
            packet.Write((int)TheBindingOfRarria.PacketTypes.EntitySlow);
            packet.Write((int)state);
            packet.Write(duration);
            packet.Write(true);
            packet.Write(projectile.identity);
            packet.Send();
        }
        else
            projectile.GetGlobalProjectile<SlowedGlobalProjectile>().Slowed = (state, duration);
    }

    /// <summary>
    /// This method sets the center of the projectile it was called upon to the center of its owner.
    /// </summary>
    public static void CenteredOnPlayer(this Projectile projectile)
    {
        Vector2 position = projectile.Center;

        if (Main.player[projectile.owner].active)
            position = Main.player[projectile.owner].Center;

        projectile.Center = position;
    }

    /// <summary>
    /// This method repells projectiles that collide with the hitbox of the projectile it was called upon in the direction opposite of the projectile's center.
    /// </summary>
    public static void ProjectileRepelling(this Projectile proj)
    {
        foreach (Projectile projectile in Main.ActiveProjectiles)
        {
            // Sure.
            Predicate<Projectile> predicate = proj.hostile ? new Predicate<Projectile>(p => p != null && p.CanBeReflected() && p.Colliding(p.getRect(), proj.getRect())) : new Predicate<Projectile>(p => p != null && p.active && p.hostile && p.type != proj.type && p.velocity.LengthSquared() > 1 && p.Colliding(p.getRect(), proj.getRect()));
            if (ReflectCheck(projectile, predicate))
            {
                projectile.velocity += proj.hostile ? projectile.Center.DirectionFrom(proj.Center) * 4 : projectile.Center.DirectionFrom(proj.Center);
                projectile.netUpdate = true;
            }
        }
    }

    public static void OrbitingPlayer(this Projectile projectile, float visualRotation, float r, float rotation)
    {
        projectile.rotation = rotation - PiOver2 * visualRotation;

        Vector2 desiredPosition = rotation.ToRotationVector2() * r;

        projectile.Center = Main.player[projectile.owner].Center + desiredPosition;
        projectile.netUpdate = true;
    }

    public static bool Transform(this Projectile projectile, bool condition)
    {
        if (condition)
            return false;

        else if (projectile.damage > 0)
            return true;

        return false;
    }
}