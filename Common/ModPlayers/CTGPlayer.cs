using System.Collections.Generic;
using System.Linq;
using CTGMod.Common.Configs;
using CTGMod.Common.Systems;
using CTGMod.Content.Buffs.GemBuffs;
using CTGMod.ID;
using Microsoft.Xna.Framework;
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

    public static int UpdateTimer;

    public Vector2 DrawCenter = Vector2.Zero;

    public override void ResetEffects()
    {
        if (CTGConfig.Instance.PlayerPositionUpdateTime < 10 ? UpdateTimer >= CTGConfig.Instance.PlayerPositionUpdateTime : UpdateTimer % 10 == 0)
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

            for (var i = 0; i < UISystem.Instance.GemSlot.ItemsRef.Length; i++)
            {
                ref var item = ref UISystem.Instance.GemSlot.ItemsRef[i];
                if (GemID.Gems.Contains(item.type))
                {
                    count++;
                    OwnedGems.Add(item.type);
                }
                else
                {
                    Player.DropItem(Player.GetSource_Misc("CTGGemSlotInvalidItem"), Player.Center, ref item);
                }
            }

            if (count > 0 && Player.hostile)
                ShouldBeDrawnOnMap = true;
        }

        if (UpdateTimer >= CTGConfig.Instance.PlayerPositionUpdateTime)
        {
            UpdateTimer = 0;
            if (ShouldBeDrawnOnMap)
                DrawCenter = Player.Center;
        }
    }

    public override void PreUpdate()
    {
        //if (GemID.Gems.Contains(Player.trashItem.type))
        //Player.KillMe(PlayerDeathReason.ByCustomReason($"{Player.name}遭到天谴。"), 999, 1);
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
                Player.DropItem(Player.GetSource_Death(), Player.Center, ref UISystem.Instance.GemSlot.ItemsRef[i]);
            }
        }
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
            UISystem.Instance.GemSlot.Visible = !UISystem.Instance.GemSlot.Visible;

            if (UISystem.Instance.GemSlot.Visible && !Main.playerInventory)
                Main.playerInventory = true;
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