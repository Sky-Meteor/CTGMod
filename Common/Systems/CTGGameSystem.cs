using CTGMod.Common.ModPlayers;
using Terraria.ModLoader;

namespace CTGMod.Common.Systems;

public class CTGGameSystem : ModSystem
{
    public override void PreUpdatePlayers()
    {
        CTGPlayer.UpdateTimer++;
    }
}