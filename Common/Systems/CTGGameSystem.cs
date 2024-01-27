using System.IO;
using System.Linq;
using CTGMod.Common.ModPlayers;
using CTGMod.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CTGMod.Common.Systems;

public class CTGGameSystem : ModSystem
{
    public static bool GameStarted = false;
    public static int GameTime;
    public static bool OncePerSecondCheck;
    public static bool RequestDisplayGameTime;
    
    public override void PreUpdateWorld()
    {
        if (GameStarted)
        {
            GameTime++;
            if (GameTime % 60 == 0)
            {
                OncePerSecondCheck = true;
            }

            if (GameTime % (60 * 60 * 5) == 0)
            {
                NetMessage.SendData(MessageID.WorldData);
            }

            if (RequestDisplayGameTime)
                RequestDisplayGameTime = false;

            if (GameTime % 60 == 0)
            {
                foreach (Player p in Main.player.Where(p => p != null && p.active))
                    p.GetModPlayer<CTGPlayer>().UpdateTimer++;
            }
        }
    }

    public override void PostUpdatePlayers()
    {
        if (Main.netMode == NetmodeID.Server)
            return;

        if (GameStarted)
        {
            if (OncePerSecondCheck)
            {
                if (GameTime % (60 * 60 * 5) == 0)
                    Main.NewText(($"游戏时间已经过{GameTime / 60}秒"), Color.Gold);
                OncePerSecondCheck = false;
            }
            else if (RequestDisplayGameTime)
            {
                Main.NewText(($"游戏时间已经过{GameTime / 60}秒"), Color.Gold);
                RequestDisplayGameTime = false;
            }
        }
    }
    
    public override void NetSend(BinaryWriter writer)
    {
        writer.Write((byte)CTGPacketID.SyncCTGWorld);
        writer.Write(GameStarted);
        writer.Write(GameTime);
        writer.Write(OncePerSecondCheck);
        writer.Write(RequestDisplayGameTime);
    }

    public override void NetReceive(BinaryReader reader)
    {
        switch (reader.ReadByte())
        {
            case CTGPacketID.SyncCTGWorld:
                GameStarted = reader.ReadBoolean();
                GameTime = reader.ReadInt32();
                OncePerSecondCheck = reader.ReadBoolean();
                RequestDisplayGameTime = reader.ReadBoolean();
                break;
        }
    }
}

public enum PlayerGroup
{
    Player,
    Spectator,
    Admin
}