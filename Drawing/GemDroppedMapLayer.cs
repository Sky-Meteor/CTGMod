using System;
using System.Collections.Generic;
using CTGMod.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Map;
using Terraria.ModLoader;
using Terraria.UI;

namespace CTGMod.Drawing;

public class GemDroppedMapLayer : ModMapLayer
{
    public static List<Item> Gems = new();

    public override void Draw(ref MapOverlayDrawContext context, ref string text)
    {
        foreach (var item in Gems)
        {
            if (item != null && item.active && !item.IsAir)
            {
                Texture2D texture = GemMapIcons.GemMapIconContents[new Tuple<int, int>(GemID.FromItemID(item.type), 0)];
                context.Draw(texture, item.Center / 16f, Color.White, new SpriteFrame(1, 1, 0, 0), .75f, .75f, Alignment.Center);
            }
        }
        Gems.Clear();
    }
}