using CTGMod.Common;
using Terraria;

namespace CTGMod.Content.Buffs.GemBuffs;

public class LargeDiamondBuff : BaseGemBuff
{
    public override void Update(Player player, ref int buffIndex)
    {
        player.GetModPlayer<CTGEffectPlayer>().BossStriker = true;
    }
}