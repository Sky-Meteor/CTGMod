using System.Collections.Generic;
using System.Linq;
using CTGMod.ID;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace CTGMod;

public class CTGPlayer : ModPlayer
{
    public List<int> OwnedGems = new();

    public bool ShouldBeDrawnOnMap;

    private int _updateTimer;

    public override void ResetEffects()
    {
        if (++_updateTimer >= 10)
        {
            _updateTimer = 0;
            ShouldBeDrawnOnMap = false;

            if (Player.hostile)
            {
                int count = 0;
                foreach (var i in Player.inventory)
                {
                    if (i != null && GemID.Gems.Contains(i.type))
                    {
                        count++;
                        OwnedGems.Add(i.type);
                    }
                }

                if (count > 0)
                    ShouldBeDrawnOnMap = true;
            }
        }
    }

    public override void PostUpdateMiscEffects()
    {

    }
}