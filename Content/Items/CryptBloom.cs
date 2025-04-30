using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Common.Helpers;

namespace TheBindingOfRarria.Content.Items;

public class CryptBloom : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.DefaultToAccessory(30, 28);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<CryptBloomPlayer>().HasCryptBloom = true;
    }
}
public class CryptBloomPlayer : ModPlayer
{
    public bool HasCryptBloom = false;
    public int killCounter = 0;

    public override void ResetEffects()
    {
        HasCryptBloom = false;
    }

    public void OnEnemyKill(Vector2 npcCenter)
    {
        if (HasCryptBloom)
        {
            killCounter++;
            if (killCounter >= 10)
            {
                Projectile.NewProjectile(Player.GetSource_FromThis(), npcCenter, Vector2.Zero, ModContent.ProjectileType<CryptBloomProjectile>(), 0, 0, Player.whoAmI);
                killCounter = 0;
            }
        }
    }
}

public class CryptBloomGlobalNPC : GlobalNPC
{
    public override void OnKill(NPC npc)
    {
        if (!npc.CountsAsACritter && !npc.friendly)
        {
            foreach (Player player in Main.player)
            {
                if (player.active && !player.dead && player.GetModPlayer<CryptBloomPlayer>().HasCryptBloom)
                {
                    float distanceSquared = Vector2.DistanceSquared(player.Center, npc.Center);
                    if (distanceSquared <= 800f * 800f)
                    {
                        player.GetModPlayer<CryptBloomPlayer>().OnEnemyKill(npc.Center);
                    }
                }
            }
        }
    }
}

public class CryptBloomProjectile : ModProjectile
{
    public override string Texture => ContentPath + "Projectiles/" + "CarefreePower";

    public override void SetDefaults()
    {
        Projectile.friendly = true;
        Projectile.timeLeft = 600;
        Projectile.tileCollide = false;
        Projectile.ignoreWater = true;
        Projectile.alpha = 100;
    }

    public override bool PreDraw(ref Color lightColor)
    {
        Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
        Vector2 drawPos = Projectile.Center - Main.screenPosition;
        float scale = 2.5f;

        texture.DrawWithTransparency(drawPos, scale, Color.Green, 200);

        return false;
    }

    public override void AI()
    {
        float radius = 80f;

        for (int i = 0; i < 3; i++)
        {
            float randomAngle = Main.rand.NextFloat(0, TwoPi);
            float randomDistance = Main.rand.NextFloat(0, radius);
            Vector2 offset = new Vector2(MathF.Cos(randomAngle), MathF.Sin(randomAngle)) * randomDistance;
            Dust dust = Dust.NewDustPerfect(Projectile.Center + offset, DustID.HealingPlus, Vector2.Zero, 100, new Color(0, 255, 100, 255), 1.5f);
            dust.noGravity = true;
            dust.velocity = Vector2.Zero;
        }

        Projectile.ai[0]++;
        if (Projectile.ai[0] >= 120)
        {
            Projectile.ai[0] = 0;
            float healRadiusSquared = radius * radius;
            foreach (Player player in Main.player)
            {
                if (player.active && !player.dead && Vector2.DistanceSquared(Projectile.Center, player.Center) <= healRadiusSquared)
                {
                    player.Heal(10);
                }
            }
        }
    }
}