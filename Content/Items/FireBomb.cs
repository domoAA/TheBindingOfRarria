
namespace TheBindingOfRarria.Content.Items
{
    public class FireBomb : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.accessory = true;
            Item.value = Item.buyPrice(0, 0, 80);
            Item.rare = ItemRarityID.LightRed;
        }
        public override void UpdateAccessory(Player player, bool hideVisual) => player.GetModPlayer<ExplotaroPlayer>().Bomboclat = Item;
        
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Bomb)
                .AddIngredient(ItemID.LivingFireBlock, 8)
                .AddIngredient(ItemID.ExplosivePowder, 13)
                .AddTile(TileID.Anvils)
                .Register();

            base.AddRecipes();
        }
    }
    public class ExplotaroPlayer : ModPlayer
    {
        public Item Bomboclat = null;
        public override void ResetEffects() => Bomboclat = null;
        
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Bomboclat != null)
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
                    Projectile.NewProjectile(Player.GetSource_Accessory(Bomboclat), target.Center, Vector2.Zero, ModContent.ProjectileType<FireExplotaro>(), damag, 1.5f, Player.whoAmI);
                }
                else
                    target.AddBuff(BuffID.OnFire, 120);
            }
            base.OnHitNPC(target, hit, damageDone);
        }
    }
}