using CTGMod.Common;
using Terraria;

namespace CTGMod.Content.Buffs.GemBuffs;

public class GemCurseI : BaseGemBuff
{
    public override string Texture => "CTGMod/Content/Buffs/BuffPlaceholder";

    public override void Update(Player player, ref int buffIndex)
    {
        player.blind = true;
        player.GetModPlayer<CTGEffectPlayer>().DisableTeleport = true;
    }
}