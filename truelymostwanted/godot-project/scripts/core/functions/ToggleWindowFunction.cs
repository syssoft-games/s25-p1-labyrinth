using Godot;

namespace LabyrinthExplorer3D.scripts.core.functions;

[GlobalClass]
public partial class ToggleWindowFunction : Function
{
    [Export] public Window TargetWindow;
    [Export] public bool SetVisible;
    
    public override void Execute()
    {
        TargetWindow.Visible = SetVisible;
    }
}