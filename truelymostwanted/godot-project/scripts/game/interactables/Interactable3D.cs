using Godot;
using Godot.Collections;

namespace LabyrinthExplorer3D.scripts.game.interactables;

[GlobalClass]
public partial class Interactable3D : Node3D
{
    [Export] public Array<InteractableFunction> Functions = new();

    public void Interact()
    {
        foreach (var function in Functions)
            function.OnInteract();
    }
}