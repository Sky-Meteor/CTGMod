using System.Collections.Generic;
using CTGMod.Common.Systems;
using CTGMod.Drawing;
using CTGMod.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static CTGMod.Common.Utils.TooltipHelper;

namespace CTGMod.Common.GlobalItems;

public class CTGGlobalItem : GlobalItem
{
    public override void Load()
    {
        GemID.Gems = new()
        {
            ItemID.LargeAmethyst, ItemID.LargeAmber, ItemID.LargeDiamond, ItemID.LargeEmerald, ItemID.LargeRuby,
            ItemID.LargeSapphire, ItemID.LargeTopaz
        };
    }

    public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
    {
        int type = item.type;
        if (GemID.Gems.Contains(type))
        {
            tooltips.Add(new TooltipLine(Mod, "CTGLargeGem0", LocalizedItemColorTooltipText(type, Color.LightGreen, "GemBuff")));
            tooltips.Add(new TooltipLine(Mod, "CTGLargeGem1", LocalizedItemColorTooltipText(type, Color.LightBlue, "DrawPlayer")));
            tooltips.Add(new TooltipLine(Mod, "CTGLargeGem2", LocalizedItemColorTooltipText(type, Color.LightBlue, "DrawGem")));
            tooltips.Add(new TooltipLine(Mod, "CTGLargeGem3", LocalizedItemColorTooltipText(ItemID.Torch, Color.Gold, "GemGlow")));
        }
    }

    public override void PostUpdate(Item item)
    {
        if (GemID.Gems.Contains(item.type))
        {
            GemDroppedMapLayer.Gems.Add(item);
            Lighting.AddLight(item.position, Color.Gold.ToVector3());
        }
    }

    public override bool CanRightClick(Item item)
    {
        if (GemID.Gems.Contains(item.type) && UISystem.Instance.GemSlot.TryInsert(item))
        {
            item.TurnToAir();
            return true;
        }

        return false;
    }
}