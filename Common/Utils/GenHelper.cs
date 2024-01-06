using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace CTGMod.Common.Utils;

public static class GenHelper
{
    public static bool CanGenerateStructure(Rectangle rectangle, Func<int, int, bool> checkFunc)
    {
        for (int x = rectangle.X; x < rectangle.Width; x++)
        {
            for (int y = rectangle.Y; y < rectangle.Height; y++)
            {
                if (!checkFunc(x, y))
                    return false;
            }
        }

        return true;
    }
}