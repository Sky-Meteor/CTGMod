using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace CTGMod.Common.Utils;

public static class Extensions
{
    public static Item QuickSpawnItemDirect(this Player player, IEntitySource source, int type, int stack = 1, int prefix = -1)
    {
        int whoAmI = Item.NewItem(source, (int)player.position.X, (int)player.position.Y, player.width, player.height, type, stack, pfix:prefix, noGrabDelay:true);
        if (Main.netMode == NetmodeID.MultiplayerClient)
            NetMessage.SendData(MessageID.SyncItem, number: whoAmI, number2: 1f);
        return Main.item[whoAmI];
    }
}