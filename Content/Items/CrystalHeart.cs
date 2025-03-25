
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
            player.GetModPlayer<CrystalDashPlayer>().Holding -= 3;
            if (player.GetModPlayer<CrystalDashPlayer>().Dashing)
            {
                player.mount.Dismount(player);
                player.RemoveAllGrapplingHooks();
                player.shimmerImmune = true;
                player.direction = player.GetModPlayer<CrystalDashPlayer>().Dir;
                player.SpawnProjectileIfNotSpawned(ModContent.ProjectileType<CrystalDashTrail>(), player.GetSource_Accessory(Item, "Crystal dash"));
            }
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            var key = KeybindSystem.CrystalDashKey.GetAssignedKeys().FirstOrDefault();
            if (key == "" || key == null)
                key = "V";

            var text = string.Format(Language.GetTextValue("Mods.TheBindingOfRarria.Items.CrystalHeart.Tooltip"), key);


            var index = tooltips.FindIndex(line => line.Name == "Tooltip0");
            if (index != -1)
            {
                text = text.Remove(text.LastIndexOf($"\n"));
                tooltips[index].Text = text;
            }
        }
    }
    public class CrystalDashPlayer : ModPlayer
    {
        public bool IsCrystalAndPeak = false;
        public int counter = 20;
        public bool Dashing = false;
        public int Dir = 1;
        public int Holding = 0;
        public List<float> RandRot = [10];
        public override void ResetEffects()
        {
            if (!IsCrystalAndPeak)
            {
                Dashing = false;
                RandRot.Clear();
                for (int i = 0; i < 10; i++)
                {
                    RandRot.Add(Main.rand.NextFloat(-PiOver4 / 4, PiOver4 / 4));
                }
            }

            if (Dashing && (Player.grapCount > 0 || Player.controlJump || Player.controlUseItem || Player.controlUseTile)) { 
                Dashing = false;
                RandRot.Clear();
                for (int i = 0; i < 10; i++)
                {
                    RandRot.Add(Main.rand.NextFloat(-PiOver4 / 4, PiOver4 / 4));
                }
            }

            Holding = Holding < 0 ? 0 : Holding;

            IsCrystalAndPeak = false;
        }
        public override void PreUpdateMovement()
        {
            if (Dashing)
            {
                Player.velocity = new Vector2(12 * Player.direction, 0.0001f);
                Player.gravity = 0;
                Player.legFrame.Y = 560;
                Player.legFrameCounter = 0;
                Player.bodyFrameCounter = 0;
            }
            else if (Holding > 0)
                Player.velocity *= 0;
        }
        public bool CheckGrounded(Player player)
        {
            for (int x = -2; x < 2;  x++)
            {
                if (WorldGen.SolidOrSlopedTile(Player.Center.ToTileCoordinates().X + x, (player.Bottom + new Vector2(0, 8)).ToTileCoordinates().Y))
                {
                    return true;
                }
            }
            return false;
        }
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if ((CheckGrounded(Player) || Player.sliding) && !Dashing && IsCrystalAndPeak && (KeybindSystem.CrystalDashKey.Current || (KeybindSystem.CrystalDashKey.GetAssignedKeys().FirstOrDefault() == null && Main.keyState.IsKeyDown(Keys.V))) && Main.myPlayer == Player.whoAmI)
            {
                if (counter > 0)
                {
                    counter--;
                    Player.immuneAlpha = (int)(200 * float.Sin(counter));
                    Dir = Player.direction;
                    Holding = 30 - counter % 30;
                    Player.mount.Dismount(Player);
                    Player.RemoveAllGrapplingHooks();

                    if (RandRot.Count < 8)
                    {
                        RandRot.Clear();
                        for (int i = 0; i < 10; i++)
                        {
                            RandRot.Add(Main.rand.NextFloat(-PiOver4 / 4, PiOver4 / 4));
                        }
                    }
                }

                else
                {
                    counter = 30;
                    Dashing = true;
                }
            }
            else if (counter < 30)
                counter = 30;
        }
        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            base.DrawEffects(drawInfo, ref r, ref g, ref b, ref a, ref fullBright);
            DrawCrystals();
        }
        public bool CalculateCrystalPos(ref int i, int y, ref float rot, ref Vector2 pos)
        {
            var tile = Main.tile[pos.ToTileCoordinates().X, pos.ToTileCoordinates().Y + y];

            if (WorldGen.SolidOrSlopedTile(pos.ToTileCoordinates().X, pos.ToTileCoordinates().Y + y - 1))
                return false;

            else if (WorldGen.SolidTile(tile))
            {
                pos += new Vector2(0, y).ToWorldCoordinates();
                pos.Y -= 20 + i;

                return true;
            }

            else if (tile.Slope != SlopeType.Solid)
            {
                pos += new Vector2(0, y).ToWorldCoordinates();
                pos.Y -= 23 + (i - 4) * 2;

                if (tile.Slope == SlopeType.SlopeDownLeft)
                    rot = PiOver4 + RandRot[i];
                else if (tile.Slope == SlopeType.SlopeDownRight)
                    rot = -PiOver4 + RandRot[i];

                return true;

            }
            return false;
        }
        public void DrawCrystals()
        {
            var texture = TextureAssets.Item[ItemID.CrystalShard].Value;
            var rect = new Rectangle(8, 0, 10, 16);
            for (int i = 1; i < Holding / 4; i++)
            {
                for (int x = -1; x < 2; x += 2)
                {
                    bool draw = true;
                    SpriteEffects effect = x == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                    var pos = Player.Center - new Vector2(0, Player.gfxOffY) + new Vector2(10 * x + 3 * i * i, 0) * x;
                    var rot = PiOver4 / 8 * x + RandRot[i];

                    for (int y = 12; y > -8; y--)
                    {
                        if (CalculateCrystalPos(ref i, y, ref rot, ref pos))
                        {
                            draw = true;
                            break;
                        }
                        else if (y == 11)
                        {
                            draw = false;
                        }
                        else
                            draw = CalculateCrystalPos(ref i, -y, ref rot, ref pos);
                    }
                    if (draw)
                    {
                        if (!Dashing)
                            Main.EntitySpriteDraw(texture, pos - Main.screenPosition, rect, Color.White with { A = 0 }, rot, texture.Size() / 2, 0.6f + i * 0.5f, effect);
                        else
                            Dust.NewDustDirect(pos, 5, 8, DustID.PinkCrystalShard, Alpha: 190, Scale: 1.5f);
                    }
                }
            }
        }
    }
}