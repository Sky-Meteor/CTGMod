using Terraria;
using Terraria.ModLoader;

namespace CTGMod.Common;

public class CTGGlobalTile : GlobalTile
{
    public override bool Slope(int i, int j, int type) // protect pylon from being destroyed by hammering the tile below
    {
        Tile tileAbove = Framing.GetTileSafely(i, j - 1);
        if (ModContent.GetModTile(tileAbove.TileType) is ModPylon pylon && pylon.FullName.Contains("CTGMod"))
        {
            return false;
        }

        return base.Slope(i, j, type);
    }
}