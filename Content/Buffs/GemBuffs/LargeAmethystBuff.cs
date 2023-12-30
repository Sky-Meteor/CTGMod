using Terraria;
using Terraria.ModLoader;

namespace CTGMod.Content.Buffs.GemBuffs;

public class LargeAmethystBuff : BaseGemBuff
{
    public override void Update(Player player, ref int buffIndex)
    {
        player.GetDamage(DamageClass.Magic) += .3f;
        player.manaRegenBuff = true;
        player.statManaMax2 += 60;
    }
}