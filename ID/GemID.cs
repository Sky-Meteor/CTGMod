﻿using System;
using System.Collections.Generic;
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

    public static int FromItemID(int id)
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
        throw new Exception($"GemID.FromItemID(int): 输入了错误的宝石物品ID：{id}");
    }
    public static int FromNameToItemID(string name)
    {
        return name switch
        {
            "LargeAmethyst" => ItemID.LargeAmethyst,
            "LargeTopaz" => ItemID.LargeTopaz,
            "LargeSapphire" => ItemID.LargeSapphire,
            "LargeEmerald" => ItemID.LargeEmerald,
            "LargeRuby" => ItemID.LargeRuby,
            "LargeDiamond" => ItemID.LargeDiamond,
            "LargeAmber" => ItemID.LargeAmber,
            _ => throw new Exception($"GemID.FromName(string): 输入了错误的宝石名：{name}")
        };
    }

    public static List<int> Gems;
}