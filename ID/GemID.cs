using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace CTGMod.ID;

public class GemID
{
    public const int LargeAmethyst = 0;
    public const int LargeTopaz = 1;
    public const int LargeSapphire = 2;
    public const int LargeEmerald = 3;
    public const int LargeRuby = 4;
    public const int LargeDiamond = 5;
    public const int LargeAmber = 6;

    public static int FromItemID(int id, bool noError = false)
    {
        if (id == ItemID.LargeAmethyst)
            return LargeAmethyst;
        if (id == ItemID.LargeTopaz)
            return LargeTopaz;
        if (id == ItemID.LargeSapphire)
            return LargeSapphire;
        if (id == ItemID.LargeEmerald)
            return LargeEmerald;
        if (id == ItemID.LargeRuby)
            return LargeRuby;
        if (id == ItemID.LargeDiamond)
            return LargeDiamond;
        if (id == ItemID.LargeAmber)
            return LargeAmber;
        return noError ? -1 : throw new Exception($"GemID.FromItemID(int): 输入了错误的宝石物品ID：{id}");
    }
    public static int FromNameToItemID(string name, bool noError = false)
    {
        return name switch
        {
            "LargeAmethyst" or "amethyst" => ItemID.LargeAmethyst,
            "LargeTopaz" or "topaz" => ItemID.LargeTopaz,
            "LargeSapphire" or "sapphire" => ItemID.LargeSapphire,
            "LargeEmerald" or "emerald" => ItemID.LargeEmerald,
            "LargeRuby" or "ruby" => ItemID.LargeRuby,
            "LargeDiamond" or "diamond" => ItemID.LargeDiamond,
            "LargeAmber" or "amber" => ItemID.LargeAmber,
            _ => noError ? -1 : throw new Exception($"GemID.FromName(string): 输入了错误的宝石名：{name}")
        };
    }

    public static string GemIDToString(int gemID, bool noError = false)
    {
        return gemID switch
        {
            LargeAmethyst => "Amethyst",
            LargeTopaz => "Topaz",
            LargeSapphire => "Sapphire",
            LargeEmerald => "Emerald",
            LargeRuby => "Ruby",
            LargeDiamond => "Diamond",
            LargeAmber => "Amber",
            _ => noError ? "" : throw new Exception($"GemID.GemIDToString(int): 输入了错误的宝石ID：{gemID}")
        };
    }

    public const int VanillaGemsCount = 7;

    public static List<int> Gems;

    public static readonly List<string> GemNames = new()
    {
        "Amber",
        "Amethyst",
        "Diamond",
        "Emerald",
        "Ruby",
        "Sapphire",
        "Topaz",
    };

    public static readonly Dictionary<string, Color> GemColors = new()
    {
        { "Amber", new Color(194, 83, 0) },
        { "Amethyst", new Color(241, 107, 255) },
        { "Diamond", new Color(139, 126, 188) },
        { "Emerald", new Color(19, 131, 87) },
        { "Ruby", new Color(155, 21, 18) },
        { "Sapphire", new Color(25, 33, 192) },
        { "Topaz", new Color(255, 197, 0) }
    };
}