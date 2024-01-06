using CTGMod.Common;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CTGMod.Content.Buffs.GemBuffs;

public class GemCurseIII : BaseGemBuff
{
    public override string Texture => "CTGMod/Content/Buffs/BuffPlaceholder";

    public override void Update(Player player, ref int buffIndex)
    {
        player.buffImmune[ModContent.BuffType<GemCurseI>()] = true;
        player.blind = true;
        player.GetModPlayer<CTGEffectPlayer>().DisableTeleport = true;

        player.buffImmune[ModContent.BuffType<GemCurseII>()] = true;
        player.blackout = true;
        // inferno ring
        player.inferno = true;

        player.buffImmune[BuffID.Weak] = true;
        player.GetDamage(DamageClass.Melee) -= 0.051f;
        player.GetAttackSpeed(DamageClass.Melee) -= 0.051f;
        player.statDefense -= 10;
        player.moveSpeed -= 0.2f;
    }
}