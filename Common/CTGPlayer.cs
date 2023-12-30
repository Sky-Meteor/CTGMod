using System.Collections.Generic;
using CTGMod.Content.Buffs.GemBuffs;
using CTGMod.ID;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CTGMod.Common;

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
            OwnedGems.Clear();

            int count = 0;
            foreach (var i in Player.inventory)
            {
                if (i != null && GemID.Gems.Contains(i.type))
                {
                    count++;
                    OwnedGems.Add(i.type);
                }
            }

            if (count > 0 && Player.hostile)
                ShouldBeDrawnOnMap = true;
        }
    }

    public override void PostUpdateMiscEffects()
    {
        foreach (int gem in OwnedGems)
        {
            switch (gem)
            {
                case ItemID.LargeAmber:
                    Player.AddBuff(ModContent.BuffType<LargeAmberBuff>(), 2);
                    break;
                case ItemID.LargeAmethyst:
                    Player.AddBuff(ModContent.BuffType<LargeAmethystBuff>(), 2);
                    break;

            }
        }
    }
}