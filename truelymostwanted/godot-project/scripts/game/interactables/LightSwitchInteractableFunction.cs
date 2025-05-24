using Godot;
using Godot.Collections;

namespace LabyrinthExplorer3D.scripts.game.interactables;

[GlobalClass]
public partial class LightSwitchInteractableFunction : InteractableFunction
{
    [Export] public Array<Light3D> Lights = new();
    
    public override void OnInteract()
    {
        foreach (var light in Lights)
        {
            if(light.IsVisible())
                light.Hide();
            else 
                light.Show();
        }
    }
}