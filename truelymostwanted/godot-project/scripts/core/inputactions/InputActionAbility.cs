using Godot;
using Godot.Collections;

[GlobalClass]
public abstract partial class InputActionAbility : Node
{
	[Export] public Node AbilityOwner;
	
	[Export] public Array<string> InputActionNames;
	[Export] public bool TriggersOnJustPressed = true;
	[Export] public bool TriggersOnPressed = false;
	[Export] public bool TriggersOnJustReleased = true;

	public bool IsAnyInputActionTriggered()
	{
		var anyJustPressed = TriggersOnJustPressed && 
								  InputActionNames.Any(name => Input.IsActionJustPressed(name));
		if (anyJustPressed)
			return true;
		
		var anyPressed = TriggersOnPressed && InputActionNames.Any(name => Input.IsActionPressed(name));
		if (anyPressed)
			return true;
		
		var anyJustReleased = TriggersOnJustReleased && 
		                           InputActionNames.Any(name => Input.IsActionJustReleased(name));
		if (anyJustReleased)
			return true;

		return false;
	}
	
	public bool MouseWasMoved(InputEvent @event) => @event is InputEventMouseMotion;

	public abstract void _OnProcess(double delta);
	public abstract void _OnUnhandledInput(InputEvent @event);
}
