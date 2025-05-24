using Godot;
using LabyrinthExplorer3D.scripts.core.extensions;

namespace LabyrinthExplorer3D.scripts.game.level;

[GlobalClass]
public partial class Level3D : Node3D
{
    [Export] public Texture2D LevelTexture;
    [Export] public Image LevelImage;

    [Export] public Node3D TileParent;
    [Export] public Node3D SpawnParent;
    [Export] public Node3D GoalParent;
    [Export] public Node3D ItemsParent3D;

    public void Clear()
    {
        TileParent.QueueFreeChildren();
        SpawnParent.QueueFreeChildren();
        GoalParent.QueueFreeChildren();
        ItemsParent3D.QueueFreeChildren();
    }
    
    public override void _Ready()
    {
        
    }
}