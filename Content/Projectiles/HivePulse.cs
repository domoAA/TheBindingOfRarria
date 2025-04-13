using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Projectiles;

public class HivePulse : ModProjectile
{
    public override string Texture => ContentPath + "Projectiles/" + Name;

    public override void SetDefaults()
    {
        Projectile.tileCollide = false;
        Projectile.penetrate = -1;
        Projectile.ignoreWater = true;
        Projectile.width = 60;
        Projectile.height = 60;
        Projectile.timeLeft = 120;
        Projectile.netImportant = true;
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = -1;
        Projectile.scale = 0.1f;
        Projectile.ai[0] = 0.1f;
        Projectile.hostile = true;
    }

    public override void AI()
    {
        Projectile.width = (int)(112 * Projectile.scale);
        Projectile.height = (int)(112 * Projectile.scale);

        Projectile.ai[0] += 0.05f;
        Projectile.scale = Projectile.ai[0];

        Projectile.Center = new Vector2(Projectile.ai[1], Projectile.ai[2]);

        Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, Terraria.ID.DustID.Honey).noGravity = true;
    }

    public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
    {
        modifiers.Cancel();
        if (target.GetModPlayer<HiveHealedPlayer>().HealedByHives.Contains(Projectile.identity)) {
            base.ModifyHitPlayer(target, ref modifiers);
            return; }

        target.Heal(Projectile.damage);
        target.GetModPlayer<HiveHealedPlayer>().HealedByHives.Add(Projectile.identity);
        base.ModifyHitPlayer(target, ref modifiers);
    }

    public override bool PreDraw(ref Color lightColor)
    {
            // Projectile.DrawWithTransparency(new Rectangle (0, 0, 256, 256), Color.Goldenrod, 1, 9, 1, 0.03f);
        float scale = Projectile.scale * 0.5f;

        Texture2D texture = TextureAssets.Projectile[Type].Value;

        Color color = Color.Goldenrod * (1f / 255f);

        for (int i = 0; i < 9; i++)
        {
            scale -= 0.03f;

            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, color with { A = 0 }, Projectile.rotation, texture.Size() * 0.5f, scale, SpriteEffects.None, 0);
        }

        return false;
    }
}

public class HiveHealedPlayer : ModPlayer
{
    public List<int> HealedByHives = [];
    private int counter = 130;

    public override void PostUpdate()
    {
        counter--;

        if (counter < 0)
        {
            counter = 130;
            if (HealedByHives.Count > 0)
                HealedByHives.RemoveAt(0);
        }
    }
}