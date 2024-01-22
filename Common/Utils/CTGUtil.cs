using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;

namespace CTGMod.Common.Utils;

public static class CTGUtil
{
    public static void NewLocalizedText(string key, Color? color = null, params object[] args) => Main.NewText(Language.GetTextValue($"Mods.CTGMod.Common.MessageInfo.{key}", args), color);

    public static void PrintText(string text, Color color)
    {
        if (Main.netMode == NetmodeID.SinglePlayer)
        {
            Main.NewText(text, color);
        }
        else
        {
            ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(text), color);
        }
    }

    public static bool SinglePlayerCheck => Main.netMode == NetmodeID.SinglePlayer;
    public static bool MultiplayerClientCheck => Main.netMode == NetmodeID.MultiplayerClient;
    public static bool ServerCheck => Main.netMode == NetmodeID.Server; 
}