using Terraria;
using Terraria.ModLoader;

namespace CTGMod.Common.Systems;

public class ActuatorDeActivePatch : ModSystem
{
    public override void Load()
    {
        On_Wiring.DeActive += On_Wiring_DeActive;
    }

    private void On_Wiring_DeActive(On_Wiring.orig_DeActive orig, int i, int j) // protect pylon from being destroyed by hammering the tile below
    {
        Tile tileAbove = Framing.GetTileSafely(i, j - 1);
        if (ModContent.GetModTile(tileAbove.TileType) is ModPylon pylon && pylon.FullName.Contains("CTGMod"))
            return;

        orig.Invoke(i, j);
    }
}