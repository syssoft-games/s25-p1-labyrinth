using Godot;
using LabyrinthExplorer3D.scripts.core.functions;
using LabyrinthExplorer3D.scripts.core.menus;
using LabyrinthExplorer3D.scripts.game.functions;

namespace LabyrinthExplorer3D.scripts.game.abilties;

[GlobalClass]
public partial class Player3dPauseAbility : Player3dAbility
{
    public override void _OnProcess(double delta)
    {
        
    }

    public override void _OnUnhandledInput(InputEvent @event)
    {
        if (!IsAnyInputActionTriggered())
            return;

        var tree = GetTree();
        if (tree.IsPaused())
        {
            ContinueGameFunction.Execute(node: this);
            EnableGameUiFunction.Execute(node: this);
            MenuController.Instance.ToggleCurrentMenu(false);
        }
        else
        {
            PauseGameFunction.Execute(node: this);
            DisableGameUiFunction.Execute(node: this);
            MenuController.Instance.ToggleCurrentMenu(true);
        }
    }
}