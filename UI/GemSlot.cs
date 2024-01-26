using System.Collections.Generic;
using System.Linq;
using CTGMod.Common.ModPlayers;
using CTGMod.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.GameContent.UI.States;
using Terraria.UI;

namespace CTGMod.UI;

public class GemSlot : UIState
{
    private Item[] _items;
    private UIItemSlot[] _slots;
    
    public bool Visible;

    private bool _requestUpdate;
    private int _saveTimer;

    public override void OnInitialize()
    {
        _items = new Item[10];
        _slots = new UIItemSlot[10];
        for (int i = 0; i < 10; i++)
        {
            _items[i] = new Item();
            _slots[i] = new UIItemSlot(_items, i, ItemSlot.Context.BankItem);
            _slots[i].Left.Set((200f + i * 56f) * Main.inventoryScale, 0f);
            _slots[i].Top.Set(500f * Main.inventoryScale, 0f);
            Append(_slots[i]);
        }

        ItemSlot.OnItemTransferred += info =>
        {
            if (GemID.Gems.Contains(info.ItemType) && (info.FromContenxt == ItemSlot.Context.BankItem || info.ToContext == ItemSlot.Context.BankItem))
            {
                _requestUpdate = true;
            }
        };

        _requestUpdate = true; // immediate update when entering world
    }

    public override void Update(GameTime gameTime)
    {
        if (Main.LocalPlayer.chest != -1 || !Main.playerInventory)
            Visible = false;

        ref IList<int> itemTypesForLoad = ref Main.LocalPlayer.GetModPlayer<CTGPlayer>().ItemTypesForLoad; 
        if (itemTypesForLoad != null && itemTypesForLoad.Count != 0)
        {
            for (int i = 0; i < itemTypesForLoad.Count; i++)
            {
                _items[i] = new Item(itemTypesForLoad[i]);
            }
            itemTypesForLoad.Clear();
        }

        if (++_saveTimer >= 1200 || _requestUpdate)
        {
            _saveTimer = 0;
            _requestUpdate = false;
            UpdateItemTypesForSave();
        }
        // update gem info in CTGPlayer & UpdateOwnedGemsPatch

        base.Update(gameTime);
    }
    
    public List<int> GetItemTypes() => _items.Select(item => item.type).ToList();

    public void UpdateItemTypesForSave() => Main.LocalPlayer.GetModPlayer<CTGPlayer>().ItemTypesForSave = GetItemTypes();

    public ref Item[] ItemsRef => ref _items;

    public bool TryInsert(Item item)
    {
        for (int i = 0; i < 10; i++)
        {
            if (!_items[i].IsAir)
                continue;

            _items[i].SetDefaults(item.type);
            return true;
        }

        return false;
    }
}