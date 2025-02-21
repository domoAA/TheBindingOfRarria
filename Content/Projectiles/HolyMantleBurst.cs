using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Projectiles
{
    public class HolyMantleBurst : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 4;
        }
        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.damage = 0;
            Projectile.netImportant = true;
            Projectile.timeLeft = 16;
        }
        public override void AI()
        {

        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            base.DrawBehind(index, behindNPCsAndTiles, behindNPCs, behindProjectiles, overPlayers, overWiresUI);
            Main.instance.DrawCacheNPCsOverPlayers.Add(index);
            overPlayers.Add(index);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            var texture = Terraria.GameContent.TextureAssets.Projectile[Type].Value;

            int frame = Math.Max(0, 3 - Projectile.timeLeft / 4);
            var rect = texture.Frame(1, 4, 0, frame, 0, -2);

            byte dimming = (byte)(255 - Projectile.timeLeft * 0);

            var color = Color.SlateBlue;

            Projectile.scale = 2;
            Projectile.DrawWithTransparency(new Vector2(0, rect.Height + 2) * 0, rect, color, dimming, 1, 0, 0);

            return false;
        }
    }
}