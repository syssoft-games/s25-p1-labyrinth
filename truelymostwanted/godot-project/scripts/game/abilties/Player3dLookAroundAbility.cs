using Godot;

namespace LabyrinthExplorer3D.scripts.game.abilties;

[GlobalClass]
public partial class Player3dLookAroundAbility : Player3dAbility
{
	[Export] public float ActionRotationSpeed = 15f;
	[Export] public float MouseRotationSpeed = 0.1f;
	[Export] public float LimitXRotation = 66.00f;
	
	[Export] public bool InvertXAxis = false;
	[Export] public bool InvertYAxis = false;

	private void _OnLookAround(Vector2 relative)
	{
		//Left (-X), Right (+X)
		var relativeDegrees = relative * MouseRotationSpeed;
		var relativeRadiansX = Mathf.DegToRad(relativeDegrees.X);
		OwningPlayer.RotateY((!InvertXAxis) ? -relativeRadiansX : relativeRadiansX);
				
		//Up (-Y), Down (+Y)
		var camRotation = OwningPlayer.Camera.RotationDegrees;
		camRotation.X += (!InvertYAxis) ? -relativeDegrees.Y : relativeDegrees.Y;
		camRotation.X = Mathf.Clamp(camRotation.X, -LimitXRotation, LimitXRotation);
		OwningPlayer.Camera.RotationDegrees = camRotation;
	}

	private void _OnActionTriggered()
	{
		var inputAxis = Input.GetVector(
			InputActionNames[0],		//left
			InputActionNames[1],		//right
			InputActionNames[2],		//up
			InputActionNames[3]		//down
		);
		var relative = inputAxis * ActionRotationSpeed;
		_OnLookAround(relative);
	}
	private void _OnMouseMoved(InputEventMouseMotion mouseMotion)
	{
		var relative = mouseMotion.Relative;
		_OnLookAround(relative);
	}
	
	public override void _OnProcess(double delta)
	{
		
	}

	public override void _OnUnhandledInput(InputEvent @event)
	{
		//Up (-Y), Down (+Y)
		if (@event is InputEventMouseMotion mouseMotion)
			_OnMouseMoved(mouseMotion);
		
		if (IsAnyInputActionTriggered())
			_OnActionTriggered();
	}
}
