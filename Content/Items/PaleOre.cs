using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace TheBindingOfRarria.Content.Items
{
    public class PaleOre : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<PaleOreTile>());
            Item.width = 30;
            Item.height = 26;
            Item.value = Item.buyPrice(0, 0, 20);
            Item.rare = ItemRarityID.Green;
        }
    }
    public class CrateLootPaleOre : GlobalItem
    {
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            if (item.type == ItemID.FrozenCrate || item.type == ItemID.FrozenCrateHard)
            {
                var rule = ItemDropRule.Common(ModContent.ItemType<PaleOre>(), 9, 2, 5);
                itemLoot.Add(rule);
            }
        }
    }
    public class PaleOreTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            TileID.Sets.Ore[Type] = true;
            TileID.Sets.FriendlyFairyCanLureTo[Type] = true;
            Main.tileSpelunker[Type] = true;
            Main.tileOreFinderPriority[Type] = 404;
            Main.tileShine2[Type] = true;
            Main.tileShine[Type] = 600;
            Main.tileMergeDirt[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;

            var name = CreateMapEntryName();
            AddMapEntry(Color.GhostWhite, name);

            DustType = DustID.WhiteTorch;
            HitSound = SoundID.Tink;
            MineResist = 4f;
            MinPick = 50;
        }
        public override bool CanExplode(int x, int y)
        {
            if (Main.tile[x, y].TileType == ModContent.TileType<PaleOreTile>())
                return false;

            return base.CanExplode(x, y);
        }
        public override void RandomUpdate(int x, int y)
        {
            Point XY = new(Main.rand.Next(-1, 2), Main.rand.Next(-1, 2));

            var pos = XY + new Point(x, y);
            var neighbor = Main.tile[pos];

            if (neighbor.TileType == ModContent.TileType<PaleOreTile>())
                neighbor = Main.tile[x + Main.rand.Next(-1, 2), y + Main.rand.Next(-1, 2)];

            else if (WorldGen.SolidOrSlopedTile(neighbor) && neighbor.TileType == TileID.IceBlock)
            {
                neighbor.ResetToType((ushort)ModContent.TileType<PaleOreTile>());
                WorldGen.SquareTileFrame(pos.X, pos.Y);
                NetMessage.SendTileSquare(-1, pos.X, pos.Y, 1);
            }
            else if (
                neighbor.TileType != TileID.LihzahrdBrick &&
                neighbor.TileType != TileID.BlueDungeonBrick &&
                neighbor.TileType != TileID.PinkDungeonBrick &&
                neighbor.TileType != TileID.GreenDungeonBrick &&
                TileID.Count > neighbor.TileType &&
                (!Main.tileOreFinderPriority.Contains((short)neighbor.TileType) ||
                Main.tileOreFinderPriority[neighbor.TileType] < 404))
            {
                Main.tile[pos.X, pos.Y].TileType = TileID.IceBlock;
                WorldGen.SquareTileFrame(pos.X, pos.Y);
                NetMessage.SendTileSquare(-1, pos.X, pos.Y, 1);
            }
        }
    }
    public class WorldGenTutorialSystem : ModSystem
    {
        public static LocalizedText WorldGenTutorialOresPassMessage { get; private set; }

        public override void SetStaticDefaults()
        {
            WorldGenTutorialOresPassMessage = Language.GetOrRegister(Mod.GetLocalizationKey($"WorldGen.{nameof(WorldGenTutorialOresPassMessage)}"));
        }

        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            int ShiniesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Shinies"));
            if (ShiniesIndex != -1)
            {
                tasks.Insert(ShiniesIndex + 1, new WorldGenTutorialOresPass("TheBindingOfRarria: Pale Ore", 100f));
            }
        }
    }

    public class WorldGenTutorialOresPass(string name, float loadWeight) : GenPass(name, loadWeight)
    {

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = WorldGenTutorialSystem.WorldGenTutorialOresPassMessage.Value;

            for (int a = 3; a > 1; a--)
            {
                for (int k = 0; k < (int)((Main.maxTilesX * Main.maxTilesY) * 0.000003 * a); k++)
                {
                    int x = WorldGen.genRand.Next(0, Main.maxTilesX);
                    int y = WorldGen.genRand.Next((int)GenVars.rockLayerLow, Main.UnderworldLayer - 100);

                    Point point = new(x, y);
                    Dictionary<ushort, int> BlackList = [];
                    WorldUtils.Gen(point, new Shapes.Rectangle(20, 20), new Actions.TileScanner(TileID.LihzahrdBrick, TileID.BlueDungeonBrick, TileID.PinkDungeonBrick, TileID.GreenDungeonBrick).Output(BlackList));
                    int BlacklistedBlocksCount = BlackList[TileID.LihzahrdBrick] + BlackList[TileID.BlueDungeonBrick] + BlackList[TileID.PinkDungeonBrick] + BlackList[TileID.GreenDungeonBrick];

                    if (BlacklistedBlocksCount > 0)
                    {
                        k--;
                        continue;
                    }

                    Dictionary<ushort, int> WhiteList = [];
                    WorldUtils.Gen(point, new Shapes.Rectangle(20, 20), new Actions.TileScanner(TileID.Stone, TileID.IceBlock).Output(WhiteList));
                    int WhitelistedBlocksCount = a == 2 ? WhiteList[TileID.Stone] + WhiteList[TileID.IceBlock] : WhiteList[TileID.IceBlock];

                    if (WhitelistedBlocksCount < 250)
                    {
                        k--;
                        continue;
                    }


                    WorldGen.TileRunner(x, y, WorldGen.genRand.Next(3, 5), WorldGen.genRand.Next(2, 6), ModContent.TileType<PaleOreTile>());
                }
            }
        }
    }
}