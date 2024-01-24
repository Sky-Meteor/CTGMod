using System.Collections.Generic;
using System.Linq;
using CTGMod.Common.Configs;
using CTGMod.Common.Systems;
using CTGMod.Content.Buffs.GemBuffs;
using CTGMod.ID;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CTGMod.Common.ModPlayers;

public class CTGPlayer : ModPlayer
{
    public List<int> OwnedGems = new();

    public bool ShouldBeDrawnOnMap;

    /// <summary>
    /// A Timer in second that syncs with CTGGameSystem
    /// </summary>
    public int UpdateTimer;

    public bool TryUseTeleportation;

    public Vector2 DrawCenter = Vector2.Zero;

    public override void ResetEffects()
    {
        if (Player.gemCount == 1)
        {
            ShouldBeDrawnOnMap = false;
            OwnedGems.Clear();

            int count = 0;
            foreach (var i in Player.inventory)
            {
                if (i != null && GemID.Gems.Contains(i.type))
                {
                    count++;
                    OwnedGems.Add(i.type);
                    i.favorited = true;
                }
            }

            for (var i = 0; i < ItemTypesForSave.Count; i++)
            {
                var type = ItemTypesForSave[i];
                if (GemID.Gems.Contains(type))
                {
                    count++;
                    OwnedGems.Add(type);
                }
                else
                {
                    if (Player.whoAmI == Main.myPlayer && UISystem.GemSlot.ItemsRef[i].type == type)
                        Player.DropItem(Player.GetSource_Misc("CTGGemSlotInvalidItem"), Player.Center, ref UISystem.GemSlot.ItemsRef[i]);
                }
            }

            if (count > 0 && Player.hostile)
                ShouldBeDrawnOnMap = true;
        }

        if (UpdateTimer >= CTGConfig.Instance.PlayerPositionUpdateTime)
        {
            UpdateTimer = 0;
            DrawCenter = Player.Center;
        }
    }

    public override void PreUpdate()
    {
        if (GemID.Gems.Contains(Player.trashItem.type))
        {
            Player.DropItem(Player.GetSource_DropAsItem(), Player.Center, ref Player.trashItem);
            if (Player.whoAmI == Main.myPlayer)
            {
                Main.NewText("禁止把宝石丢进垃圾桶！", Color.Red);
            }
        }
    }
    
    public override void PostUpdateMiscEffects()
    {
        foreach (int gem in OwnedGems)
        {
            switch (gem)
            {
                case ItemID.LargeAmber:
                    Player.AddBuff(ModContent.BuffType<LargeAmberBuff>(), 2);
                    break;
                case ItemID.LargeAmethyst:
                    Player.AddBuff(ModContent.BuffType<LargeAmethystBuff>(), 2);
                    break;
                case ItemID.LargeDiamond:
                    Player.AddBuff(ModContent.BuffType<LargeDiamondBuff>(), 2);
                    break;
                case ItemID.LargeEmerald:
                    Player.AddBuff(ModContent.BuffType<LargeEmeraldBuff>(), 2);
                    break;
                case ItemID.LargeRuby:
                    Player.AddBuff(ModContent.BuffType<LargeRubyBuff>(), 2);
                    break;
                case ItemID.LargeSapphire:
                    Player.AddBuff(ModContent.BuffType<LargeSapphireBuff>(), 2);
                    break;
                case ItemID.LargeTopaz:
                    Player.AddBuff(ModContent.BuffType<LargeTopazBuff>(), 2);
                    break;
            }
        }

        if (OwnedGems.Count == 3)
            Player.AddBuff(ModContent.BuffType<GemCurseI>(), 2);
        else if (OwnedGems.Count == 4)
            Player.AddBuff(ModContent.BuffType<GemCurseII>(), 2);
        else if (OwnedGems.Count >= 5)
            Player.AddBuff(ModContent.BuffType<GemCurseIII>(), 2);

        if (TryUseTeleportation)
        {
            Player.TeleportationPotion();
            TryUseTeleportation = false;
        }
    }

