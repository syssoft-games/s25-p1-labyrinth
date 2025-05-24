using System;
using System.IO;
using System.Linq;
using Godot;
using Godot.Collections;
using LabyrinthExplorer3D.scripts.game.items;
using LabyrinthExplorer3D.scripts.game.player;

namespace LabyrinthExplorer3D.scripts.game.level;

[GlobalClass]
public partial class LevelGenerator3D : Node3D
{
    public static LevelGenerator3D Instance { get; private set; }
    
    [Flags]
    public enum Neighbours
    {
        None = 0,
        E = 1,
        S = 2,
        W = 4,
        N = 8,
        
        ES = E + S,
        EW = E + W,
        EN = E + N,
        SW = S + W, 
        SN = S + N,
        WN = W + N,
        
        ESW = E + S + W,
        ESN = E + S + N,
        EWN = E + W + N,
        SWN = S + W + N,
        
        ESWN = E + S + W + N,
    }

    [Export] public LevelController3D LevelController;
    [Export] public LevelTileSetDictionary LevelTileSetDictionary;
    [Export] public PackedScene ItemPackedScene;

    [Export] public Color EmptyTileColor = Colors.Black;
    [Export] public Color RegularTileColor = Colors.White;
    [Export] public Color GlasTileColor = Colors.Gray;
    [Export] public Color SpawnColor = Colors.Green;
    [Export] public Color GoalColor = Colors.Red;
    [Export] public Color ItemColor = Colors.Blue;
    
    public bool IsEmptyField(Image img, Vector2I imgSize, int x, int y)
    {
        return img.GetPixel(x, y) == EmptyTileColor;
    }
    public bool IsSpawnField(Image img, Vector2I imgSize, int x, int y)
    {
        return img.GetPixel(x, y) == SpawnColor;
    }
    public bool IsGoalField(Image img, Vector2I imgSize, int x, int y)
    {
        return img.GetPixel(x, y) == GoalColor;
    }
    public bool IsItemField(Image img, Vector2I imgSize, int x, int y)
    {
        return img.GetPixel(x, y) == ItemColor;
    }
    
    public Neighbours GetNeighbours(Image img, Vector2I imgSize, int x, int y, bool skipCheck = true)
    {
        var neighbours = Neighbours.None;
        
        var left = new Vector2I(x - 1, y);
        if(left.X >= 0 && img.GetPixel(left.X, left.Y) != EmptyTileColor)
            neighbours |= Neighbours.W;
        
        var right = new Vector2I(x + 1, y);
        if(right.X < imgSize.X && img.GetPixel(right.X, right.Y) != EmptyTileColor)
            neighbours |= Neighbours.E;
        
        var up = new Vector2I(x, y - 1);
        if(up.Y >= 0 && img.GetPixel(up.X, up.Y) != EmptyTileColor)
            neighbours |= Neighbours.N;
        
        var down = new Vector2I(x, y + 1);
        if(down.Y < imgSize.Y && img.GetPixel(down.X, down.Y) != EmptyTileColor)
            neighbours |= Neighbours.S;
        
        return neighbours;
    }

    public string GetTileType()
    {
        var chanceForTransparency = 0.15f;
        var randomValue = GD.Randf();
        return randomValue <= chanceForTransparency 
            ? "glas" 
            : "default";
    }
    public string GetTileMeshName(Neighbours neighbours)
    {
        return neighbours.ToString().Replace(", ", ""); //"E, S, W, N" --> "ESWN"
    }
    public bool TryGetMesh(string type, string meshName, out Mesh mesh)
    {
        var canGetType = LevelTileSetDictionary.LevelTileSets.TryGetValue(type, out var tileSet);
        if (!canGetType)
        {
            mesh = null;
            return false;
        }
        
        var canGetTile = tileSet.TileMeshes.TryGetValue(meshName, out mesh);
        return canGetTile;
    }
    private bool _TryInstantiateMesh(Level3D level, Vector3 globalPos, Mesh mesh)
    {
        try
        {
            var meshInstance = new MeshInstance3D();
            meshInstance.Mesh = mesh;
            meshInstance.CreateTrimeshCollision();
            level.TileParent.AddChild(meshInstance);
            meshInstance.GlobalPosition = globalPos;
            return true;
        }
        catch (Exception exception)
        {
            GD.PrintErr(exception);
            return false;
        }
    }


