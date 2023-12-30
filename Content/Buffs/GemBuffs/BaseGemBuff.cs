using Terraria;
using Terraria.ModLoader;

namespace CTGMod.Content.Buffs.GemBuffs;

public abstract class BaseGemBuff : ModBuff
{
    public override void SetStaticDefaults()
    {
        Main.buffNoTimeDisplay[Type] = true;
        Main.buffNoSave[Type] = true;
    }
}