using Godot;

namespace LabyrinthExplorer3D.scripts.core.functions;

[GlobalClass]
public partial class ContinueGameFunction : Function, IFunction
{
    public static void Execute(Node node)
    {
        node.GetTree().SetPause(false);
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void Execute()
    {
        Execute(node: this);  
    }
}