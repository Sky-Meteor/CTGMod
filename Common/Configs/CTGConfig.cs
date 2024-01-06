using System.ComponentModel;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace CTGMod.Common.Configs;

public class CTGConfig : ModConfig
{
    public override ConfigScope Mode => ConfigScope.ServerSide;

    [DefaultValue(10)]
    [Range(1, 60 * 60 * 20)]
    public int PlayerPositionUpdateTime;

    public static CTGConfig Instance => ModContent.GetInstance<CTGConfig>();

    public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref NetworkText message)
    {
        message = NetworkText.FromLiteral($"CTGMod: Config is changed by {Main.player[whoAmI].name}.");
        return true;
    }
}