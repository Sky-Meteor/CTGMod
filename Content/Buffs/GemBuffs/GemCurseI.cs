using CTGMod.Common.ModPlayers;
using Terraria;
using Terraria.ID;

namespace CTGMod.Content.Buffs.GemBuffs;

public class GemCurseI : BaseGemBuff
{
    public override string Texture => "CTGMod/Content/Buffs/BuffPlaceholder";

    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();
        Main.debuff[Type] = true;
        BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
    }

    public override void Update(Player player, ref int buffIndex)
    {
        player.blind = true;
        player.GetModPlayer<CTGEffectPlayer>().DisableTeleport = true;
    }
}