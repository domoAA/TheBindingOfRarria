using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Items;
using TheBindingOfRarria.Content.Dusts;

namespace TheBindingOfRarria.Content.Projectiles
{
    public class Cleave : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.height = 48;
            Projectile.width = 56;
            Projectile.maxPenetrate = -1;
            Projectile.timeLeft = 6;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 15;
        }
        public bool cleaved = false;
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }
        public override void AI()
        {
            if (Projectile.ai[0] < 4)
                Projectile.ai[0] += 0.3f;
            else
                Projectile.Kill();

            var a = new Vector2(Projectile.ai[1], Projectile.ai[2]);
            Projectile.rotation = Projectile.Center.DirectionFrom(a).ToRotation() + MathHelper.PiOver2;

            if (!cleaved) {
                Projectile.scale = float.Sqrt(Projectile.Center.Distance(a)) / 12;
                cleaved = true;
                var color = Color.DarkGray;

                a.SpawnDust(ModContent.DustType<PixellatedDustE98>(), 8f * Projectile.scale, 0.7f * Projectile.scale, color, 7, 35, 0.9f, a.DirectionFrom(Projectile.Center).ToRotation() + MathHelper.PiOver2, 2, -0.05f);

                var sound = SoundID.Item14;
                sound.Pitch -= 0.4f;
                sound.Volume *= 0.5f;
                SoundEngine.PlaySound(sound, a);
            }
            var b = (Projectile.Center - Main.screenPosition + Projectile.Center.DirectionFrom(a).RotatedBy(MathHelper.PiOver2) * 66 * Projectile.scale - new Vector2(Main.screenWidth / 2, Main.screenHeight / 2)) * Main.GameZoomTarget + new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
            var c = (Projectile.Center - Main.screenPosition + Projectile.Center.DirectionFrom(a).RotatedBy(-MathHelper.PiOver2) * 66 * Projectile.scale - new Vector2(Main.screenWidth / 2, Main.screenHeight / 2)) * Main.GameZoomTarget + new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);

            foreach (var target in Main.ActiveNPCs)
            {
                if (target.friendly || target.immune[Projectile.owner] > 0)
                    continue;

                for (int i = 4; i > 0; i--)
                {
                    Vector2 pos;
                    switch (i)
                    {
                        case 0: pos = new Vector2(target.position.X + target.width, target.position.Y + target.height); break;
                        case 1: pos = new Vector2(target.position.X + target.width, target.position.Y); break;
                        case 2: pos = new Vector2(target.position.X, target.position.Y + target.height); break;
                        default: pos = target.position; break;
                    }
                    if (LightCone.IsPointInTriangle(pos - Main.screenPosition - new Vector2(Main.screenWidth / 2, Main.screenHeight / 2) * Main.GameZoomTarget + new Vector2(Main.screenWidth / 2, Main.screenHeight / 2), a - Main.screenPosition - new Vector2(Main.screenWidth / 2, Main.screenHeight / 2) * Main.GameZoomTarget + new Vector2(Main.screenWidth / 2, Main.screenHeight / 2), b, c))
                    {
                        var info = target.CalculateHitInfo(Projectile.damage, Projectile.direction, Main.rand.Next(101) < Projectile.CritChance, 5, Projectile.DamageType, true, Main.player[Projectile.owner].luck);
                        target.StrikeNPC(info);
                        NetMessage.SendStrikeNPC(target, info);
                        target.immune[Projectile.owner] += 15;
                        break;
                    }
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            //Projectile.DrawWithTransparency(Projectile.Center.DirectionTo(a) * Projectile.Center.Distance(a) / 2, Projectile.MyTexture().Bounds, Color.LightYellow, 3, 3, 3, 0.05f);
            return false;
        }
    }
}