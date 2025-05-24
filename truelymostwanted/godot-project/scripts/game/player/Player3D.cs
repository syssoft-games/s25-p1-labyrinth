using Godot;
using Godot.Collections;
using LabyrinthExplorer3D.scripts.game.abilties;
using LabyrinthExplorer3D.scripts.game.components;

[GlobalClass]
public partial class Player3D : CharacterBody3D
{
	[Export] public int PlayerID;
	[Export] public string PlayerName;
	
	[Export] public Node3D RightHandParent;
	[Export] public Node3D LeftHandParent;
	
	[Export] public Array<Player3dAbility> PlayerAbilities;
	[Export] public Array<Component> Components;
	[Export] public Camera3D Camera;

	public bool TryGetAbility<T>(out T ability) where T : Player3dAbility
		=> PlayerAbilities.TryGetTypeOf(out ability);
	public bool TryGetComponent<T>(out T component) where T : Component
		=> Components.TryGetTypeOf(out component);
	
	public override void _UnhandledInput(InputEvent @event)
	{
		foreach(var ability in PlayerAbilities)
			ability._OnUnhandledInput(@event);
	}
	public override void _Process(double delta)
	{
		foreach(var ability in PlayerAbilities)
			ability._OnProcess(delta);
	}
}
