using Godot;
using Godot.Collections;
using LabyrinthExplorer3D.scripts.core.extensions;

namespace LabyrinthExplorer3D.scripts.core.menus;

[GlobalClass]
public partial class MenuController : Control
{
    private static MenuController _Instance = null;
    public static MenuController Instance => _Instance;
    
    [Export] public Array<Menu> AllMenus;
    [Export] public string StandardMenu = "Main";
    [Export] private Menu CurrentMenu;

    public void DisableAllMenus()
    {
        GD.Print("Hiding all other menus");
        foreach (var menu in AllMenus)
            menu.Hide();
    }

    public bool ToggleCurrentMenu(bool isVisible)
    {
        if (CurrentMenu is null)
            return false;
        
        if (isVisible)
            CurrentMenu.Show();
        else 
            CurrentMenu.Hide();
        
        return true;
    }
    
    public bool IsExitGameMenuName(string menuName) => menuName is "Close" or "Quit" or "QuitGame" or "Exit" or "ExitGame";
    
    public bool NavigateToMenu(string senderMenuName, string menuName, bool targetVisibility = true)
    {
        GD.Print("NavigateToMenu: " + senderMenuName + " -> " + menuName);
        
        //(1) if the menu name indicate quitting the game, we shut it down entirely
        if (IsExitGameMenuName(menuName))
        {
            GetTree().Quit();
            return true;
        }
        
        //(2) If its a menu name, try to find the menu 
        if (!AllMenus.TryFindFirst(m => m.Name.Equals(menuName), out CurrentMenu)) 
            return false;
        
        //(2.1) Disable all other menus and open the target one if requested
        DisableAllMenus();
        ToggleCurrentMenu(targetVisibility);
        return true;
    }
    public bool NavigateToMenu(Menu sender, string menuName, bool targetVisibility = true)
    {
        return NavigateToMenu(sender.Name, menuName, targetVisibility);   
    }
    public bool NavigateToMenu(Menu sender, MenuNavButton menuNavButton)
    {
        return NavigateToMenu(sender.Name, menuNavButton.TargetMenuName, menuNavButton.TargetVisibility);
    }

    public override void _Ready()
    {
        base._Ready();
        _Instance = this;
        
        var matches = this.GetChildrenOfType<Menu>(out var childList);
        AllMenus = new(childList);
        foreach (var menu in childList)
            menu.ParentController = this;

        NavigateToMenu(StandardMenu, StandardMenu, true);
    }
}