using CTGMod.Common.ModPlayers;
using Terraria;
using Terraria.ModLoader;

namespace CTGMod.Common;

public class CTGGlobalProjectile : GlobalProjectile
{
    public override void GrapplePullSpeed(Projectile projectile, Player player, ref float speed)
    {
        if (player.GetModPlayer<CTGEffectPlayer>().StrongHook)
            speed *= 1.5f;
    }

    public override void GrappleRetreatSpeed(Projectile projectile, Player player, ref float speed)
    {
        if (player.GetModPlayer<CTGEffectPlayer>().StrongHook)
            speed *= 1.5f;
    }
}