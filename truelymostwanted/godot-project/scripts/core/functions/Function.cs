using Godot;

namespace LabyrinthExplorer3D.scripts.core.functions;

[GlobalClass]
public abstract partial class Function : Node
{
    public abstract void Execute();
}