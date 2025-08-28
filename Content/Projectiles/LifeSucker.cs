using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ModLoader;
using TheBindingOfRarria.Common.Helpers;
using TheBindingOfRarria.Common.Registries;
using TheBindingOfRarria.Common.Systems;
using TheBindingOfRarria.Content.Items;
using static TheBindingOfRarria.Common.Helpers.Helper;

namespace TheBindingOfRarria.Content.Projectiles;

public class LifeSucker : ModProjectile
{
    public override string Texture => ContentPath + "Projectiles/" + Name;

    public override void SetDefaults()
    {
        Projectile.ignoreWater = true;
        Projectile.tileCollide = false;
        Projectile.friendly = true;
        Projectile.penetrate = 2;
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = -1;
        Projectile.timeLeft = 50;
        Projectile.DamageType = DamageClass.Magic;
        Projectile.scale = 0.6f;

        Projectile.ai[1] = Main.rand.Next(3);
    }

    public override void AI()
    {
        if (Projectile.timeLeft == 38)
            SoundEngine.PlaySound(Sounds.DespairTrigger[Main.rand.Next(2)] with { Volume = 0.5f, PitchVariance = 0.2f, Type = SoundType.Sound }, Projectile.Center - Projectile.velocity);

        if (Projectile.ai[0] != 0 && Main.npc[(int)Projectile.ai[0]].active)
            Projectile.Center = Main.npc[(int)Projectile.ai[0]].Center;
        else
            Projectile.Kill();

        Projectile.velocity = Projectile.Center - Main.player[Projectile.owner].Center;

        Projectile.scale += Projectile.ai[2] == 0 ? 0.05f : -0.08f;

        foreach (var proj in Main.ActiveProjectiles)
        {
            if (proj.type == Type && proj.owner == Projectile.owner && proj.ai[0] == Projectile.ai[0] && proj.identity != Projectile.identity)
                proj.Kill();
        }

        Projectile.netUpdate = true;
    }

    public override bool ShouldUpdatePosition() => false;

    public override bool? CanHitNPC(NPC target)
    {
        if (target.whoAmI != Projectile.ai[0] || Projectile.scale < 1.2f || target.immortal)
            return false;

        return base.CanHitNPC(target);
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        Main.player[Projectile.owner].GetModPlayer<LifeSuckerPlayer>().heal += damageDone / 5;
        Projectile.ai[2] = 1;
    }

    public override bool PreDraw(ref Color lightColor)
    {
        Color color = Color.DeepSkyBlue;
        color.A = 210;

        if (Projectile.timeLeft < 38)
            color = Color.Red;

        Texture2D texture = TextureAssets.Projectile[Type].Value;

        float rotation = PiOver2;

        for (int i = 0; i < 4; i++)
        {
            rotation += PiOver2;
            Main.spriteBatch.DrawPixellated(texture, Projectile.Center - Main.screenPosition, null, 0.9f * new Vector2(0.4f * (-float.Pow(Projectile.timeLeft - 20, 2) * 0.001f + 0.4f), Projectile.scale * 0.1f) * 2, rotation, texture.Size() * 0.5f, color, SpriteEffects.None, PixellationSystem.RenderType.Additive, PixellationSystem.RenderLayer.Projectiles);
        }

        Main.spriteBatch.DrawPixellated(texture, Projectile.Center - (Projectile.velocity / 2) - Main.screenPosition, null, new Vector2(Projectile.velocity.Length() / 256, Projectile.scale * 0.1f) * 2, Projectile.velocity.ToRotation() + Pi, texture.Size() * 0.5f, color, SpriteEffects.None, PixellationSystem.RenderType.Additive, PixellationSystem.RenderLayer.Projectiles);

        return false;
    }
}