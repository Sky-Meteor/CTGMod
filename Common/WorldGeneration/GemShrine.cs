using System.Collections.Generic;
using System.Linq;
using CTGMod.Common.Configs;
using CTGMod.Common.Systems;
using CTGMod.Common.Utils;
using Microsoft.Xna.Framework;
using StructureHelper;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.IO;
using Terraria.WorldBuilding;

namespace CTGMod.Common.WorldGeneration;

public class GemShrine
{
    public static readonly List<string> VanillaGems = new()
    {
        "Amber",
        "Amethyst",
        "Diamond",
        "Emerald",
        "Ruby",
        "Sapphire",
        "Topaz",
    };

    public static void GenerateGemShrine(GenerationProgress progress, GameConfiguration configuration)
    {
        WorldGenSystem.Shrines.Clear();
        foreach (var gem in VanillaGems)
        {
            for (int retry = 0; retry < 80; retry++)
            {
                int x = WorldGen.genRand.Next(100, Main.maxTilesX - 100);
                int y = (int)Main.worldSurface + WorldGen.genRand.Next(0, (int)(Main.maxTilesY - Main.worldSurface - 200 /* underworld */ ));

                Point16 dim = Point16.Zero;
                Generator.GetDimensions($"Structures/{gem}Shrine", CTGMod.Instance, ref dim);

                var rect = new Rectangle(x, y, dim.X, dim.Y);
                if (!GenHelper.CanGenerateStructure(rect, CheckGeneration))
                    continue;

                Generator.GenerateStructure($"Structures/{gem}Shrine", new Point16(x, y), CTGMod.Instance);
                WorldGenSystem.Shrines.Add(gem, rect);

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
        TileID.AmberGemspark,
        TileID.AmethystGemspark,
        TileID.DiamondGemspark,
        TileID.EmeraldGemspark,
        TileID.RubyGemspark,
        TileID.SapphireGemspark,
        TileID.TopazGemspark
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
        WallID.LihzahrdBrickUnsafe,
        WallID.AmberGemspark,
        WallID.AmethystGemspark,
        WallID.DiamondGemspark,
        WallID.EmeraldGemspark,
        WallID.RubyGemspark,
        WallID.SapphireGemspark,
        WallID.TopazGemspark,
    };

    public static bool CheckGeneration(int x, int y)
    {
        Tile tile = Framing.GetTileSafely(x, y);

        if (!WorldGen.InWorld(x, y) || InvalidTiles.Contains(tile.TileType) || InvalidWalls.Contains(tile.WallType) || tile.LiquidAmount != 0)
            return false;

        var aether = new Rectangle((int)GenVars.shimmerPosition.X, (int)GenVars.shimmerPosition.Y, 125, 125);
        if (aether.Contains(x, y))
            return false;
        
        if (WorldGenSystem.Shrines.Values.Any(rect => rect.Distance(new Vector2(x, y)) < CTGConfig.Instance.ShrineDistance))
        {
            return false;
        }

        return true;
    }
}