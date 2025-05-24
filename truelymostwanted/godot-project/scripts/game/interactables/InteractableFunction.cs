using Godot;

namespace LabyrinthExplorer3D.scripts.game.interactables;

[GlobalClass]
public abstract partial class InteractableFunction : Node
{
    public abstract void OnInteract();
}