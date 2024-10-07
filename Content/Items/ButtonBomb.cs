using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Projectiles;
using Microsoft.Xna.Framework;
using System.Linq;

namespace TheBindingOfRarria.Content.Items
{
    public class ButtonBomb : ModItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(8, 4));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 32;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<ButtonBombPlayer>().HasABomb = true;
        }
    }
    public class ButtonBombPlayer : ModPlayer
    {
        public bool HasABomb = false;
        public override void ResetEffects()
        {
            HasABomb = false;
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!HasABomb || proj.type == ModContent.ProjectileType<DetonatedButton>())
            {
                base.OnHitNPCWithProj(proj, target, hit, damageDone);
                return;
            }
            if (target.GetGlobalNPC<ButtonedNPC>().ButtonCD == 0) {
                Vector2 offset = (target.Center - proj.Center);
                var bigger = target.width > target.height ? target.height : target.width;
                var together = target.width * target.width + target.height * target.height;
                if (offset.LengthSquared() > together) {
                    offset.Normalize();
                    offset *= bigger; }
                else
                    offset *= 0.9f;
                Projectile.NewProjectile(Player.GetSource_FromThis(), target.Center - offset, new Vector2(0, 0), ModContent.ProjectileType<Extra98Bomb>(), 0, 0, Player.whoAmI, target.whoAmI, offset.X, offset.Y);
                target.GetGlobalNPC<ButtonedNPC>().ButtonCD = 20; }
        }
    }
    public class ButtonedNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public int ButtonCD = 0;
        public override void PostAI(NPC npc)
        {
            if (ButtonCD > 0)
                ButtonCD--;
        }
    }
}