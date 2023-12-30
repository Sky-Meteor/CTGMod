using Terraria;
using Terraria.ID;

namespace CTGMod.Content.Buffs.GemBuffs;

public class LargeAmberBuff : BaseGemBuff
{
    public override void Update(Player player, ref int buffIndex)
    {
        player.ApplyEquipFunctional(new Item(ItemID.EoCShield), false);
        player.ApplyEquipFunctional(new Item(ItemID.FrogLeg), false);
    }
}