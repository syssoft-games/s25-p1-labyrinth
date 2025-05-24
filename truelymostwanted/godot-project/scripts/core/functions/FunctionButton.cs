using Godot;
using Godot.Collections;
using LabyrinthExplorer3D.scripts.core.functions;

[GlobalClass]
public partial class FunctionButton : Button
{
    [Export] public Array<Function> Functions;

    private void OnPressed()
    {
        if (Functions is null or { Count: 0 })
            return;
        
        foreach (var function in Functions)
            function.Execute();
    }
    
    public override void _Ready()
    {
        base._Ready();
        Pressed += OnPressed;
    }
}