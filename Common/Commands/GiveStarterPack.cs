using System.Collections.Generic;
using System.Linq;
using CTGMod.ID;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CTGMod.Common.Commands;

public class GiveStarterPack : ModCommand
{
    public override CommandType Type => CommandType.Chat;

    public override string Command => "starterpack";

    public override string Description => "给予宝石争夺战开局礼包";

    public override string Usage => "参数可选：amber/amethyst/diamond/emerald/ruby/sapphire/topaz 给自己指定礼包\n" +
                                    "allplayer/ap 给所有玩家随机礼包\n" +
                                    "player/p + 玩家名中的任意连续部分（留空为给自己） 给某个玩家随机礼包\n" +
                                    "player/p + 玩家名中的任意连续部分 + amber/amethyst/diamond/emerald/ruby/sapphire/topaz 给某个玩家指定礼包";

    public override void Action(CommandCaller caller, string input, string[] args)
    {
        Player player = caller.Player;
        switch (args.Length)
        {
            case 0:
                string text1 = Usage;
                caller.Reply(text1);
                break;
            case 1:
                switch (args[0])
                {
                    case "allplayer":
                    case "ap":
                        var list = GemID.Gems.ToList();
                        for (int i = 0; i < 7; i++)
                            list.Sort((_, _) => Main.rand.NextBool() ? 1 : -1);
                        int packCount = 0;

                        foreach (Player p in Main.player)
                        {
                            if (p != null && p.active)
                                TryGivePack(p, list[packCount]);
                            if (++packCount >= 7)
                            {
                                packCount = 0;
                                for (int i = 0; i < 7; i++)
                                    list.Sort((_, _) => Main.rand.NextBool() ? 1 : -1);
                            }
                        }
                        break;
                    case "player":
                    case "p":
                        TryGivePack(player, Main.rand.Next(GemID.Gems));
                        break;
                    default:
                        if (!TryGivePack(player, args[0]))
                            caller.Reply($"礼包类型参数\"{args[0]}\"输入错误");
                        break;
                }
                break;
            case 2:
                switch (args[0])
                {
                    case "player":
                    case "p":
                        foreach (var p in Main.player)
                        {
                            if (p != null && p.active && p.name.Contains(args[1]) && !p.dead && !p.ghost)
                            {
                                TryGivePack(p, Main.rand.Next(GemID.Gems));
                                return;
                            }
                        }
                        caller.Reply($"找不到名称包含\"{args[1]}\"的玩家");
                        break;
                }
                break;
            case 3:
                switch (args[0])
                {
                    case "player":
                    case "p":
                        foreach (var p in Main.player)
                        {
                            if (p != null && p.active && p.name.Contains(args[1]) && !p.dead && !p.ghost)
                            {
                                if (!TryGivePack(p, args[2]))
                                    caller.Reply($"礼包类型参数\"{args[2]}\"输入错误");
                                return;
                            }
                        }
                        caller.Reply($"找不到名称包含\"{args[1]}\"的玩家");
                        break;
                }
                break;
            default:
                caller.Reply("命令错误：参数过长");
                break;
        }
    }

    public static bool TryGivePack(Player player, string gemType)
    {
        int id = GemID.FromNameToItemID(gemType, noError: true);
        if (id == -1)
            return false;

        TryGivePack(player, id);
        return true;
    }

    public static void TryGivePack(Player player, int gemType)
    {
        foreach (var item in StarterPackList)
        {
            Item i = player.QuickSpawnItemDirect(player.GetSource_GiftOrReward("CTGStarterPack"), item.Item1, item.Item2);
            i.prefix = 0;
            if (item.Item3 != 0)
                i.Prefix(item.Item3);
        }

        player.QuickSpawnItemDirect(player.GetSource_GiftOrReward("CTGStarterPack"), gemType);
        if (gemType == ItemID.LargeEmerald)
            player.QuickSpawnItemDirect(player.GetSource_GiftOrReward("CTGStarterPack"), ItemID.WebSlinger); // 蛛丝吊索
    }

    public static readonly List<(int, int, int)> StarterPackList = new ()
    {
        (ItemID.LifeCrystal, 10, 0),
        (ItemID.BonePickaxe, 1, PrefixID.Light),
        (ItemID.Wood, 400, 0),
        (ItemID.Torch, 100, 0),
        (ItemID.IronBar, 15, 0),
        (ItemID.GoldenDelight, 1, 0),
        (ItemID.TeleportationPotion, 1, 0),
        (ItemID.GPS, 1, 0),
        (ItemID.CloudinaBottle, 1, 0)
    };
}