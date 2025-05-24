using Godot;

namespace LabyrinthExplorer3D.scripts.game.items;

[GlobalClass]
public partial class ItemData : Resource
{
    [Export] public int Id;
    [Export] public string Name;
    [Export] public string Description;
    [Export] public int Value;
    [Export] public int MaxStackSize = 64;
    [Export] public Texture2D Icon;
    [Export] public PackedScene ItemPrefab;
    [Export] public PackedScene EntityPrefab;
}