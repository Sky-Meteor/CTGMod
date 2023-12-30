using CTGMod.Common;
using Terraria;
using Terraria.ID;

namespace CTGMod.Content.Buffs.GemBuffs;

public class LargeTopazBuff : BaseGemBuff
{
    public override void Update(Player player, ref int buffIndex)
    {
        player.pickSpeed -= .25f;
        // spelunker
        player.findTreasure = true;
        player.GetModPlayer<CTGEffectPlayer>().StrongPickaxe = true;
    }
}