using CTGMod.Common.ModPlayers;
using CTGMod.Common.Systems;
using CTGMod.Common.Utils;
using CTGMod.ID;
using Microsoft.Xna.Framework;
using Terraria;
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

        switch (args[0].ToLower())
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
                    Main.LocalPlayer.GetModPlayer<CTGPlayer>().TryUseTeleportation = true;
                }
                else
                {
                    ModPacket packet = Mod.GetPacket();
                    packet.Write((byte)CTGPacketID.StartGame);
                    packet.Send();
                    foreach (var plr in Main.player)
                    {
                        if (plr == null || !plr.active)
                            continue;
                        plr.GetModPlayer<CTGPlayer>().TryUseTeleportation = true;
                    }
                }
                CTGUtil.PrintText("游戏开始！", Color.Gold);
                break;
            case "stop":
                if (!CTGGameSystem.GameStarted)
                {
                    caller.Reply("游戏未进行");
                    break;
                }

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
                CTGUtil.PrintText("游戏停止！", Color.Gold);
                break;
            case "when":
                if (!CTGGameSystem.GameStarted)
                {
                    caller.Reply("游戏未开始！");
                    break;
                }
                caller.Reply($"当前游戏时间：{CTGGameSystem.GameTime / 60}秒", Color.Gold);
                break;
            case "prepare":
                GiveStarterPack.TryGivePackToAllPlayers();
                break;
            default:
                caller.Reply($"命令错误：参数{args[0]}错误");
                break;
        }
    }
}