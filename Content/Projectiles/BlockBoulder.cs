using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using TheBindingOfRarria.Common.Helpers;

namespace TheBindingOfRarria.Content.Projectiles;

public class BlockBoulder : ModProjectile
{
    public override string Texture => ContentPath + "Projectiles/" + Name;

    public override void SetDefaults()
    {
        Projectile.tileCollide = false;
        Projectile.penetrate = -1;
        Projectile.ignoreWater = true;
        Projectile.friendly = true;
        Projectile.width = 64;
        Projectile.height = 64;
        Projectile.timeLeft = 120;
        Projectile.netImportant = true;
    }

    public override void AI()
    {
        var owner = Main.player[Projectile.owner];

        if (owner.active) 
        { 
            owner.Center = Projectile.Center;
            owner.mount.Dismount(owner);
            owner.RemoveAllGrapplingHooks();
            owner.velocity *= 0;
        }
        else
            Projectile.Kill();

        Projectile.ReflectProjectiles();
    }

    public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
    {
        Main.instance.DrawCacheNPCsOverPlayers.Add(index);

        overPlayers.Add(index);
    }

    public override bool PreDraw(ref Color lightColor)
    {
        Texture2D texture = TextureAssets.Projectile[Type].Value;

            // Why in predraw ??????
        Projectile.scale = 2;
        Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, texture.Size() * 0.5f, Projectile.scale, SpriteEffects.None);

        return false;
    }
}