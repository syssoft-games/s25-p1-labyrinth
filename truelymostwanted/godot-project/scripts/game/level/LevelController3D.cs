using Godot;

namespace LabyrinthExplorer3D.scripts.game.level;

[GlobalClass]
public partial class LevelController3D : Node3D
{
    public static LevelController3D Instance { get; private set; }
    
    [Export] public Level3D CurrentLevel;

    public override void _Ready()
    {
        base._Ready();
        Instance = this;
    }
}