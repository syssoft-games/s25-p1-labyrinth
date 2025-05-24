using Godot;

namespace LabyrinthExplorer3D.scripts.game.gui;

[GlobalClass]
public partial class GameUI : Panel
{
    public static GameUI Instance { get; private set; }

    public override void _Ready()
    {
        base._Ready();
        Instance = this;
    }
}