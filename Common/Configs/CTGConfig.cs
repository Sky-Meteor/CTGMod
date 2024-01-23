using System.ComponentModel;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace CTGMod.Common.Configs;

public class CTGConfig : ModConfig
{
    public override ConfigScope Mode => ConfigScope.ServerSide;

    [DefaultValue(5)]
    [Range(1, 60 * 60 * 20)]
    public int PlayerPositionUpdateTime;

    [DefaultValue(true)]
    public bool AutoGemSlot;

    [DefaultValue(500)]
    [Range(50, 5000)]
    public int ShrineDistance;

    public static CTGConfig Instance => ModContent.GetInstance<CTGConfig>();

    public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref NetworkText message)
    {
        message = NetworkText.FromLiteral($"CTGMod: Config is changed by {Main.player[whoAmI].name}.");
        return true;
    }
}