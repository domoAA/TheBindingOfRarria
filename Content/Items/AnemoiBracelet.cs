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
        public float chance = 0f;
        public bool blocked = false;
        public Vector2 direction = Vector2.UnitY;
        public Vector2 position = Vector2.Zero;
        public override void ResetEffects()
        {
            chance = 0f;
        }
        public override void PostUpdate()
        {
            base.PostUpdate();
            if (blocked)
            {
                position.SpawnDust(ModContent.DustType<PixelatedDustParticle>(), 1.6f, 0.36f, Color.LightSeaGreen, 7, 25, 0.7f, direction.ToRotation() + MathHelper.PiOver2);
                blocked = false;
                direction = Vector2.UnitX;
                position = Vector2.Zero;
            }
        }
        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
        {
            if (Main.rand.NextFloat() < chance * 10)
            {
                modifiers.Cancel();
                Player.immune = true;
                Player.immuneTime = 70;
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    ModPacket packet = ModContent.GetInstance<TheBindingOfRarria>().GetPacket();
                    packet.Write((int)TheBindingOfRarria.PacketTypes.DustSpawn);
                    packet.WriteVector2(Player.Center + Player.Center.DirectionTo(npc.Center) * Player.Hitbox.Size() / 2);
                    packet.WriteVector2(npc.Center.DirectionTo(Player.Center));
                    packet.Send();
                }
                else
                {
                    blocked = true;
                    position = Player.Center + Player.Center.DirectionTo(npc.Center) * Player.Hitbox.Size() / 2;
                    direction = npc.Center.DirectionTo(Player.Center);
                }
            }
            else
                base.ModifyHitByNPC(npc, ref modifiers);
        }
        public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
        {
            if (Main.rand.NextFloat() < chance * 10) {
                modifiers.Cancel();
                Player.immune = true;
                Player.immuneTime = 70;
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    ModPacket packet = ModContent.GetInstance<TheBindingOfRarria>().GetPacket();
                    packet.Write((int)TheBindingOfRarria.PacketTypes.DustSpawn);
                    packet.WriteVector2(Player.Center + Player.Center.DirectionTo(proj.Center) * Player.Hitbox.Size() / 2);
                    packet.WriteVector2(proj.velocity);
                    packet.Send();
                }
                else
                {
                    blocked = true;
                    position = Player.Center + Player.Center.DirectionTo(proj.Center) * Player.Hitbox.Size() / 2;
                    direction = proj.velocity;
                }
            }
            else
                base.ModifyHitByProjectile(proj, ref modifiers);
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