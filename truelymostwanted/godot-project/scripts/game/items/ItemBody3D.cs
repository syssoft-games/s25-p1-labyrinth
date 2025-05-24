using Godot;

namespace LabyrinthExplorer3D.scripts.game.items;

public partial class ItemBody3D : RigidBody3D
{
    [Export] public Item3D Item;
}