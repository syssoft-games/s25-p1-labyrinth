using Godot;

namespace LabyrinthExplorer3D.scripts.game.items;

[GlobalClass]
public partial class Item3D : Node3D
{
    [Export] public ItemData ItemData;
    
    [Export] public ItemBody3D ItemBody;
    [Export] public ItemCollectArea3D ItemCollectArea;

    public override void _Ready()
    {
        base._Ready();
        
        if (ItemBody is not null)
            ItemBody.Item = this;
        
        if (ItemCollectArea is not null)
            ItemCollectArea.Item = this;
    }
}