using CTGMod.Common.ModPlayers;
using Terraria;
using Terraria.ModLoader;

namespace CTGMod.Common.GlobalItems;

public class CTGEffectItem : GlobalItem
{
    public override void ModifyHitNPC(Item item, Player player, NPC target, ref NPC.HitModifiers modifiers)
    {
        if (player.GetModPlayer<CTGEffectPlayer>().StrongPickaxe && item.pick > 0)
            modifiers.FinalDamage *= 2;
    }

    public override void ModifyHitPvp(Item item, Player player, Player target, ref Player.HurtModifiers modifiers)
    {
        if (player.GetModPlayer<CTGEffectPlayer>().StrongPickaxe && item.pick > 0)
            modifiers.FinalDamage *= 2;
    }
}