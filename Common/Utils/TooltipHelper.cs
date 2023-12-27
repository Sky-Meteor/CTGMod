using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;

namespace CTGMod.Common.Utils;

public static class TooltipHelper
{
    public static string ItemTooltipText(int itemID, string text) => $"[i:{itemID}] {text}";

    public static string ColorTooltipText(Color color, string text) => $"[c/{color.Hex3()}:{text}]";

    public static string ItemColorTooltipText(int itemID, Color color, string text) => $"[i:{itemID}] [c/{color.Hex3()}:{text}]";

    public static string LocalizedItemColorTooltipText(int itemID, Color color, string key) => $"[i:{itemID}] [c/{color.Hex3()}:{Language.GetTextValue($"Mods.CTGMod.Common.Tooltips.{key}")}]";
}