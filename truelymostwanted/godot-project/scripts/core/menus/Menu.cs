using Godot;
using Godot.Collections;
using LabyrinthExplorer3D.scripts.core.extensions;

namespace LabyrinthExplorer3D.scripts.core.menus;

[GlobalClass]
public partial class Menu : Panel
{
    [Export] public MenuController ParentController;
    [Export] public Node ButtonsParent;
    [Export] public Array<MenuNavButton> MenuButtons;

    public override void _Ready()
    {
        base._Ready();
        var buttonCount = ButtonsParent.GetChildrenOfType<MenuNavButton>(out var childrenOfType);
        MenuButtons = new(childrenOfType);
        foreach (var button in childrenOfType)
        {
            MenuButtons.Add(button);
            button.ParentMenu = this;
        }
    }
}