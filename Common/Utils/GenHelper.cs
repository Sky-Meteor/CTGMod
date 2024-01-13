using System;
using Microsoft.Xna.Framework;

namespace CTGMod.Common.Utils;

public static class GenHelper
{
    public static bool CanGenerateStructure(Rectangle rectangle, Func<int, int, bool> checkFunc)
    {
        for (int x = 0; x < rectangle.Width; x++)
        {
            for (int y = 0; y < rectangle.Height; y++)
            {
                if (!checkFunc(rectangle.X + x, rectangle.Y + y))
                    return false;
            }
        }

        return true;
    }
}