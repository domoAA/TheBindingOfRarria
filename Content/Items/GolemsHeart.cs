using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using TheBindingOfRarria.Content.Projectiles;

namespace TheBindingOfRarria.Content.Items
{
    public class GolemsHeart : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 24;
            Item.accessory = true;
            Item.defense = 1;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<GolemPlayer>().IsRockHard = true;
        }
        public override int ChoosePrefix(UnifiedRandom rand)
        {
            var chance = rand.NextFloat() / 2;
            if (chance < 0.1f)
                return PrefixID.Warding;
            else if (chance < 0.2f)
                return PrefixID.Armored;
            else if (chance < 0.3f)
                return PrefixID.Hard;
            else 
                return base.ChoosePrefix(rand);
        }
    }
    public class GolemPlayer : ModPlayer
    {
        public bool IsRockHard = false;
        public override void ResetEffects()
        {
            IsRockHard = false;
        }
        public override void PostUpdateEquips()
        {
            var index = Array.FindIndex(Main.projectile, proj => proj.active && proj.type == ModContent.ProjectileType<RepellingPulse>() && proj.owner == Player.whoAmI);
            if (!IsRockHard)
            {
                if (index != -1)
                    Main.projectile[index].Kill();
            }
            else
            {
                if (index == -1)
                    Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Microsoft.Xna.Framework.Vector2.Zero, ModContent.ProjectileType<RepellingPulse>(), 0, 0, Player.whoAmI);
            }
        }
    }
}