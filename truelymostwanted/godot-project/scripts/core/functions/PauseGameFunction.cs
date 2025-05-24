using Godot;

namespace LabyrinthExplorer3D.scripts.core.functions;

[GlobalClass]
public partial class PauseGameFunction : Function, IFunction
{
    public static void Execute(Node node)
    {
        node.GetTree().SetPause(true);
        Input.MouseMode = Input.MouseModeEnum.Visible;
    }

    public override void Execute()
    {
        Execute(node: this);  
    }
}