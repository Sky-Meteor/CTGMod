using System;
using System.Collections.Generic;
using CTGMod.Drawing.Element;
using CTGMod.ID;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using Terraria;
using Terraria.GameContent;

namespace CTGMod.Drawing;

public class GemMapIcons
{
    public static Dictionary<Tuple<int, int>, Texture2D> GemMapIconContents = new();
    public static bool Loaded = false;
    public static void PrepareMapIcons()
    {
        GemMapIconContents.Clear();
        for (int gemID = 0; gemID < GemID.Gems.Count; gemID++)
        {
            for (int team = 0; team < Main.teamColor.Length; team++)
            {
                Texture2D texture = gemID <= 6 ? TextureAssets.Gem[gemID].Value : ModContent.Request<Texture2D>($"CTGMod/Assets/Gems/{gemID}").Value;
                var icon = new OutlinedMapIcon(texture, Main.teamColor[team]);
                icon.Request();
                icon.PrepareRenderTarget();
                GemMapIconContents.Add(new Tuple<int, int>(gemID, team), icon.GetTarget());
            }
        }
        Loaded = true;
    }
}