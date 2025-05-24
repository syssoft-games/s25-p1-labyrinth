using Godot;
using Godot.Collections;

namespace LabyrinthExplorer3D.scripts.game.level;

[GlobalClass]
public partial class LevelTileSet : Resource
{
    [Export] public Dictionary<string, Mesh> TileMeshes = new()
    {
        { "E", null },
        { "S", null },
        { "W", null },
        { "N", null },

        { "ES", null },
        { "EW", null },
        { "EN", null },
        { "SW", null },
        { "SN", null },
        { "WN", null },

        { "ESW", null },
        { "ESN", null },
        { "EWN", null },
        { "SWN", null },

        { "ESWN", null },
    };
}