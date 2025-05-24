using Godot;
using LabyrinthExplorer3D.scripts.core.menus;
using LabyrinthExplorer3D.scripts.game.gui;
using LabyrinthExplorer3D.scripts.game.time;

namespace LabyrinthExplorer3D.scripts.core.functions;

[GlobalClass]
public partial class StartGameFunction : Function, IFunction
{
    public static void Execute(Node node)
    {
        ContinueGameFunction.Execute(node);
        TimeController.Instance?.SetTime(0, 0);
        MenuController.Instance?.ToggleCurrentMenu(false);
        GameUI.Instance?.Show();
    }
    
    public override void Execute()
    {
        Execute(node: this);   
    }
}