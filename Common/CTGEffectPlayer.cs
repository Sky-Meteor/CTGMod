using Terraria;
using Terraria.ModLoader;

namespace CTGMod.Common;

public class CTGEffectPlayer : ModPlayer
{
    public bool StrongPickaxe;

    public override void ResetEffects()
    {
        StrongPickaxe = false;
    }

    public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
    {
        if (StrongPickaxe && item.pick > 0)
            damage *= 2;
    }
}