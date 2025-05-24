using Godot;

namespace LabyrinthExplorer3D.scripts.game.abilties;

[GlobalClass]
public partial class Player3dInteractAbility : Player3dAbility
{
    public override void _OnProcess(double delta)
    {
        
    }

    public override void _OnUnhandledInput(InputEvent @event)
    {
        if (!IsAnyInputActionTriggered())
            return;
        
        GD.Print("Me wants to interact!");
    }
}