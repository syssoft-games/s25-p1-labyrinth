using Godot;

namespace LabyrinthExplorer3D.scripts.game.abilties;

[GlobalClass]
public abstract partial class Player3dAbility : InputActionAbility
{
    public Player3D OwningPlayer
    {
        get
        {
            if(AbilityOwner is Player3D player)
                return player;
            return null;
        }
    }
}