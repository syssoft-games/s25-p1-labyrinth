using Godot;

namespace LabyrinthExplorer3D.scripts.game.abilties;

public partial class Player3dItemDropAbility : Player3dAbility
{
    public override void _OnProcess(double delta)
    {
        
    }

    public override void _OnUnhandledInput(InputEvent @event)
    {
        if (!IsAnyInputActionTriggered())
            return;
    }
}