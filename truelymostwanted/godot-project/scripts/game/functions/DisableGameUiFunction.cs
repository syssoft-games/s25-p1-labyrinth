using Godot;
using LabyrinthExplorer3D.scripts.core.functions;
using LabyrinthExplorer3D.scripts.game.gui;

namespace LabyrinthExplorer3D.scripts.game.functions;

[GlobalClass]
public partial class DisableGameUiFunction : Function, IFunction
{
    public static void Execute(Node node)
    {
        GameUI.Instance.Hide();
    }

    public override void Execute()
    {
        Execute(node: this); 
    }
}