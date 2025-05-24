using Godot;
using LabyrinthExplorer3D.scripts.game.abilties;
using LabyrinthExplorer3D.scripts.game.components;

namespace LabyrinthExplorer3D.scripts.game.items;

[GlobalClass]
public partial class ItemCollectArea3D : Area3D
{
    [Export] public Item3D Item;

    public override void _Ready()
    {
        base._Ready();
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node3D body)
    {
        if(body is not Player3D player)
            return;
        GD.Print("Player Collision");
        
        var canGetInventory = player.TryGetComponent<InventoryComponent>(out var inventoryComponent);
        if (!canGetInventory)
            return;
        
        GD.Print("Player collects item");
        inventoryComponent.Inventory.StoreItem(Item.ItemData, 1, out var remainingAmount);
        if (remainingAmount == 0)
        {
            Item.Hide();
            Item.CallDeferred(Node.MethodName.QueueFree);
        }
    }
}