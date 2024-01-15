using CTGMod.Common.ModPlayers;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CTGMod.Content.Buffs.GemBuffs;

public class GemCurseII : BaseGemBuff
{
    public override string Texture => "CTGMod/Content/Buffs/DebuffPlaceholder";

    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();
        Main.debuff[Type] = true;
        BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
    }

    public override void Update(Player player, ref int buffIndex)
    {
        player.buffImmune[ModContent.BuffType<GemCurseI>()] = true;
        player.blind = true;
        player.GetModPlayer<CTGEffectPlayer>().DisableTeleport = true;

        player.blackout = true;
        // inferno ring
        player.inferno = true;
    }
}