using System;
using Terraria;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Projectiles;
using Microsoft.Xna.Framework;

namespace TheBindingOfRarria.Content.Items
{
    public class ToothAndNail : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 24;
            Item.accessory = true;
            Item.defense = 300;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<StonePlayer>().IsSigma = true;
        }
    }
    public class StonePlayer : ModPlayer
    {
        public bool IsSigma = false;
        public int StoneCD = 300;
        public override void ResetEffects()
        {
            IsSigma = false;
        }
        public override void UpdateEquips()
        {
            if (IsSigma)
            {
                StoneCD--;
            }
            else
                StoneCD = 300;

            int time = Player.longInvince ? 60 : 0;
            switch (StoneCD)
            {
                case <= 0:
                    Player.immune = true;
                    Player.immuneTime += 60 + time;
                    StoneCD = 360 + time;
                    break;
                case > 300:
                    if(Array.FindIndex(Main.projectile, proj => proj.active && proj.timeLeft > 0 && proj.type == ModContent.ProjectileType<InvisibleNails>() && proj.owner == Player.whoAmI) == -1)
                        Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, new Vector2(0, 0), ModContent.ProjectileType<InvisibleNails>(), 40, 0, Player.whoAmI, time, 0, 0);
                    break;
                default:
                    if (StoneCD%30 == 0 && StoneCD < 100)
                    {
                        Player.immuneAlpha = 255;
                    }
                    break;
            }
        }
    }
}