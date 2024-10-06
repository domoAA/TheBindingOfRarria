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
    }
    public class StonePlayer : ModPlayer
    {
        public int StoneCD = 300;
        public override void UpdateEquips()
        {
            var index = Array.FindIndex(Player.armor, item => !item.social && item.type == ModContent.ItemType<ToothAndNail>());
            if (index != -1)
            {
                StoneCD--;
            }
            else
                StoneCD = 300;

            int time = Player.longInvince ? 60 : 0;
            switch (StoneCD)
            {
                case <= 0:
                    Player.AddImmuneTime(-1, 60 + time);
                    StoneCD = 360;
                    break;
                case > 300:
                    if(Array.FindIndex(Main.projectile, proj => proj.active && proj.timeLeft > 0 && proj.type == ModContent.ProjectileType<InvisibleNails>() && proj.owner == Player.whoAmI) == -1)
                        Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, new Vector2(0, 0), ModContent.ProjectileType<InvisibleNails>(), 40, 0, Player.whoAmI, time, 0, 0);
                    break;
                default:
                    if (StoneCD%30 == 0 && StoneCD < 100)
                    {
                        Player.GetImmuneAlpha(Color.White, 255);
                    }
                    break;
            }
        }
    }
}