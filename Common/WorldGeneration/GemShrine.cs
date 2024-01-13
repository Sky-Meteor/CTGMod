using System.Collections.Generic;
using System.Reflection;
using CTGMod.Common.Utils;
using Microsoft.Xna.Framework;
using StructureHelper;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Tile_Entities;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.Default;
using Terraria.WorldBuilding;

namespace CTGMod.Common.WorldGeneration;

public class GemShrine
{
    public static readonly List<string> VanillaGems = new()
    {
        "Topaz",
    };

    public static void GenerateGemShrine(GenerationProgress progress, GameConfiguration configuration)
    {
        foreach (var gem in VanillaGems)
        {
            for (int retry = 0; retry < 40; retry++)
            {
                int x = Main.maxTilesX / 5 + WorldGen.genRand.Next(0, (int)(Main.maxTilesX * 0.6f));
                int y = Main.maxTilesY / 3 + WorldGen.genRand.Next(0, (int)(Main.maxTilesY * 0.5f));

                Point16 dim = Point16.Zero;
                Generator.GetDimensions($"Structures/{gem}Shrine", CTGMod.Instance, ref dim);

                if (!GenHelper.CanGenerateStructure(new Rectangle(x, y, dim.X, dim.Y), CheckGeneration))
                    continue;

                Generator.GenerateStructure($"Structures/{gem}Shrine", new Point16(x, y), CTGMod.Instance);
                
                //TETeleportationPylon.Place(x + (dim.X + 1) / 2, y + dim.Y - 1);
                break;
            }
        }
    }

    public static readonly List<int> InvalidTiles = new()
    {
        TileID.BlueDungeonBrick,
        TileID.GreenDungeonBrick,
        TileID.PinkDungeonBrick,
        TileID.CrackedBlueDungeonBrick,
        TileID.CrackedGreenDungeonBrick,
        TileID.CrackedPinkDungeonBrick,
        TileID.LihzahrdBrick,
    };

    public static readonly List<int> InvalidWalls = new()
    {
        WallID.BlueDungeonSlabUnsafe,
        WallID.GreenDungeonSlabUnsafe,
        WallID.PinkDungeonSlabUnsafe,
        WallID.BlueDungeonTileUnsafe,
        WallID.GreenDungeonTileUnsafe,
        WallID.PinkDungeonTileUnsafe,
        WallID.BlueDungeonUnsafe,
        WallID.GreenDungeonUnsafe,
        WallID.PinkDungeonUnsafe,
        WallID.LihzahrdBrickUnsafe
    };

    public static bool CheckGeneration(int x, int y)
    {
        Tile tile = Framing.GetTileSafely(x, y);

        if (!WorldGen.InWorld(x, y) || InvalidTiles.Contains(tile.TileType) || InvalidWalls.Contains(tile.WallType) || tile.LiquidAmount != 0)
            return false;

        var aether = new Rectangle((int)GenVars.shimmerPosition.X, (int)GenVars.shimmerPosition.Y, 125, 125);
        if (aether.Contains(x, y))
            return false;

        return true;
    }
}