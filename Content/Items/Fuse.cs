using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Projectiles;

namespace TheBindingOfRarria.Content.Items
{
    public class Fuse : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 20;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<ExplotaroPlayer>().IsBomboclat = true;
        }
    }
    public class ExplotaroPlayer : ModPlayer
    {
        public bool IsBomboclat = false;
        public override void ResetEffects()
        {
            IsBomboclat = false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (IsBomboclat)
            {
                var index = 0;
                var power = 0f;
                if (hit.Crit)
                {
                    if (target.HasBuff(BuffID.OnFire3))
                    {
                        index = target.FindBuffIndex(BuffID.OnFire3);
                        power = 0.4f;
                    }

                    else if (target.HasBuff(BuffID.OnFire))
                    {
                        index = target.FindBuffIndex(BuffID.OnFire);
                        power = 0.1f;
                    }

                    if (power == 0)
                        return;

                    int damag = (int)(target.buffTime[index] * power) + 1;
                    if (Main.myPlayer != Player.whoAmI)
                        return;
                    Projectile.NewProjectile(Player.GetSource_OnHit(target), target.Center, Vector2.Zero, ModContent.ProjectileType<FireExplotaro>(), damag, 1.5f, Player.whoAmI);
                }
                else
                    target.AddBuff(BuffID.OnFire, 120);
            }
            base.OnHitNPC(target, hit, damageDone);
        }
    }
}