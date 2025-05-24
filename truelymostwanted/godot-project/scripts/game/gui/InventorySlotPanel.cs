using Godot;
using LabyrinthExplorer3D.scripts.game.inventory;

namespace LabyrinthExplorer3D.scripts.game.gui;

[GlobalClass]
public partial class InventorySlotPanel : Panel
{
    [Export] public int SlotID;
    [Export] public Inventory Inventory;

    [Export] public Panel HighlightedPanel;
    [Export] public Panel IconPanel;
    [Export] public Label AmountLabel;

    private void _OnInventorySlotUpdated(Inventory inv, int slotIndex)
    {
        if (slotIndex != SlotID)
            return;

        var slot = inv.ItemSlots[slotIndex];

        if (slot.Item is null)
        {
            IconPanel.AddThemeStyleboxOverride("panel", new StyleBoxFlat() { BgColor = Colors.Transparent });
            AmountLabel.Text = "";
            return;
        }
        else
        {
            IconPanel.AddThemeStyleboxOverride("panel", new StyleBoxTexture() { Texture = slot.Item.Icon });
            AmountLabel.Text = slot.Amount.ToString();   
        }
    }
    private void _OnInventorySlotSelected(Inventory inv, int slotIndex)
    {
        HighlightedPanel.SelfModulate = (slotIndex == SlotID) 
            ? Colors.White 
            : Colors.Transparent;
    }
    
    public override void _Ready()
    {
        base._Ready();
        Inventory.SlotUpdated += _OnInventorySlotUpdated;
        Inventory.SlotSelected += _OnInventorySlotSelected;
    }
}