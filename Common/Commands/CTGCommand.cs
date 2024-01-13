using CTGMod.Common.Systems;
using Microsoft.Xna.Framework;
using Terraria.Chat;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CTGMod.Common.Commands;

public class CTGCommand : ModCommand
{
    public override CommandType Type => CommandType.Chat;

    public override string Command => "ctg";

    public override void Action(CommandCaller caller, string input, string[] args)
    {
        if (args.Length < 1)
        {
            caller.Reply("命令错误：缺少参数");
        }

        switch (args[0])
        {
            case "start":
                if (CTGGameSystem.GameStarted)
                    caller.Reply("游戏正在进行");
                CTGGameSystem.GameStarted = true;
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("游戏开始！"), Color.Gold);
                break;
            case "stop":
                CTGGameSystem.GameStarted = false;
                CTGGameSystem.GameTime = 0;
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("游戏停止！"), Color.Gold);
                break;
            default:
                caller.Reply($"命令错误：参数{args[0]}错误");
                break;
        }
    }
}