    public bool TryDestroyLevel3D()
    {
        //(0) Get Level
        var level = LevelController.CurrentLevel;
        
        //(1) Clear level
        level.Clear();

        return true;
    }

    private bool _TryGenerateLevel3D(Texture2D texture, Image img)
    {
        try
        {
            //(0) Get Level
            var level = LevelController.CurrentLevel;
            
            //(1) Destroy previous level
            TryDestroyLevel3D();
            
            //(2) Iterate over the pixel and create the level
            var imgSize = img.GetSize();
            for (int y = 0; y < imgSize.Y; y++)
            {
                for (int x = 0; x < imgSize.X; x++)
                {
                    if (IsEmptyField(img, imgSize, x, y)) 
                        continue;
                    
                    var neighbours = GetNeighbours(img, imgSize, x, y);
                    var typeName = GetTileType();
                    var meshName = GetTileMeshName(neighbours);
                    if (!TryGetMesh(typeName, meshName, out var mesh)) 
                        continue;
                    var position = new Vector3(x * 4, 0, y * 4);
                    _TryInstantiateMesh(level, position, mesh);

                    if (IsSpawnField(img, imgSize, x, y))
                    {
                        PlayerController3D.Instance.CurrentPlayer.GlobalPosition = position + new Vector3(0, 3, 0);
                    }

                    if (IsItemField(img, imgSize, x, y))
                    {
                        var itemNode = ItemPackedScene.Instantiate<Item3D>();
                        level.ItemsParent3D.AddChild(itemNode);
                        itemNode.GlobalPosition = position;
                    }
                    
                }
            }
            
            level.LevelTexture = texture;
            level.LevelImage = img;
            return true;
        }
        catch (Exception exception)
        {
            GD.PrintErr(exception);
            return false;
        }
    }
    public bool TryGenerateLevel3D(Image img)
    {
        if (img == null)
            return false;

        var texture = ImageTexture.CreateFromImage(img);
        return _TryGenerateLevel3D(texture, img);
    }
    public bool TryGenerateLevel3D(Texture2D texture2D)
    {
        if (texture2D == null)
            return false;
        
        var image = texture2D.GetImage();
        return _TryGenerateLevel3D(texture2D, image);
    }
    public bool TryGenerateLevel3D(string levelFilePath)
    {
        var canLoad = TryLoadLevel(levelFilePath, out var texture2D, out var image);
        if (!canLoad)
            return false;
        return _TryGenerateLevel3D(texture2D, image);
    }

    public bool TryGenerateLevel3D()
    {
        var lvl = LevelController.CurrentLevel;
        var texture = lvl.LevelTexture;
        var image = lvl.LevelImage;
        return _TryGenerateLevel3D(texture, image);
    }

    public bool TryLoadLevel(string levelFilePath, out Texture2D levelTexture, out Image levelImage)
    {
        try
        {
            if (levelFilePath.StartsWith("res://") || levelFilePath.StartsWith("uid://"))
            {
                levelTexture = ResourceLoader.Load<Texture2D>(levelFilePath);
                levelImage = levelTexture.GetImage();
            }
            else if (levelFilePath.StartsWith("user://"))
            {
                var globalizedPath = ProjectSettings.GlobalizePath(levelFilePath);
                levelImage = Image.LoadFromFile(globalizedPath);
                levelTexture = ImageTexture.CreateFromImage(levelImage);
            }
            else
            {
                levelImage = Image.LoadFromFile(levelFilePath);
                levelTexture = ImageTexture.CreateFromImage(levelImage);
            }

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            levelImage = null;
            levelTexture = null;
            return false;
        }
    }

    public override void _Ready()
    {
        base._Ready();
        Instance = this;
        //var canLoad = TryGenerateLevel3D("res://resources/textures/LVL_0.png");
        //GD.Print($"Can Load: {canLoad}");
    }
}