using CTGMod.Common.ModPlayers;
using Microsoft.Xna.Framework;
using Terraria.Chat;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CTGMod.Common.Systems;

public class CTGGameSystem : ModSystem
{
    public static bool GameStarted = false;
    public static int GameTime;

    public override void PreUpdatePlayers()
    {
        CTGPlayer.UpdateTimer++;

        if (GameStarted)
        {
            GameTime++;
            ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral($"Time: {GameTime}"), Color.Gold);
            if (GameTime % (60 * 60 * 5) == 0)
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral($"游戏时间已经过{GameTime / (60 * 60 * 5)}"), Color.Gold);
        }
    }

    public override void PostUpdateTime()
    {
        base.PostUpdateTime();
    }
}