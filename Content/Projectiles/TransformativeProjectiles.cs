using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Items;

namespace TheBindingOfRarria.Content.Projectiles.TransformativeProjectiles
{
    public class GlobalProjectileTransformation : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public bool Bouncy = false;
        public bool Spectral = false;
        public bool Godly = false;
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (!projectile.Transform(projectile.owner != Main.myPlayer || projectile.hostile || Main.LocalPlayer.HeldItem.consumable || Main.LocalPlayer.ChooseAmmo(Main.LocalPlayer.HeldItem).ammo == AmmoID.Gel))
                return;

            else if (projectile.DamageType == DamageClass.Ranged)
            {
                if (Main.player[projectile.owner].GetModPlayer<ProjectileTransformPlayer>().IsNailed)
                {
                    projectile.active = false;
                    Projectile.NewProjectile(source, projectile.position, projectile.velocity * 2, ModContent.ProjectileType<NailProjectile>(), (int)(projectile.damage * 0.67f), 2, projectile.owner);
                }
            }

            if (Main.LocalPlayer.GetModPlayer<PlanetPlayer>().planet == PlanetPlayer.Planets.Earth)
            {
                projectile.GetGlobalProjectile<OrbitalDebrisProjectile>().Orbiting = true;
                projectile.timeLeft += 180;
            }

            if (Main.LocalPlayer.GetModPlayer<PinnedPlayer>().Pinned && Main.rand.NextFloat() < (0.1f + Main.LocalPlayer.luck / 10))
            {
                Spectral = true;
                projectile.tileCollide = false;
                if (projectile.penetrate != -1)
                    projectile.penetrate += 2;
                projectile.usesIDStaticNPCImmunity = true;
                projectile.idStaticNPCHitCooldown = 15;
            }
            else if (Main.LocalPlayer.GetModPlayer<CementosPlayer>().Cementos)
            {
                Bouncy = true;
            }
        }
        public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {
            if (!Spectral && Bouncy && projectile.tileCollide)
            {
                // If the projectile hits the left or right side of the tile, reverse the X velocity
                if (Math.Abs(projectile.velocity.X - oldVelocity.X) > float.Epsilon)
                {
                    projectile.velocity.X = -oldVelocity.X;
                }

                // If the projectile hits the top or bottom side of the tile, reverse the Y velocity
                if (Math.Abs(projectile.velocity.Y - oldVelocity.Y) > float.Epsilon)
                {
                    projectile.velocity.Y = -oldVelocity.Y;
                }

                // borrowing code from EM, yessir

                projectile.timeLeft -= 60;
                return false;
            }
            else if (Spectral)
                return false;

            return base.OnTileCollide(projectile, oldVelocity);
        }
        public override bool PreDraw(Projectile projectile, ref Color lightColor)
        {
            if (Spectral)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
                Main.instance.GraphicsDevice.BlendState = BlendState.Additive;
                lightColor.A = 100;
            }
            return base.PreDraw(projectile, ref lightColor);
        }
    }
}