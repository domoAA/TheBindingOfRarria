using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Projectiles;
using Microsoft.Xna.Framework;

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
            Item.value = Item.buyPrice(0, 0, 60);
            Item.rare = ItemRarityID.Green;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<ButtonBombPlayer>().Button = Item;
        }
        public override void AddRecipes()
        {
            Recipe.Create(Item.type)
                .AddIngredient(ItemID.Grenade, 10)
                .AddIngredient(ItemID.CopperBar, 4)
                .AddIngredient(ItemID.RedDye, 2)
                .AddTile(TileID.Anvils)
                .Register();

            base.AddRecipes();
        }
    }
    public class ButtonBombPlayer : ModPlayer
    {
        public Item Button = null;
        public override void ResetEffects()
        {
            Button = null;
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Button == null || proj.type == ModContent.ProjectileType<Extra98Bomb>() || proj.type == ModContent.ProjectileType<FireExplotaro>())
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
                    offset *= 0.8f;

                if (Main.myPlayer != Player.whoAmI)
                    return;
                Projectile.NewProjectileDirect(Player.GetSource_Accessory(Button), target.Center - offset, new Vector2(0, 0), ModContent.ProjectileType<Extra98Bomb>(), 3, 1, Player.whoAmI, target.whoAmI, offset.X, offset.Y).rotation = Main.rand.NextFloat() * MathHelper.TwoPi;
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