using System.Collections.Generic;
using System.Linq;
using CTGMod.ID;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CTGMod.Common.Commands;

public class GiveStarterPack : ModCommand
{
    public override CommandType Type => CommandType.Chat;

    public override string Command => "starterpack";

    public override void Action(CommandCaller caller, string input, string[] args)
    {
        Player player = caller.Player;
        switch (args.Length)
        {
            case 0:
                string text1 = "第一个参数可选：amber/amethyst/diamond/emerald/ruby/sapphire/topaz | allplayer";
                caller.Reply(text1);
                break;
            case 1:
                switch (args[0])
                {
                    case "allplayer":
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
                    default:
                        if (!TryGivePack(player, args[0]))
                            caller.Reply($"第一个参数\"{args[0]}\"输入错误");
                        break;
                }
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
            if (item.Item3 != 0)
                i.Prefix(item.Item3);
        }

        player.QuickSpawnItemDirect(player.GetSource_GiftOrReward("CTGStarterPack"), gemType);
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