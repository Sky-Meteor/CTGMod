using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CTGMod.Common.Systems;

public class UpdateArmorLightsPatch : ModSystem
{
    public override void Load()
    {
        On_Player.UpdateArmorLights += UpdateArmorLights;
    }

    // patch this because it is called right after updating large gems
    private void UpdateArmorLights(On_Player.orig_UpdateArmorLights orig, Player self)
    {
        if (self.gemCount == 0)
        {
            foreach (var i in UISystem.Instance.GemSlot.GetItemTypes())
            {
                int gem;

                if (i >= ItemID.LargeAmethyst && i <= ItemID.LargeDiamond)
                {
                    gem = i - ItemID.LargeAmethyst;
                    self.ownedLargeGems[gem] = true;
                }

                if (i == ItemID.LargeAmber)
                {
                    gem = 6;
                    self.ownedLargeGems[gem] = true;
                }
            }
        }

        orig.Invoke(self);
    }
}