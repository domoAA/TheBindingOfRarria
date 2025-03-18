
namespace TheBindingOfRarria.Content.Items
{
    public class HeavyBlow : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.height = 30;
            Item.width = 30;
        }

        public override void UpdateAccessory(Player player, bool hideVisual) => player.GetModPlayer<KnockbackAccessoryPlayer>().KnockbackItem = Item;
        
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.IronBroadsword, 7)
                .AddIngredient(ModContent.ItemType<PaleOre>(), 30)
                .AddTile(TileID.Anvils)
                .Register();

            CreateRecipe()
                .AddIngredient(ItemID.LeadBroadsword, 7)
                .AddIngredient(ModContent.ItemType<PaleOre>(), 30)
                .AddTile(TileID.Anvils)
                .Register();

            base.AddRecipes();
        }
    }
    public class KnockbackAccessoryPlayer : ModPlayer
    {
        public Item KnockbackItem = null;
        public override void ResetEffects() => KnockbackItem = null;
        
        public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (KnockbackItem != null)
            {
                modifiers.Knockback += 1.5f;
                modifiers.FinalDamage += modifiers.Knockback.ApplyTo(0.67f);

                var position = target.Center + target.Center.DirectionTo(Player.Center) * target.Hitbox.Size() / 2;
                Projectile.NewProjectile(Player.GetSource_Accessory(KnockbackItem), position, Player.Center.DirectionTo(position), ModContent.ProjectileType<HeavyBlowThing>(), 0, 0, Player.whoAmI, modifiers.Knockback.ApplyTo(1) / 2);
            }
            base.ModifyHitNPCWithItem(item, target, ref modifiers);
        }
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (KnockbackItem != null)
            {
                modifiers.Knockback += 1.5f;
                modifiers.FinalDamage += modifiers.Knockback.ApplyTo(0.67f);

                var direction = proj.velocity / proj.velocity.Length();
                var position = proj.Center + proj.Center.DirectionTo(target.Center) * proj.Hitbox.Size() / 2;
                if (proj.aiStyle == ProjAIStyleID.Flail || proj.aiStyle == ProjAIStyleID.SolarEffect || proj.aiStyle == ProjAIStyleID.Whip || proj.velocity.LengthSquared() < 1)
                {
                    direction = target.Center.DirectionFrom(proj.Center);
                    position = target.Center + target.Center.DirectionTo(proj.Center) * target.Hitbox.Size() / 2;
                }
                Projectile.NewProjectile(Player.GetSource_Accessory(KnockbackItem), position, direction, ModContent.ProjectileType<HeavyBlowThing>(), 0, 0, proj.owner, modifiers.Knockback.ApplyTo(1) / 2);
            }
            base.ModifyHitNPCWithProj(proj, target, ref modifiers);
        }
    }
}