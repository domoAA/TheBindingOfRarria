using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TheBindingOfRarria.Common.Config;
using TheBindingOfRarria.Common.Helpers;
using TheBindingOfRarria.Common.Systems;
using TheBindingOfRarria.Content.Projectiles;

namespace TheBindingOfRarria.Content.Items;

/*public class CrystalHeart : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.height = 30;
        Item.width = 30;
        Item.rare = ItemRarityID.Expert;
        Item.value = Item.buyPrice(0, 2);
        Item.expert = true;
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

    public class CrystalHeartDropNPC : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.GraniteFlyer)
                npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsExpert(), ModContent.ItemType<CrystalHeart>(), 60));

            base.ModifyNPCLoot(npc, npcLoot);
        }
    }

    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        string key = KeybindSystem.CrystalDashKey.GetAssignedKeys().FirstOrDefault();
        if (key == "" || key == null)
            key = "V";

        string text = string.Format(Language.GetTextValue("Mods.TheBindingOfRarria.Items.CrystalHeart.Tooltip"), key);


        int index = tooltips.FindIndex(line => line.Name == "Tooltip0");
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

    public float[] RandomRotations = new float[10];

    public override void ResetEffects()
    {
        if (!IsCrystalAndPeak)
        {
            Dashing = false;

            for (int i = 0; i < 10; i++)
                RandomRotations[i] = Main.rand.NextFloat(-PiOver4 / 4, PiOver4 / 4);
        }

        if (Dashing && (Player.grapCount > 0 || Player.controlJump || Player.controlUseItem || Player.controlUseTile)) {
            Dashing = false;

            for (int i = 0; i < 10; i++)
                RandomRotations[i] = Main.rand.NextFloat(-PiOver4 / 4, PiOver4 / 4);
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
        else
            for (int i = 0; i < 10; i++)
                RandomRotations[i] = Main.rand.NextFloat(-PiOver4 / 4, PiOver4 / 4);

    }

        // Make into a helper method.
    public bool CheckGrounded()
    {
        for (int x = -2; x < 2;  x++)
            if (WorldGen.SolidOrSlopedTile(Player.Center.ToTileCoordinates().X + x, (Player.Bottom + new Vector2(0, 8)).ToTileCoordinates().Y))
                return true;

        return false;
    }

    public override void ProcessTriggers(TriggersSet triggersSet)
    {
        if ((CheckGrounded() || Player.sliding) && !Dashing && IsCrystalAndPeak && (KeybindSystem.CrystalDashKey.Current || (KeybindSystem.CrystalDashKey.GetAssignedKeys().FirstOrDefault() == null && Main.keyState.IsKeyDown(Keys.V))) && Main.myPlayer == Player.whoAmI)
        {
            if (counter > 0)
            {
                counter--;
                Player.immuneAlpha = (int)(200 * float.Sin(counter));
                Dir = Player.direction;
                Holding = 30 - counter % 30;
                Player.mount.Dismount(Player);
                Player.RemoveAllGrapplingHooks();

                if (RandomRotations.Length < 9 || RandomRotations.Last() == 0)
                for (int i = 0; i < 10; i++)
                    RandomRotations[i] = Main.rand.NextFloat(-PiOver4 / 4, PiOver4 / 4);
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
        if (drawInfo.shadow == 0)
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
                rot = PiOver4 + RandomRotations[i];
            else if (tile.Slope == SlopeType.SlopeDownRight)
                rot = -PiOver4 + RandomRotations[i];

            return true;

        }
        return false;
    }

    public void DrawCrystals()
    {
        Texture2D texture = TextureAssets.Item[ItemID.CrystalShard].Value;

        Rectangle frame = new(8, 0, 10, 16);

        for (int i = 1; i < Holding / 4; i++)
        {
            for (int x = -1; x < 2; x += 2)
            {
                bool draw = true;
                SpriteEffects effect = x == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                Vector2 position = Player.Center - new Vector2(0, Player.gfxOffY) + new Vector2(10 * x + 3 * i * i, 0) * x;
                float rotation = PiOver4 / 8 * x + RandomRotations[i];

                for (int y = 12; y > -8; y--)
                {
                    if (CalculateCrystalPos(ref i, y, ref rotation, ref position))
                    {
                        draw = true;
                        break;
                    }
                    else if (y == 11)
                        draw = false;
                    else
                        draw = CalculateCrystalPos(ref i, -y, ref rotation, ref position);
                }
                if (draw)
                {
                    var color = Color.White * (100f / 255f);
                    if (!Dashing)
                        Main.EntitySpriteDraw(texture, position - Main.screenPosition, frame, color with { A = 0 }, rotation, texture.Size() * 0.5f, 0.6f + i * 0.5f, effect);
                    else
                        Dust.NewDustDirect(position, 5, 8, DustID.PinkCrystalShard, Alpha: 190, Scale: 1.5f);
                }
            }
        }
    }
}*/