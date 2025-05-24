using Godot;
using LabyrinthExplorer3D.scripts.game.level;

namespace LabyrinthExplorer3D.scripts.core.functions;

[GlobalClass]
public partial class LoadLevelFunction : Function
{
    [Export] public FileDialog Dialog;
    [Export] public Panel LevelPreview;
    
    private void OnDialogConfirmed()
    {
        var path = Dialog.CurrentPath;
        GD.Print(path);
        LevelGenerator3D.Instance.TryLoadLevel(path, out var levelTexture, out var levelImage);
        
        var lvl = LevelController3D.Instance.CurrentLevel;
        lvl.LevelTexture = levelTexture;
        lvl.LevelImage = levelImage;
        LevelPreview.AddThemeStyleboxOverride("panel", new StyleBoxTexture() { Texture = levelTexture } );
    }
    
    public override void Execute()
    {
        Dialog.Show();
    }

    public override void _Ready()
    {
        Dialog.Confirmed += OnDialogConfirmed;
        base._Ready();
    }
}