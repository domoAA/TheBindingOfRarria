using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Items;

namespace TheBindingOfRarria.Content.Projectiles
{
    public class Extra98Bomb : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.CritChance = (int)(Main.player[Projectile.owner].GetCritChance(DamageClass.Generic));
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 20;
            Projectile.width = 80;
            Projectile.height = 80;
            Projectile.timeLeft = 132;
            for (int i = 7; i > 0; i--)
            {
                bomba[i] = Vector2.One.RotatedBy(MathHelper.PiOver4 * (i + 1) + Main.rand.NextFloat(-MathHelper.Pi / 10, MathHelper.Pi / 10));
            }
        }
        private NPC Target
        {
            get => Projectile.ai[0] == 0 ? null : Main.npc[(int)Projectile.ai[0]];
            set
            {
                Projectile.ai[0] = value == null ? 0 : value.whoAmI;
            }
        }
        private Vector2 Offset = new(0, 0);
        public Vector2[] bomba = new Vector2[8];
        public override void AI()
        {
            if (Target  == null || !Target.active)
                Projectile.Kill();
            else if (Projectile.timeLeft > 12)
            {
                Offset = new Vector2(Projectile.ai[1], Projectile.ai[2]);
                Projectile.Center = Target.Center - Offset;
                Projectile.netUpdate = true;
            }
            else if (Projectile.timeLeft == 12 && bomba != null)
            {
                var color = Color.White;
                Projectile.Center.SpawnDust(bomba, ModContent.DustType<PixelatedDustParticle>(), 2, Main.rand.NextFloat(0.7f, 1.1f), color, 2, -0.2f, MathHelper.PiOver4);
                
                var sound = SoundID.Item14;
                sound.Pitch += 0.8f;
                sound.Volume *= 0.3f;
                SoundEngine.PlaySound(sound, Projectile.Center);
            }
        }
        public override bool? CanHitNPC(NPC target)
        {
            if (Projectile.timeLeft > 4)
                return false;

            return base.CanHitNPC(target);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.scale = 0.5f;
            if (Projectile.timeLeft > 12)
                Projectile.DrawPixellated(lightColor, 220, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, PixellationSystem.RenderType.Additive);
            
            return false;
        }
    }
}