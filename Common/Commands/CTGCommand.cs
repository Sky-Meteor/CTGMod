using CsvHelper.TypeConversion;
using CTGMod.Common.Systems;
using CTGMod.Common.Utils;
using CTGMod.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Chat;
using Terraria.ID;
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
                {
                    caller.Reply("游戏正在进行");
                    break;
                }

                if (CTGUtil.SinglePlayerCheck)
                {
                    CTGGameSystem.GameStarted = true;
                    CTGGameSystem.GameTime = 0;
                }
                else
                {
                    ModPacket packet = Mod.GetPacket();
                    packet.Write((byte)CTGPacketID.StartGame);
                    packet.Send();
                }
                CTGUtil.PrintText("游戏开始！", Color.Gold);
                break;
            case "stop":
                if (CTGUtil.SinglePlayerCheck)
                {
                    CTGGameSystem.GameStarted = false;
                    CTGGameSystem.GameTime = 0;
                }
                else
                {
                    ModPacket packet2 = Mod.GetPacket();
                    packet2.Write((byte)CTGPacketID.EndGame);
                    packet2.Send();
                }
                CTGUtil.PrintText("游戏开始！", Color.Gold);
                break;
            case "when":
                if (!CTGGameSystem.GameStarted)
                {
                    caller.Reply("游戏未开始！");
                    break;
                }
                caller.Reply($"当前游戏时间：{CTGGameSystem.GameTime / 60}秒", Color.Gold);
                break;
            default:
                caller.Reply($"命令错误：参数{args[0]}错误");
                break;
        }
    }
}