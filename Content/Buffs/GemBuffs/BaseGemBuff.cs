using Terraria;
using Terraria.ModLoader;

namespace CTGMod.Content.Buffs.GemBuffs;

public abstract class BaseGemBuff : ModBuff
{
    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();
    }

    public override void Update(Player player, ref int buffIndex)
    {
        base.Update(player, ref buffIndex);
    }
}