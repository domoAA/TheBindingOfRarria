using System.Collections.Generic;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Terraria;
using TheBindingOfRarria.Content.Tiles;
using Microsoft.Xna.Framework;

namespace TheBindingOfRarria.Content.WorldGeneration;

public sealed class SihilStoneGeneration : ModSystem
{
    private static LocalizedText SihilStonePassMessage;

    public override void SetStaticDefaults() => SihilStonePassMessage = Language.GetOrRegister(Mod.GetLocalizationKey($"WorldGen.{nameof(SihilStonePassMessage)}"));

    public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
    {
        int ShiniesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Shinies"));
        if (ShiniesIndex != -1)
            tasks.Insert(ShiniesIndex + 1, new SihilStonePass("TheBindingOfRarria: Sihil Stone", 100f));
    }

    private sealed class SihilStonePass(string name, float loadWeight) : GenPass(name, loadWeight)
    {
        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = SihilStonePassMessage.Value;

            for (int k = 0; k < (int)(Main.maxTilesX * Main.maxTilesY * 0.0001); k++)
            {
                int x = WorldGen.genRand.Next(150, Main.maxTilesX - 150);
                int y = WorldGen.genRand.Next((int)Main.worldSurface - 100, (int)Main.rockLayer);

                Point point = new(x, y);
                for (int X = -1; X < 2; X++) 
                {
                    var floorTile = Main.tile[x + X, y + 1];
                    if (WorldGen.SolidOrSlopedTile(floorTile) && (floorTile.TileType == TileID.SnowBlock || floorTile.TileType == TileID.IceBlock))
                        if (WorldGen.TileEmpty(x + X, y) && WorldGen.TileEmpty(x + X, y - 1))
                            WorldGen.Place3x2(x, y, (ushort)ModContent.TileType<SihilStone>());
                } 
            }
        }
    }
}