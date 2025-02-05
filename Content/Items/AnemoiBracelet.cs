using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items
{
    public class AnemoiBracelet : ModItem
    {
        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
            return !equippedItem.HasTag(TheBindingOfRarria.dodgeItems) || !incomingItem.HasTag(TheBindingOfRarria.dodgeItems);
        }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 24;
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.buyPrice(0, 1, 70);
        }
        public float chance = 0.07f;
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<NatureDodgePlayer>().HasBracelet = true;
            chance = Math.Min(0.07f + (player.moveSpeed - 0.5f) / 10, 0.21f);
            player.GetModPlayer<NatureDodgePlayer>().chance = chance;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            base.ModifyTooltips(tooltips);
            var text = string.Format(Language.GetTextValue("Mods.TheBindingOfRarria.Items.AnemoiBracelet.Tooltip"), $"{(int)(chance * 100)}%");
            for (int i = 10; i > 0; i--)
            {
                var index = tooltips.FindIndex(line => line.Text.Contains(Language.GetTextValue("Mods.TheBindingOfRarria.Items.AnemoiBracelet.Tooltip").Remove(5)));
                if (index != -1) {
                    tooltips[index].Text = text.Remove(text.IndexOf($"\n"));
                    break; }
            } 
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
            ReflectionVisual(position, rotation);
            Position = Vector2.Zero;
            Rotation = 0;
            Player.immune = true;
            Player.SetImmuneTimeForAllTypes(Player.longInvince ? 150 : 90);
        }
        public static void ReflectionVisual(Vector2 center, float rotation)
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
                    Dust.NewDustPerfect(position, ModContent.DustType<PixelatedDustParticle>(), velocity * 6, 0, Color.ForestGreen, 0.35f);
                }
                r += 7;
                vel.Y += 3;
                total += 4;
            }
        }
        public bool blocked = false;
        public float Rotation = 0;
        public float chance = 0.07f;
        public Vector2 Position = Vector2.Zero;
        public void CheckNatureDodge(Projectile projectile, NPC npc)
        {
            if (!HasBracelet)
                return;

            
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
    public class PixelatedDustParticle : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.noLight = true;
        }
        public override bool Update(Dust dust)
        {
            dust.position += dust.velocity;
            dust.rotation = dust.velocity.ToRotation();
            dust.velocity *= 0.9f;
            dust.color.A -= 8;
            float light = 0.002f * dust.color.A;

            Lighting.AddLight(dust.position, light, light, light);

            if (dust.color.A < 100)
            {
                dust.active = false;
            }

            return false;
        }
        public override bool PreDraw(Dust dust)
        {
            Texture2D.Value.DrawPixellated((dust.position - Main.screenPosition) / 2, dust.scale * new Vector2(0.9f, 0.015f * dust.color.A), dust.rotation + MathHelper.PiOver2, dust.color, PixellationSystem.RenderType.Additive);
            return false;
        }
    }
}