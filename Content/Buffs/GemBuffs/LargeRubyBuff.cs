using Terraria;
using Terraria.ID;

namespace CTGMod.Content.Buffs.GemBuffs;

public class LargeRubyBuff : BaseGemBuff
{
    public override void Update(Player player, ref int buffIndex)
    {
        player.statLifeMax2 += player.statLifeMax / 5;
        player.lifeRegen += 4;
        player.lifeMagnet = true;
    }
}