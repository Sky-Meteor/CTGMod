using Terraria;
using Terraria.ModLoader;

namespace CTGMod.Common;

public class CTGEffectPlayer : ModPlayer
{
    public bool StrongPickaxe;
    public bool BossStriker;
    public bool StrongHook;

    //see DisableTeleportPatch
    public bool DisableTeleport;
    

    public override void ResetEffects()
    {
        StrongPickaxe = false;
        BossStriker = false;
        StrongHook = false;
        DisableTeleport = false;
    }

    public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
    {
        if (StrongPickaxe && item.pick > 0)
            damage *= 2;
    }

    public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
    {
        if (BossStriker && target.boss)
            modifiers.FinalDamage *= 2;
    }
}