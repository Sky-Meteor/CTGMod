using CTGMod.Drawing;
using Terraria;
using Terraria.ModLoader;

namespace CTGMod.Common.Systems;

public class DoDrawPatch : ModSystem
{
    public override void Load()
    {
        On_Main.DoDraw += DoDraw;
    }

    private void DoDraw(On_Main.orig_DoDraw orig, Main self, Microsoft.Xna.Framework.GameTime gameTime)
    {
        if (!GemMapIcons.Loaded)
            GemMapIcons.PrepareMapIcons();
        orig.Invoke(self, gameTime);
    }

    public override void Unload()
    {
        On_Main.DoDraw -= DoDraw;
    }
}