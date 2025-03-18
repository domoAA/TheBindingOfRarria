
namespace TheBindingOfRarria.Content.Items
{
    public class TitanicHydra : ModItem
    {
        public int Frame = new Random().Next(0, 2);
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.height = 32;
            Item.width = 34;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.buyPrice(0, 1, 11, 11);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statLifeMax2 += 60;
            player.GetModPlayer<TitanicPlayer>().Hydra = Item;
            player.GetModPlayer<TitanicPlayer>().counter--;
        }
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            frame = new Rectangle(0, Frame * frame.Height / 2, frame.Width, frame.Height / 2);
            origin = frame.Size() / 2;
            var texture = TextureAssets.Item[Item.type].Value;
            spriteBatch.Draw(texture, position, frame, drawColor, 0, origin, scale * 2, SpriteEffects.None, 0);
            return false;
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Main.GetItemDrawFrame(Item.type, out var texture, out var rect);
            var frame = new Rectangle(0, Frame * rect.Height / 2, rect.Width, rect.Height / 2);
            var origin = frame.Size() / 2;
            spriteBatch.Draw(texture, Item.Bottom - Main.screenPosition - new Vector2(0, origin.Y), frame, lightColor, rotation, origin, scale, SpriteEffects.None, 0);
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.PowerGlove)
                .AddIngredient(ItemID.TitanGlove)
                .AddIngredient(ItemID.LifeCrystal, 3)
                .AddIngredient(ItemID.AegisFruit)
                .AddIngredient(ItemID.AegisCrystal)
                .AddTile(TileID.AdamantiteForge)
                .Register();

            base.AddRecipes();
        }
    }
    public class TitanicPlayer : ModPlayer
    {
        public int counter = 0;
        public Item Hydra = null;
        public override void ResetEffects()
        {
            if (Hydra == null)
                counter = 360;
            Hydra = null;
        }
        public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (counter < 0 && Hydra != null)
            {
                modifiers.FlatBonusDamage += Player.statLifeMax2 / 5;

                counter = 360;

                var position = target.Center + target.Center.DirectionTo(Player.Center) * target.Hitbox.Size() / 2;
                Projectile.NewProjectile(Player.GetSource_Accessory(Hydra), target.Center + target.Center.DirectionFrom(Player.Center) * 150, Vector2.Zero, ModContent.ProjectileType<Cleave>(), Player.statLifeMax2 / 5, 5, Player.whoAmI, 0, position.X, position.Y);
            }
            base.ModifyHitNPCWithItem(item, target, ref modifiers);
        }
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (counter < 0 && Hydra != null)
            {
                modifiers.FlatBonusDamage += Player.statLifeMax2 / 5;

                counter = 360;

                var direction = target.Center + proj.velocity * 150 / proj.velocity.Length();
                var position = proj.Center + proj.Center.DirectionTo(target.Center) * proj.Hitbox.Size() / 2;
                if (proj.aiStyle == ProjAIStyleID.Flail || proj.aiStyle == ProjAIStyleID.SolarEffect || position.Distance(direction) > 250 || proj.aiStyle == ProjAIStyleID.Whip || proj.velocity.LengthSquared() < 1)
                {
                    direction = target.Center + target.Center.DirectionFrom(proj.Center) * 150;
                    position = target.Center + target.Center.DirectionTo(proj.Center) * target.Hitbox.Size() / 2;
                }
                Projectile.NewProjectile(Player.GetSource_Accessory(Hydra), direction, Vector2.Zero, ModContent.ProjectileType<Cleave>(), Player.statLifeMax2 / 5, 5, proj.owner, 0, position.X, position.Y);
            }
            base.ModifyHitNPCWithProj(proj, target, ref modifiers);
        }
    }
    public class CrateLootTitanicHydra : GlobalItem
    {
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            if (item.type == ItemID.GoldenCrateHard)
            {
                var rule = ItemDropRule.Common(ModContent.ItemType<TitanicHydra>(), 6);
                itemLoot.Add(rule);
            }
        }
    }
}