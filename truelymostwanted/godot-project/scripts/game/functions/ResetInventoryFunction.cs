using Godot;
using LabyrinthExplorer3D.scripts.core.functions;
using LabyrinthExplorer3D.scripts.game.components;
using LabyrinthExplorer3D.scripts.game.player;

namespace LabyrinthExplorer3D.scripts.game.functions;

[GlobalClass]
public partial class ResetInventoryFunction : Function, IFunction
{
    public static void Execute(Node node)
    {
        var canGet = PlayerController3D.Instance.CurrentPlayer.TryGetComponent(out InventoryComponent inventoryComponent);
        if (!canGet)
            return;

        inventoryComponent.Inventory.Clear();
    }

    public override void Execute()
    {
        Execute(node: this);  
    }
}