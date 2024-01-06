using Terraria;
using Terraria.ModLoader;

namespace CTGMod.Content.Buffs.GemBuffs;

public class LargeSapphireBuff : BaseGemBuff
{
    public override void Update(Player player, ref int buffIndex)
    {
        player.GetDamage(DamageClass.Summon) += .5f;
    }
}