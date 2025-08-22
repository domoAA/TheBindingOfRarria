using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using TheBindingOfRarria.Content.Tiles;

namespace TheBindingOfRarria.Content.WorldGeneration;

public sealed class PaleOreGeneration : ModSystem
{
    private static LocalizedText PaleOrePassMessage;

    public override void SetStaticDefaults() => PaleOrePassMessage = Language.GetOrRegister(Mod.GetLocalizationKey($"WorldGen.{nameof(PaleOrePassMessage)}"));

    public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
    {
        int ShiniesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Shinies"));
        if (ShiniesIndex != -1)
            tasks.Insert(ShiniesIndex + 1, new PaleOrePass("TheBindingOfRarria: Pale Ore", 100f));
    }

    private sealed class PaleOrePass(string name, float loadWeight) : GenPass(name, loadWeight)
    {
        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = PaleOrePassMessage.Value;

            for (int k = 0; k < (int)(Main.maxTilesX * Main.maxTilesY * 0.00002); k++)
            {
                int x = WorldGen.genRand.Next(150, Main.maxTilesX - 150);
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
                int WhitelistedBlocksCount = WhiteList[TileID.Stone] + WhiteList[TileID.IceBlock];

                if (WhitelistedBlocksCount < 250)
                {
                    k--;
                    continue;
                }

                WorldGen.TileRunner(x, y, WorldGen.genRand.Next(3, 5), WorldGen.genRand.Next(3, 6), ModContent.TileType<PaleOreTile>());
            }

        }
    }
}