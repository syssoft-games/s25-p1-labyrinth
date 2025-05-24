using Godot;
using Godot.Collections;
using LabyrinthExplorer3D.scripts.game.items;

namespace LabyrinthExplorer3D.scripts.game.inventory;

[GlobalClass]
public partial class Inventory : Resource
{
    public delegate void SlotUpdatedDelegate(Inventory inv, int slotIndex);
    public delegate void SlotSelectedDelegate(Inventory inv, int slotIndex);

    private event SlotUpdatedDelegate _SlotUpdated;
    private event SlotSelectedDelegate _SlotSelected;
    
    public event SlotUpdatedDelegate SlotUpdated
    {
        add => _SlotUpdated += value;
        remove => _SlotUpdated -= value;
    }
    public event SlotSelectedDelegate SlotSelected
    {
        add => _SlotSelected += value;
        remove => _SlotSelected -= value;
    }
    
    [Export] public Array<InventorySlot> ItemSlots;
    [Export] public int CurrentSlotIndex = 0;
    
    public void SelectSlot(int index)
    {
        CurrentSlotIndex = index;
        _SlotSelected?.Invoke(this, CurrentSlotIndex);
    }
    public void SelectFirstSlot()
    {
        CurrentSlotIndex = 0;
        _SlotSelected?.Invoke(this, CurrentSlotIndex);
    }
    public void SelectLastSlot()
    {
        CurrentSlotIndex = ItemSlots.Count - 1;
        _SlotSelected?.Invoke(this, CurrentSlotIndex);
    }
    public void SelectNextSlot()
    {
        CurrentSlotIndex += 1;
        if(CurrentSlotIndex >= ItemSlots.Count)
            CurrentSlotIndex = 0;
        
        _SlotSelected?.Invoke(this, CurrentSlotIndex);
    }
    public void SelectPreviousSlot()
    {
        CurrentSlotIndex -= 1;
        if(CurrentSlotIndex < 0)
            CurrentSlotIndex = ItemSlots.Count - 1;
        
        _SlotSelected?.Invoke(this, CurrentSlotIndex);
    }

    public bool StoreItem(ItemData itemData, int amount, out int remainingAmount)
    {
        if (itemData is null || amount < 0)
        {
            remainingAmount = amount;
            return false;
        }
        
        //(1) Store how many items are remaining
        remainingAmount = amount;
        
        //(2) For as long as we have items to store and NOT ALL slots are full, continue to store 
        while (remainingAmount > 0)
        {
            //(2.1) Find if any item slot is started, and fill it
            var canFindStarted = ItemSlots.TryFindFirst(s => s.CanStoreItem(itemData), out var startedSlot);
            if (canFindStarted)
            {
                startedSlot.Store(itemData, amount, out remainingAmount);
                _SlotUpdated?.Invoke(this, startedSlot.ID);
                continue;
            }
            
            //(2.2) Find the first empty slot
            var canFindEmpty = ItemSlots.TryFindFirst(s => s.HasNoItemAssigned() && s.IsEmpty(), out var emptySlot);
            if (canFindEmpty)
            {
                emptySlot.StoreForced(itemData, amount, out remainingAmount);
                _SlotUpdated?.Invoke(this, emptySlot.ID);
                continue;
            }

            //(2.3) If there isn't any further space, we can't store the item.
            return false;
        }
        
        //(3) All items have been stored, we're done!
        return true;
    }
    public bool DropItem(int itemSlotIndex, int amount, out int remainingAmount)
    {
        if (ItemSlots.IsOutOfRange(itemSlotIndex))
        {
            remainingAmount = -1;
            return false;
        }
        
        var canTake = ItemSlots[itemSlotIndex].Take(amount, out remainingAmount);
        if (!canTake)
            return false;
        
        _SlotUpdated?.Invoke(this, itemSlotIndex);
        return true;
    }

    public void Clear()
    {
        for (var i = 0; i < ItemSlots.Count; i++)
        {
            ItemSlots[i].Clear();
            _SlotUpdated?.Invoke(this, i);
        }
        SelectSlot(0);
    }
}