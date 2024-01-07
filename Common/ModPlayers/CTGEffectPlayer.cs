using Terraria;
using Terraria.ModLoader;

namespace CTGMod.Common.ModPlayers;

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
    
    public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
    {
        if (BossStriker && target.boss)
            modifiers.FinalDamage *= 2;
    }
}