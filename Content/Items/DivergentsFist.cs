
namespace TheBindingOfRarria.Content.Items
{
    public class DivergentsFist : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 26;
            Item.height = 32;

        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<YujiItemPlayer>().counter--;
            player.GetModPlayer<YujiItemPlayer>().Fist = Item;
        }
    }
    public class YujiItemPlayer : ModPlayer
    {
        public Item Fist = null;
        public int counter = 0;
        public override void ResetEffects()
        {
            Fist = null;
        }
        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Fist != null && counter <= 0)
            {
                counter = 180;
                Vector2 offset = target.Center.DirectionTo(Player.Center) * target.Hitbox.Size() / 2;

                if (Main.myPlayer != Player.whoAmI)
                    return;

                Projectile.NewProjectile(Player.GetSource_Accessory(Fist, "Yuji fist attack"), target.Center + offset, -offset, ModContent.ProjectileType<CEFist>(), Player.statManaMax2 / 10, 5, Player.whoAmI, target.whoAmI, offset.X, offset.Y);
            }
        }
    }
}