using Godot;

namespace LabyrinthExplorer3D.scripts.game.abilties;

[GlobalClass]
public partial class Player3dMovementAbility : Player3dAbility
{
    [Export] public Vector2 InputVector2;
    [Export] public float Speed = 5.0f;

    
    public override void _OnProcess(double delta)
    {
        Vector3 velocity = OwningPlayer.Velocity;

        // Add the gravity.
        if (!OwningPlayer.IsOnFloor())
        {
            velocity += OwningPlayer.GetGravity() * (float)delta;
        }

        // // Handle Jump.
        // if (Input.IsActionJustPressed("ui_accept") && OwningPlayer.IsOnFloor())
        // {
        //     velocity.Y = OwningPlayer.JumpVelocity;
        // }

        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.
        Vector2 inputDir = InputVector2;
        Vector3 direction = (OwningPlayer.Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
        if (direction != Vector3.Zero)
        {
            velocity.X = direction.X * Speed;
            velocity.Z = direction.Z * Speed;
        }
        else
        {
            velocity.X = Mathf.MoveToward(OwningPlayer.Velocity.X, 0, Speed);
            velocity.Z = Mathf.MoveToward(OwningPlayer.Velocity.Z, 0, Speed);
        }

        OwningPlayer.Velocity = velocity;
        OwningPlayer.MoveAndSlide();
    }
    public override void _OnUnhandledInput(InputEvent @event)
    {
        if (!IsAnyInputActionTriggered())
            return;
        
        InputVector2 = Input.GetVector(
            InputActionNames[0], 
            InputActionNames[1], 
            InputActionNames[2], 
            InputActionNames[3]
        );
    }

}