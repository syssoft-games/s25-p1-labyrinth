using Godot;
using LabyrinthExplorer3D.scripts.game.inventory;
using LabyrinthExplorer3D.scripts.game.items;

namespace LabyrinthExplorer3D.scripts.game.abilties;

[GlobalClass]
public partial class Player3dInventoryAbility : Player3dAbility
{
    [Export] public Inventory Inventory;
    
    public void SelectSlot(int index)
    {
        Inventory.CurrentSlotIndex = index;
    }
    public void SelectFirstSlot()
    {
        Inventory.CurrentSlotIndex = 0;
    }
    public void SelectLastSlot()
    {
        Inventory.CurrentSlotIndex = Inventory.ItemSlots.Count - 1;
    }
    public void SelectNextSlot()
    {
        Inventory.CurrentSlotIndex += 1;
        Inventory.CurrentSlotIndex %= Inventory.ItemSlots.Count;
    }
    public void SelectPreviousSlot()
    {
        Inventory.CurrentSlotIndex -= 1;
        Inventory.CurrentSlotIndex %= Inventory.ItemSlots.Count;
    }

    public bool StoreItem(Item3D item3D)
    {
        if(item3D == null)
            return false;
        
        return Inventory.StoreItem(item3D.ItemData, 1, out var remainingAmount);
    }
    
    public override void _OnProcess(double delta) { }
    public override void _OnUnhandledInput(InputEvent @event) { }
}