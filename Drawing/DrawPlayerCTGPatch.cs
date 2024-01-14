using CTGMod.Common.ModPlayers;
using CTGMod.Common.Systems;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CTGMod.Drawing;

public class DrawPlayerCTGPatch : PlayerDrawLayer
{
    public override Position GetDefaultPosition()
    {
        return new BeforeParent(PlayerDrawLayers.CaptureTheGem);
    }

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        if (drawInfo.shadow != 0f || !drawInfo.drawPlayer.active)
            return;

        ref Player player = ref drawInfo.drawPlayer;
        
        if (player.gemCount == 0)
        {
            foreach (int i in player.GetModPlayer<CTGPlayer>().ItemTypesForSave)
            {
                int gem;
                
                if (i >= ItemID.LargeAmethyst && i <= ItemID.LargeDiamond)
                {
                    gem = i - ItemID.LargeAmethyst;
                    player.ownedLargeGems[gem] = true;
                }

                if (i == ItemID.LargeAmber)
                {
                    gem = 6;
                    player.ownedLargeGems[gem] = true;
                }
            }
        }
    }
}