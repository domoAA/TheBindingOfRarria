using System;
using Terraria;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.Graphics.Effects;
using ReLogic.Content;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TheBindingOfRarria.Content.Items
{
    public class ToothAndNail : ModItem
    {
        public override void SetStaticDefaults()
        {
            if (!Main.dedServ)
            {
                //Asset<Effect> filterShader = Mod.Assets.Request<Effect>("Common/PlayerFlashingShader");
                //Filters.Scene["WhiteFlashing"] = new Filter(new ScreenShaderData(filterShader, "ArmorTint"), EffectPriority.Medium);
            }
        }
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 24;
            Item.accessory = true;
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
                    if (StoneCD % 30 == 0 && StoneCD < 100)
                        if (Main.netMode != NetmodeID.Server) // This all needs to happen client-side!
                        {
                            //Filters.Scene.Activate("WhiteFlashing", Player.Center).GetShader().UseColor(Color.White).UseTargetPosition(Player.Center); //.UseImage()
                        }
                    //else if(Main.netMode != NetmodeID.Server && Filters.Scene["Flashing"].IsActive())
                    //{
                        //Filters.Scene["WhiteFlashing"].Deactivate();
                    //}
                    break;
            }
        }
    }
}