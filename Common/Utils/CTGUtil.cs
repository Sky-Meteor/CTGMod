using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;

namespace CTGMod.Common.Utils;

public static class CTGUtil
{
    public static void NewLocalizedText(string key, Color? color = null, params object[] args) => Main.NewText(Language.GetTextValue($"Mods.CTGMod.Common.MessageInfo.{key}", args), color);
}