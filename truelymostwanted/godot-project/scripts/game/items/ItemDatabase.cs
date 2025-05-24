using System.Collections;
using System.Collections.Generic;
using Godot;
using Godot.Collections;

namespace LabyrinthExplorer3D.scripts.game.items;

[GlobalClass]
public partial class ItemDatabase : Resource
{
    [Export] public Array<ItemData> Items;
}