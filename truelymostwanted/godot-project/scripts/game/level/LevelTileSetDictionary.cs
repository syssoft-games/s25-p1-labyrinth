using Godot;
using Godot.Collections;

namespace LabyrinthExplorer3D.scripts.game.level;

[GlobalClass]
public partial class LevelTileSetDictionary : Resource
{
    [Export] public Dictionary<string, LevelTileSet> LevelTileSets = new(); // <string, LevelTileSet>
}