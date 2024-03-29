﻿using System.Collections.Generic;
using System.IO;
using CTGMod.Common.ModPlayers;
using CTGMod.Common.Systems;
using CTGMod.Common.Utils;
using CTGMod.ID;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CTGMod
{
    public class CTGMod : Mod
    {
        public static CTGMod Instance;

        internal static ModKeybind GemSlotKey;

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            byte packetID = reader.ReadByte();
            Player player;
            CTGPlayer mp;
            switch (packetID)
            {
                case CTGPacketID.SyncGemSlotItems:
                    player = Main.player[reader.ReadByte()];
                    mp = player.GetModPlayer<CTGPlayer>();
                    mp.ItemTypesForSave = JsonConvert.DeserializeObject<IList<int>>(reader.ReadString());
                    if (CTGUtil.ServerCheck)
                        mp.SyncPlayer(-1, whoAmI, false);
                    break;
                case CTGPacketID.StartGame:
                    CTGGameSystem.GameStarted = true;
                    CTGGameSystem.GameTime = 0;
                    NetMessage.SendData(MessageID.WorldData);
                    break;
                case CTGPacketID.EndGame:
                    CTGGameSystem.GameStarted = false;
                    CTGGameSystem.GameTime = 0;
                    NetMessage.SendData(MessageID.WorldData);
                    break;
                case CTGPacketID.RequestGameTime:
                    CTGGameSystem.RequestDisplayGameTime = true;
                    NetMessage.SendData(MessageID.WorldData, whoAmI);
                    break;
                case CTGPacketID.SyncPlayerGroup:
                    player = Main.player[reader.ReadByte()];
                    mp = player.GetModPlayer<CTGPlayer>();
                    if (CTGUtil.ServerCheck)
                        mp.SyncPlayer(-1, whoAmI, false);
                    break;
            }
        }

        public override void Load()
        {
            Instance = this;
            GemSlotKey = KeybindLoader.RegisterKeybind(this, "GemSlot", "G");
        }

        public override void Unload()
        {
            GemSlotKey = null;
            Instance = null;
        }
    }
}