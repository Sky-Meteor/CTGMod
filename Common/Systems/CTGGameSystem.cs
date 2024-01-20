using System.IO;
using System.Linq;
using CTGMod.Common.ModPlayers;
using CTGMod.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CTGMod.Common.Systems;

public class CTGGameSystem : ModSystem
{
    public static bool GameStarted = false;
    public static int GameTime;
    public static bool OncePerSecondCheck;
    
    public override void PreUpdateWorld()
    {
        if (GameStarted)
        {
            GameTime++;
            if (GameTime % 60 == 0)
            {
                OncePerSecondCheck = true;
                NetMessage.SendData(MessageID.WorldData);
            }
        }
    }

    public override void PostUpdatePlayers()
    {
        if (Main.netMode == NetmodeID.Server)
            return;

        if (GameStarted && OncePerSecondCheck)
        {
            OncePerSecondCheck = false;
            
            if (GameTime % (60 * 60 * 5) == 0)
                Main.NewText(($"游戏时间已经过{GameTime / 60}秒"), Color.Gold);
        }
    }
    
    public override void NetSend(BinaryWriter writer)
    {
        writer.Write((byte)CTGPacketID.SyncCTGWorld);
        writer.Write(GameStarted);
        writer.Write(GameTime);
        writer.Write(OncePerSecondCheck);
    }

    public override void NetReceive(BinaryReader reader)
    {
        switch (reader.ReadByte())
        {
            case CTGPacketID.SyncCTGWorld:
                GameStarted = reader.ReadBoolean();
                GameTime = reader.ReadInt32();
                OncePerSecondCheck = reader.ReadBoolean();
                foreach (Player p in Main.player.Where(p => p != null && p.active))
                    p.GetModPlayer<CTGPlayer>().UpdateTimer++;
                break;
        }
    }
}