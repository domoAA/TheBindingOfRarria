using System;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace TheBindingOfRarria.Content
{
    public static class GeneralExtensions
    {
        public static T Find<T>(this ActiveEntityIterator<T> iterator, Func<T, bool> predicate) where T : Entity
        {
            foreach (var entity in iterator)
            {
                if (predicate(entity))
                    return entity;
            }
            return null;
        }
    }
    public static class PlayerExtensions
    {
        public static bool ReflectProjectiles(this Player player, Rectangle rect, DamageClass damageClass, float chance)
        {
            var target = Array.Find(Main.projectile, proj => proj.velocity != Vector2.Zero && proj.active && proj.hostile && proj.Colliding(proj.getRect(), rect));
            if (target != null)
            {
                if (new Random().NextDouble() >= chance)
                    return false;

                target.velocity = -target.velocity;
                target.hostile = false;
                target.friendly = true;
                target.reflected = true;
                target.DamageType = damageClass;
                return true;
            }
            return false;
        }
    }
}