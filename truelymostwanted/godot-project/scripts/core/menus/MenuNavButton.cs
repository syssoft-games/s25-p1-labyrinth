using Godot;

namespace LabyrinthExplorer3D.scripts.core.menus;

[GlobalClass]
public partial class MenuNavButton : FunctionButton
{
    [Export] public Menu ParentMenu;
    [Export] public string TargetMenuName;
    [Export] public bool TargetVisibility = true;
    
    private void OnPressed()
    {
        ParentMenu.ParentController.NavigateToMenu(ParentMenu, this);
    }
    
    public override void _Ready()
    {
        base._Ready();
        Pressed += OnPressed;
    }
}