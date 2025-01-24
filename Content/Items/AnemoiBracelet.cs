using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items
{
    public class AnemoiBracelet : ModItem
    {
        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
            return !equippedItem.HasTag(TheBindingOfRarria.blockItems) || !incomingItem.HasTag(TheBindingOfRarria.blockItems);
        }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 24;
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.buyPrice(0, 1, 70);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<NatureDodgePlayer>().HasBracelet = true;
        }
        public override void AddRecipes()
        {
            Recipe.Create(Item.type)
                .AddIngredient(ItemID.AnkletoftheWind)
                .AddIngredient(ItemID.Wood, 50)
                .AddIngredient(ItemID.JungleSpores, 12)
                .AddIngredient(ItemID.Vine, 6)
                .AddTile(TileID.WorkBenches)
                .Register();

            base.AddRecipes();
        }
    }
    public class NatureDodgePlayer : ModPlayer
    {
        public bool HasBracelet = false;
        public override void ResetEffects()
        {
            HasBracelet = false;
            blocked = false;
        }
        public void NatureDodge(Vector2 position, float rotation)
        {
            ImmunityWhiteFlash(position, rotation);
            Position = Vector2.Zero;
            Rotation = 0;
            Player.immune = true;
            Player.SetImmuneTimeForAllTypes(Player.longInvince ? 150 : 90);
        }
        public static void ImmunityWhiteFlash(Vector2 center, float rotation)
        {
            var r = 7f;
            var total = 8;
            var vel = Vector2.Zero;
            for (int l = 0; l < 1; l++)
            {
                for (int i = 0; i < total; i++)
                {
                    var offset = (MathHelper.TwoPi / total * i).ToRotationVector2() * r;
                    offset.Y *= 0.5f;
                    var position = offset.RotatedBy(rotation) + center;
                    var velocity = new Vector2(offset.X, offset.Y + vel.Y).RotatedBy(rotation) / 6;
                    Dust.NewDustPerfect(position, Terraria.ID.DustID.FireworksRGB, velocity, 0, Color.ForestGreen, 1).noGravity = true;
                }
                r += 7;
                vel.Y += 3;
                total += 4;
            }
        }
        public bool blocked = false;
        public float Rotation = 0;
        public Vector2 Position = Vector2.Zero;
        public void CheckNatureDodge(Projectile projectile, NPC npc)
        {
            if (!HasBracelet)
                return;

            var chance = Math.Min(0.07f + Player.moveSpeed / 10, 0.21f);
            if (Main.rand.NextFloat() < chance)
                blocked = true;


            //FreeDodge(modifiers.ToHurtInfo(proj.damage, Player.statDefense, Player.DefenseEffectiveness.Value, modifiers.Knockback.ApplyTo(1f), modifiers.KnockbackImmunityEffectiveness.Value == 0));

            if (!blocked)
                return;

            if (projectile == null) {
                Position = Player.Center + Player.Center.DirectionTo(npc.Center) * 2;
                Rotation = Player.Center.DirectionTo(npc.Center).ToRotation() + MathHelper.PiOver2; }
            else {
                Position = projectile.Center + projectile.Center.DirectionFrom(Player.Center) * 2;
                Rotation = projectile.velocity.ToRotation() - MathHelper.PiOver2; }

            NatureDodge(Position, Rotation);
        }
        public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
        {
            base.ModifyHitByProjectile(proj, ref modifiers);

            CheckNatureDodge(proj, null);
        }
        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
        {
            base.ModifyHitByNPC(npc, ref modifiers);

            CheckNatureDodge(null, npc);
        }
        public override bool FreeDodge(Player.HurtInfo info)
        {
            if (HasBracelet && blocked)
            {
                return true;
            }
            return base.FreeDodge(info);
        }
    }
}