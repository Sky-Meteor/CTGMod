using CTGMod.Common.ModPlayers;
using CTGMod.Common.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CTGMod.Common.Systems;

public class DisableTeleportPatch : ModSystem
{
    public override void Load()
    {
        On_Player.Teleport += Teleport;
        On_Player.Spawn += Spawn;
    }

    private void Spawn(On_Player.orig_Spawn orig, Player self, PlayerSpawnContext context)
    {
        if (context == PlayerSpawnContext.RecallFromItem && self.GetModPlayer<CTGEffectPlayer>().DisableTeleport)
        {
            if (self.whoAmI == Main.myPlayer)
                CTGUtil.NewLocalizedText("CannotTpCurse", Color.Red);
            return;
        }
        orig.Invoke(self, context);
    }

    private void Teleport(On_Player.orig_Teleport orig, Player self, Vector2 newPos, int Style, int extraInfo)
    {
        // shimmer unstuck -> 12
        if (!self.GetModPlayer<CTGEffectPlayer>().DisableTeleport || self.GetModPlayer<CTGPlayer>().TryUseTeleportation || Style == 12)
            orig.Invoke(self, newPos, Style, extraInfo);
        else
        {
            self.RemoveAllGrapplingHooks();
            if (self.whoAmI == Main.myPlayer)
                CTGUtil.NewLocalizedText("CannotTpCurse", Color.Red);
        }
    }

    public override bool HijackSendData(int whoAmI, int msgType, int remoteClient, int ignoreClient, NetworkText text, int number, float number2, float number3, float number4, int number5, int number6, int number7)
    {
        if (msgType == MessageID.TeleportEntity && Main.player[(int)number2].GetModPlayer<CTGEffectPlayer>().DisableTeleport && !Main.player[(int)number2].GetModPlayer<CTGPlayer>().TryUseTeleportation && number5 != 12)
            return true;
        if (msgType == MessageID.PlayerSpawn && Main.player[number].GetModPlayer<CTGEffectPlayer>().DisableTeleport && (PlayerSpawnContext)(int)number2 == PlayerSpawnContext.RecallFromItem)
            return true;
        return base.HijackSendData(whoAmI, msgType, remoteClient, ignoreClient, text, number, number2, number3, number4, number5, number6, number7);
    }
}