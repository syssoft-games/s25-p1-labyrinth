using Godot;

namespace LabyrinthExplorer3D.scripts.game.player;

[GlobalClass]
public partial class PlayerController3D : Node3D
{
    public static PlayerController3D Instance { get; private set; }

    [Export] public Player3D CurrentPlayer;
    
    public override void _Ready()
    {
        base._Ready();
        Instance = this;
    }
}