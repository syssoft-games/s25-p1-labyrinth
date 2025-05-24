using Godot;
using LabyrinthExplorer3D.scripts.game.interactables;
using LabyrinthExplorer3D.scripts.game.items;

namespace LabyrinthExplorer3D.scripts.game.abilties;

[GlobalClass]
public partial class Player3dRayCastAbility : Player3dAbility
{
    [Export] public RayCast3D RayCast;
    [Export] public bool IsColliding = false;
    
    [ExportGroup("Collision Target")]
    [Export] public GodotObject Target = null;
    [Export] public bool TargetIsPlayer = false;
    [Export] public bool TargetIsItem = false;
    [Export] public bool TargetIsInteractable = false;

    private void _ProcessRayCast()
    {
        IsColliding = RayCast.IsColliding();
        
        if (!IsColliding)
        {
            Target = null;
            TargetIsPlayer = false;
            TargetIsItem = false;
            TargetIsInteractable = false;
        }
        else
        {
            Target = RayCast.GetCollider();
            TargetIsPlayer = Target is Player3D;
            TargetIsItem = Target is ItemCollectArea3D or ItemBody3D;
            TargetIsInteractable = Target is Interactable3D;
        }
    }
    
    public override void _OnProcess(double delta)
    {
        
    }
    
    public override void _OnUnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseMotion)
            RayCast.ForceRaycastUpdate();
		
        if (IsAnyInputActionTriggered())
            RayCast.ForceRaycastUpdate();
    }
}