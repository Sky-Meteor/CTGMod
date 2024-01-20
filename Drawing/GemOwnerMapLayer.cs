using System;
using System.Collections.Generic;
using CTGMod.Common.ModPlayers;
using CTGMod.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Map;
using Terraria.ModLoader;
using Terraria.UI;

namespace CTGMod.Drawing;

public class GemOwnerMapLayer : ModMapLayer
{
    private int _timer;
    private bool _shouldCycleGem;
    private static Dictionary<int, int> _currentDisplayingGem = new();
    public override void Draw(ref MapOverlayDrawContext context, ref string text)
    {
        _shouldCycleGem = false;
        if (++_timer >= 60)
        {
            _shouldCycleGem = true;
            _timer = 0;
        }
        foreach (Player p in Main.player)
        {
            if (p == null || !p.active)
                continue;
            CTGPlayer mp = p.GetModPlayer<CTGPlayer>();

            _currentDisplayingGem.TryAdd(p.whoAmI, 0);

            if (_shouldCycleGem)
                _currentDisplayingGem[p.whoAmI]++;

            if (!mp.ShouldBeDrawnOnMap || mp.OwnedGems.Count == 0 || p.dead || p.ghost)
                continue;

            if (_currentDisplayingGem[p.whoAmI] > mp.OwnedGems.Count - 1)
                _currentDisplayingGem[p.whoAmI] = 0;

            Vector2 position = mp.DrawCenter / 16f;
            if (position == Vector2.Zero)
                return;

            if (context.Draw(GemMapIcons.GemMapIconContents[new Tuple<int, int>(GemID.FromItemID(mp.OwnedGems[_currentDisplayingGem[p.whoAmI]]), p.team)]
                    , position, Color.White, new SpriteFrame(1, 1, 0, 0), .65f, .65f, Alignment.Center).IsMouseOver)
                text = p.name;
        }
    }
}