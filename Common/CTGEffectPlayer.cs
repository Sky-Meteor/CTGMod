using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CTGMod.Common;

public class CTGEffectPlayer : ModPlayer
{
    public bool StrongPickaxe;

    //see DisableTeleportPatch
    public bool DisableTeleport;
    

    public override void ResetEffects()
    {
        StrongPickaxe = false;
        DisableTeleport = false;
    }

    public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
    {
        if (StrongPickaxe && item.pick > 0)
            damage *= 2;
    }
}