using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Common.Helpers;
using TheBindingOfRarria.Common.Systems;
using TheBindingOfRarria.Content.Dusts;

namespace TheBindingOfRarria.Content.Projectiles;

public class Extra98Bomb : ModProjectile
{
    public override string Texture => ContentPath + "Projectiles/" + Name;

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

        for (int i = 7; i >= 0; i--)
            bomba[i] = Vector2.One.RotatedBy(PiOver4 * (i + 1) + Main.rand.NextFloat(-Pi / 10, Pi / 10));
    }

    private NPC Target
    {
        get => Projectile.ai[0] == 0 ? null : Main.npc[(int)Projectile.ai[0]];
        set => Projectile.ai[0] = value == null ? 0 : value.whoAmI;
    }

    private Vector2 Offset = new(0, 0);
    private readonly Vector2[] bomba = new Vector2[8];

    public override void AI()
    {
        if (Target == null || !Target.active)
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
            Projectile.Center.SpawnDust(bomba, ModContent.DustType<PixellatedDustE98>(), 2.2f, Main.rand.NextFloat(0.8f, 1.6f), color, 2, -1.12f, PiOver4);
            
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

        return null;
    }

    public override bool PreDraw(ref Color lightColor)
    {
            // DrawPixellated(lightColor, 220, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, PixellationSystem.RenderType.Additive);

        Projectile.scale = 0.5f;

        Texture2D texture = TextureAssets.Projectile[Type].Value;

        if (Projectile.timeLeft > 12)
            Main.spriteBatch.DrawPixellated(texture, Projectile.Center - Main.screenPosition, null, Projectile.scale, Projectile.rotation, texture.Size() * 0.5f, lightColor, PixellationSystem.RenderType.Additive);
        
        return false;
    }
}