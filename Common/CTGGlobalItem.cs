using System.Collections.Generic;
using System.Linq;
using CTGMod.Drawing;
using CTGMod.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static CTGMod.Common.Utils.TooltipHelper;

namespace CTGMod.Common;

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
            tooltips.Add(new TooltipLine(Mod, "CTGLargeGem0", LocalizedItemColorTooltipText(type, Color.LightBlue, "DrawPlayer")));
            tooltips.Add(new TooltipLine(Mod, "CTGLargeGem1", LocalizedItemColorTooltipText(type, Color.LightBlue, "DrawGem")));
            tooltips.Add(new TooltipLine(Mod, "CTGLargeGem2", LocalizedItemColorTooltipText(ItemID.Torch, Color.Gold, "GemGlow")));
        }
    }

    public override void PostUpdate(Item item)
    {
        if (GemID.Gems.Contains(item.type))
        {
            GemDroppedMapDrawing.Gems.Add(item);
            Lighting.AddLight(item.position, Color.Gold.ToVector3());
        }
    }

    //public static string LocalizedTooltip(string key) => $"Mods.CTGMod.Common.Tooltips.{key}";
}