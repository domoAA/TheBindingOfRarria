using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Projectiles
{
    public class Extra98Bomb : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.timeLeft = 120;
            Projectile.rotation = (float)(new Random().NextDouble() * MathHelper.TwoPi);
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
        public override void AI()
        {
            if (Target  == null) 
                Projectile.Kill();
            else if (!Target.active)
                Projectile.Kill();
            else
            {
                Offset = new Vector2(Projectile.ai[1], Projectile.ai[2]);
                Projectile.Center = Target.Center - Offset;
            }
        }
        public override void OnKill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(0, 0), ModContent.ProjectileType<DetonatedButton>(), 5, 5, Projectile.owner);
        }
    }
    public class DetonatedButton : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Default;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 20;
            Projectile.timeLeft = 12;
            Projectile.friendly = true;
        }
        public override void AI()
        {
            if (++Projectile.frameCounter >= 3)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }
        }
    }
}