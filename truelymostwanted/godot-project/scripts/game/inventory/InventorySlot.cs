using System;
using Godot;
using LabyrinthExplorer3D.scripts.game.items;

namespace LabyrinthExplorer3D.scripts.game.inventory;

[GlobalClass]
public partial class InventorySlot : Resource
{
    [Export] public int ID;
    [Export] public ItemData Item;
    [Export] public int Amount;
    
    public bool HasNoItemAssigned() => Item is null;
    public bool HasAnyItemAssigned() => Item is not null;
    public bool HasItemAssigned(ItemData item) => Item == item;

    public bool IsEmpty() => Amount <= 0;
    public bool HasAmount() => Amount > 0;
    public bool MaximumAmountReached() => Amount == Item.MaxStackSize;
    
    public bool CanStoreItem(ItemData item) => HasItemAssigned(item) && !MaximumAmountReached();
    
    public bool Clear()
    {
        Item = null;
        Amount = 0;
        return true;
    }

    public bool Take(int amount, out int remainingAmount)
    {
        if (IsEmpty())
        {
            remainingAmount = amount;
            return false;
        }

        var maxToTake = Math.Min(amount, Amount);
        Amount -= maxToTake;
        remainingAmount = maxToTake;

        if (Amount == 0)
            Clear();

        return true;
    }

    public bool Store(ItemData item, int amount, out int remainingAmount)
    {
        if (item is null || amount <= 0)
        {
            remainingAmount = amount;
            return false;
        }

        if (!CanStoreItem(item))
        {
            remainingAmount = amount;
            return false;
        }

        Item = item;
        var amountToStore = Math.Min(amount, Item.MaxStackSize);
        Amount += amountToStore;
        remainingAmount = amount - amountToStore;
        return true;   
    }

    public bool StoreForced(ItemData item, int amount, out int remainingAmount)
    {
        Item = item;
        var amountToStore = Math.Min(amount, Item.MaxStackSize);
        Amount += amountToStore;
        remainingAmount = amount - amountToStore;
        return true; 
    }
}