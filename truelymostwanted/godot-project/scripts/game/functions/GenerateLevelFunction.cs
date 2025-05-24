using Godot;
using LabyrinthExplorer3D.scripts.core.functions;
using LabyrinthExplorer3D.scripts.game.level;

namespace LabyrinthExplorer3D.scripts.game.functions;

[GlobalClass]
public partial class GenerateLevelFunction : Function, IFunction
{
    public static void Execute(Node node)
    {
        LevelGenerator3D.Instance.TryGenerateLevel3D();
    }

    public override void Execute()
    {
        Execute(this);
    }
}