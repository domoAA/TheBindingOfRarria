
using System.Diagnostics.Metrics;
using Terraria;
using Terraria.GameInput;

namespace TheBindingOfRarria.Content.Items
{
    public class CrystalHeart : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.height = 30;
            Item.width = 30;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<CrystalDashPlayer>().IsCrystalAndPeak = true;
            if (player.GetModPlayer<CrystalDashPlayer>().Dashing)
            {
                player.immune = true;
                player.immuneTime = 30;
                player.shimmerImmune = true;
                player.immuneAlpha = 100;
                player.direction = player.GetModPlayer<CrystalDashPlayer>().Dir;
                player.SpawnProjectileIfNotSpawned(ModContent.ProjectileType<CrystalDashTrail>(), player.GetSource_Accessory(Item, "Crystal dash"));
            }
        }
    }
    public class CrystalDashPlayer : ModPlayer
    {
        public bool IsCrystalAndPeak = false;
        public int counter = 20;
        public bool Dashing = false;
        public int Dir = 1;
        public override void ResetEffects()
        {
            if (!IsCrystalAndPeak)
                Dashing = false;

            if (Dashing && (Player.controlJump || Player.controlUseItem || Player.controlUseTile))
                Dashing = false;

            IsCrystalAndPeak = false;
        }
        public override void PreUpdateMovement()
        {
            if (Dashing)
            {
                Player.velocity = new Vector2(10 * Player.direction, 0);
                Player.gravity = 0;
            }
        }
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (!Dashing && IsCrystalAndPeak && (KeybindSystem.CrystalDashKey.Current || (KeybindSystem.CrystalDashKey.GetAssignedKeys().FirstOrDefault() == null && Main.keyState.IsKeyDown(Keys.V))) && Main.myPlayer == Player.whoAmI)
            {
                if (counter > 0)
                {
                    counter--;
                    Player.immuneAlpha = (int)(200 * float.Sin(counter));
                    Dir = Player.direction;
                    DrawCrystals(20 - counter % 20);
                }

                else
                {
                    counter = 20;
                    Dashing = true;
                }
            }
            else if (counter < 20)
                counter = 20;
        }
        public void DrawCrystals(int progress)
        {
            for (int i = progress / 4; i > 0; i--)
            {
                var texture = TextureAssets.Tile[TileID.Crystals].Value;
                var rect = new Rectangle(0, texture.Height / 19 * Main.rand.Next(18), texture.Width / 4, texture.Height / 19);
                Main.spriteBatch.Begin();
                Main.EntitySpriteDraw(texture, Player.Center + new Vector2(100, 0), rect, Color.White, 0, rect.Size() / 2, 1, SpriteEffects.None);
                Main.spriteBatch.End();
            }
        }
    }
}