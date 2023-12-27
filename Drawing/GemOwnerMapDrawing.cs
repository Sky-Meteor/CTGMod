using System;
using System.Collections.Generic;
using CTGMod.Drawing.Element;
using CTGMod.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.Map;
using Terraria.ModLoader;
using Terraria.UI;

namespace CTGMod.Drawing;

public class GemOwnerMapDrawing : ModMapLayer
{
    private int _timer;
    private bool _shouldUpdateGem;
    private static Dictionary<int, int> _currentDisplayingGem = new();
    public override void Draw(ref MapOverlayDrawContext context, ref string text)
    {
        _shouldUpdateGem = false;
        if (++_timer >= 60)
        {
            _shouldUpdateGem = true;
            _timer = 0;
        }
        foreach (Player p in Main.player)
        {
            if (p == null || !p.active)
                continue;
            CTGPlayer mp = p.GetModPlayer<CTGPlayer>();
            if (_shouldUpdateGem)
            {
                if (_currentDisplayingGem.ContainsKey(p.whoAmI))
                    _currentDisplayingGem[p.whoAmI]++;
                else
                    _currentDisplayingGem.Add(p.whoAmI, 0);
                if (_currentDisplayingGem[p.whoAmI] > mp.OwnedGems.Count - 1)
                    _currentDisplayingGem[p.whoAmI] = 0;
            }
            if (p.whoAmI == Main.myPlayer || !mp.ShouldBeDrawnOnMap || p.dead || p.ghost)
                continue;

            Vector2 position = p.Center / 16f;
            if (p.team != 0 && p.team == Main.LocalPlayer.team)
                position -= new Vector2(0, 5);
            if (context.Draw(GemMapIcons.GemMapIconContents[new Tuple<int, int>(GemID.FromItemID(mp.OwnedGems[_currentDisplayingGem[p.whoAmI]]), p.team)]
                    , position, Color.White, new SpriteFrame(1, 1, 0, 0), .65f, .65f, Alignment.Center).IsMouseOver)
                text = p.name;
        }
    }
}