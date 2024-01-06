using System.Collections.Generic;
using CTGMod.Content.Buffs.GemBuffs;
using CTGMod.ID;
using Terraria.DataStructures;
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
                    i.favorited = true;
                }
            }

            if (count > 0 && Player.hostile)
                ShouldBeDrawnOnMap = true;
        }
    }

    public override void PreUpdate()
    {
        if (GemID.Gems.Contains(Player.trashItem.type))
            Player.KillMe(PlayerDeathReason.ByCustomReason($"{Player.name}遭到天谴。"), 999, 1);
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
                case ItemID.LargeDiamond:
                    Player.AddBuff(ModContent.BuffType<LargeDiamondBuff>(), 2);
                    break;
                case ItemID.LargeEmerald:
                    Player.AddBuff(ModContent.BuffType<LargeEmeraldBuff>(), 2);
                    break;
                case ItemID.LargeRuby:
                    Player.AddBuff(ModContent.BuffType<LargeRubyBuff>(), 2);
                    break;
                case ItemID.LargeSapphire:
                    Player.AddBuff(ModContent.BuffType<LargeSapphireBuff>(), 2);
                    break;
                case ItemID.LargeTopaz:
                    Player.AddBuff(ModContent.BuffType<LargeTopazBuff>(), 2);
                    break;
            }
        }

        if (OwnedGems.Count == 3)
            Player.AddBuff(ModContent.BuffType<GemCurseI>(), 2);
        else if (OwnedGems.Count == 4)
            Player.AddBuff(ModContent.BuffType<GemCurseII>(), 2);
        else if (OwnedGems.Count >= 5)
            Player.AddBuff(ModContent.BuffType<GemCurseIII>(), 2);
    }

    public override void PostUpdateBuffs()
    {
        int gravIndex = Player.FindBuffIndex(BuffID.Gravitation);
        if (gravIndex != -1 && Player.buffTime[gravIndex] > 60 * 60)
            Player.buffTime[gravIndex] = 60 * 60;
    }
}