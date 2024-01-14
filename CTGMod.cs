using System.Collections.Generic;
using System.IO;
using CTGMod.Common.ModPlayers;
using CTGMod.ID;
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
            switch (packetID)
            {
                case CTGPacketID.SyncGemSlotItems:
                    Player player = Main.player[reader.ReadByte()];
                    CTGPlayer mp = player.GetModPlayer<CTGPlayer>();
                    mp.ItemTypesForSave = JsonConvert.DeserializeObject<IList<int>>(reader.ReadString());

                    if (Main.netMode == NetmodeID.Server)
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