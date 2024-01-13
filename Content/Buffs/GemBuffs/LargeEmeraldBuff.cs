using CTGMod.Common.ModPlayers;
using Terraria;
using Terraria.ModLoader;

namespace CTGMod.Content.Buffs.GemBuffs;

public class LargeEmeraldBuff : BaseGemBuff
{
    public override void Update(Player player, ref int buffIndex)
    {
        player.GetModPlayer<CTGEffectPlayer>().StrongHook = true;
        player.GetKnockback(DamageClass.Generic) *= 1.5f;
    }
}