    public override void PostUpdateBuffs()
    {
        int gravIndex = Player.FindBuffIndex(BuffID.Gravitation);
        if (gravIndex != -1 && Player.buffTime[gravIndex] > 60 * 60)
            Player.buffTime[gravIndex] = 60 * 60;
    }

    public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
    {
        if (Player.whoAmI == Main.myPlayer)
        {
            for (int i = 0; i < 10; i++)
            {
                Player.DropItem(Player.GetSource_Death(), Player.Center, ref UISystem.GemSlot.ItemsRef[i]);
            }
        }
    }

    public override bool OnPickup(Item item)
    {
        if (Player.whoAmI == Main.myPlayer && CTGConfig.Instance.AutoGemSlot && GemID.Gems.Contains(item.type))
        {
            int index = UISystem.GemSlot.ItemsRef.ToList().FindIndex(i => i.type == ItemID.None);
            if (index != -1)
            {
                UISystem.GemSlot.ItemsRef[index] = item;
                PopupText.NewText(PopupTextContext.ItemPickupToVoidContainer, item, 1); // manually display pickup text
                UISystem.GemSlot.UpdateItemTypesForSave();
                return false;
            }
        }
        return base.OnPickup(item);
    }

    public IList<int> ItemTypesForSave = new List<int>();
    public override void SaveData(TagCompound tag)
    {
        if (Player.whoAmI == Main.myPlayer)
            tag.Add("CTGGemSlotTypes", ItemTypesForSave.ToList());
    }
    
    public IList<int> ItemTypesForLoad = new List<int>();
    public override void LoadData(TagCompound tag)
    {
        if (Player.whoAmI == Main.myPlayer)
        {
            IList<int> gemSlotItemList = tag.GetList<int>("CTGGemSlotTypes");
            ItemTypesForLoad = new List<int>();
            ItemTypesForLoad = gemSlotItemList.ToList();
        }
    }
    
    public override void ProcessTriggers(TriggersSet triggersSet)
    {
        if (CTGMod.GemSlotKey.JustPressed && Main.LocalPlayer.chest == -1)
        {
            UISystem.GemSlot.Visible = !UISystem.GemSlot.Visible;

            if (UISystem.GemSlot.Visible && !Main.playerInventory)
                Main.playerInventory = true;
        }
    }

    public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
    {
        ModPacket packet = Mod.GetPacket();
        packet.Write((byte)CTGPacketID.SyncGemSlotItems);
        packet.Write((byte)Player.whoAmI);
        packet.Write(JsonConvert.SerializeObject(ItemTypesForSave));
        packet.Send(toWho, fromWho);
    }

    public override void CopyClientState(ModPlayer targetCopy)
    {
        if (targetCopy is not CTGPlayer mp)
            return;
        mp.ItemTypesForSave = ItemTypesForSave.ToList();
    }
    

    public override void SendClientChanges(ModPlayer clientPlayer)
    {
        if (clientPlayer is not CTGPlayer mp)
            return;

        if (!mp.ItemTypesForSave.SequenceEqual(ItemTypesForSave))
        {
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)CTGPacketID.SyncGemSlotItems);
            packet.Write((byte)Player.whoAmI);
            packet.Write(JsonConvert.SerializeObject(ItemTypesForSave));
            packet.Send(-1, Player.whoAmI);
        }
    }

    public override void Load()
    {
        OwnedGems = new List<int>();
        DrawCenter = Vector2.Zero;
        ItemTypesForSave = new List<int>();
        ItemTypesForLoad = new List<int>();
    }

    public override void Unload()
    {
        OwnedGems = null;
        DrawCenter = Vector2.Zero;
        ItemTypesForSave = null;
        ItemTypesForLoad = null;
    }
}

public enum PlayerGroup
{
    Player,
    Spectator,
    Admin
}