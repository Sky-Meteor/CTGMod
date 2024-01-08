using System.Collections.Generic;
using CTGMod.UI;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace CTGMod.Common.Systems;

public class UISystem : ModSystem
{
    internal GemSlot GemSlot;
    internal UserInterface GemSlotUserInterface;

    internal static UISystem Instance => ModContent.GetInstance<UISystem>();

    public override void Load()
    {
        GemSlot = new GemSlot();
        GemSlot.Activate();

        GemSlotUserInterface = new UserInterface();
        GemSlotUserInterface.SetState(GemSlot);
    }

    public override void UpdateUI(GameTime gameTime)
    {
        GemSlotUserInterface?.Update(gameTime);
    }

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        int index = layers.FindIndex(l => l.Name == "Vanilla: Inventory");

        layers.Insert(index + 1, new LegacyGameInterfaceLayer("CTGMod: Gem Slot", () =>
        {
            if (GemSlot.Visible)
                GemSlot.Draw(Main.spriteBatch);
            return true;
        }, InterfaceScaleType.UI));
    }

    public override void Unload()
    {
        GemSlotUserInterface = null;
        GemSlot = null;
    }
}