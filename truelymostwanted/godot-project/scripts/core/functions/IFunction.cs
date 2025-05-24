using Godot;

namespace LabyrinthExplorer3D.scripts.core.functions;

public interface IFunction
{
    public static abstract void Execute(Node node);
    public void Execute();
}