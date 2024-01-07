using Terraria.ModLoader;

namespace CTGMod
{
	public class CTGMod : Mod
    {
        public static CTGMod Instance;

        internal static ModKeybind GemSlotKey;

        public override void Load()
        {
            Instance = this;
            GemSlotKey = KeybindLoader.RegisterKeybind(this, "GemSlot", "G");
        }

        public override void Unload()
        {
            GemSlotKey = null;
            Instance = null;
        }
    }
}