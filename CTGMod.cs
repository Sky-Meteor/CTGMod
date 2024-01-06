using Terraria.ModLoader;

namespace CTGMod
{
	public class CTGMod : Mod
    {
        public static CTGMod Instance;
        
        public override void Load()
        {
            Instance = this;
        }

        public override void Unload()
        {
            Instance = null;
        }
    }
}