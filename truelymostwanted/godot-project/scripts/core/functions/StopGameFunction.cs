using Godot;
using LabyrinthExplorer3D.scripts.core.menus;
using LabyrinthExplorer3D.scripts.game.gui;

namespace LabyrinthExplorer3D.scripts.core.functions;

[GlobalClass]
public partial class StopGameFunction : Function, IFunction
{
    public static void Execute(Node node)
    {
        PauseGameFunction.Execute(node);
        MenuController.Instance.ToggleCurrentMenu(true);
        GameUI.Instance.Hide();
    }
    
    public override void Execute()
    {
        Execute(node: this);   
    }
}