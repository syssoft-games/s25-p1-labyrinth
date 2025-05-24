using Godot;
using LabyrinthExplorer3D.scripts.game.inventory;

namespace LabyrinthExplorer3D.scripts.game.components;

[GlobalClass]
public partial class InventoryComponent : Component
{
    [Export] public Inventory Inventory;
}