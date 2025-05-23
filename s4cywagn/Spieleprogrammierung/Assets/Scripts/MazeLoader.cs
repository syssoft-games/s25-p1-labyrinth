using System.Collections.Generic;
using System.IO;
using UnityEngine;

// MazeLoader.cs – Version without external JSON package (uses UnityEngine.JsonUtility)

/*
 1. Place a maze.json inside Assets/StreamingAssets – sample format:
 {
   "width": 2,
   "height": 2,
   "cells": [
     {"x":0,"y":0,"wallNorth":true,"wallSouth":false,"wallEast":false,"wallWest":true,"material":"absorb","isLight":true,"isGoal":false},
     {"x":1,"y":0,"wallNorth":true,"wallSouth":false,"wallEast":true,"wallWest":false,"material":"reflect","isLight":false,"isGoal":false},
     {"x":0,"y":1,"wallNorth":false,"wallSouth":true,"wallEast":false,"wallWest":true,"material":"transparent","isLight":false,"isGoal":false},
     {"x":1,"y":1,"wallNorth":false,"wallSouth":true,"wallEast":true,"wallWest":false,"material":"absorb","isLight":false,"isGoal":true}
   ]
 }
 2. Attach this script to an empty GameObject called "Maze" and assign prefabs/materials in the Inspector.
 3. Requires NO external packages – Unity's built‑in JsonUtility handles the parsing.
*/

[System.Serializable]
public class MazePlan
{
    public int width;
    public int height;
    public Cell[] cells;
}

[System.Serializable]
public class Cell
{
    public int x;
    public int y;
    public bool wallNorth;
    public bool wallSouth;
    public bool wallEast;
    public bool wallWest;
    public string material; // "absorb", "reflect", "transparent"
    public bool isLight;
    public bool isGoal;
}

public class MazeLoader : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject wallPrefab;
    public GameObject floorPrefab;
    public GameObject goalPrefab;
    public Light lightPrefab;

    [Header("Materials")]
    public Material absorbMaterial;
    public Material reflectMaterial;
    public Material transparentMaterial;

    [Header("Paths")]
    public string jsonFileName = "maze.json";

    private Dictionary<string, Material> materialLookup;

    private void Awake()
    {
        materialLookup = new Dictionary<string, Material>
        {
            {"absorb", absorbMaterial},
            {"reflect", reflectMaterial},
            {"transparent", transparentMaterial}
        };
        LoadMaze();
    }

    private void LoadMaze()
    {
        var path = Path.Combine(Application.streamingAssetsPath, jsonFileName);
        if (!File.Exists(path))
        {
            Debug.LogError($"Maze JSON not found: {path}");
            return;
        }
        string json = File.ReadAllText(path);

        // Parse with Unity's built‑in JSON system
        MazePlan plan = JsonUtility.FromJson<MazePlan>(json);
        if (plan == null || plan.cells == null)
        {
            Debug.LogError("Failed to parse maze.json – check JSON format matches MazePlan schema.");
            return;
        }

        foreach (Cell cell in plan.cells)
        {
            Vector3 basePos = new Vector3(cell.x, 0, cell.y);

            // Floor tile
            if (floorPrefab) Instantiate(floorPrefab, basePos, Quaternion.identity, transform);

            // Walls (each wall 1×2×0.1, pivot centred)
            if (cell.wallNorth) SpawnWall(basePos + new Vector3(0, 0, 0.5f), Quaternion.identity, cell.material);
            if (cell.wallSouth) SpawnWall(basePos + new Vector3(0, 0, -0.5f), Quaternion.identity, cell.material);
            if (cell.wallEast)  SpawnWall(basePos + new Vector3(0.5f, 0, 0), Quaternion.Euler(0, 90, 0), cell.material);
            if (cell.wallWest)  SpawnWall(basePos + new Vector3(-0.5f, 0, 0), Quaternion.Euler(0, 90, 0), cell.material);

            // Dynamic light
            if (cell.isLight && lightPrefab)
            {
                Light l = Instantiate(lightPrefab, basePos + Vector3.up * 1.5f, Quaternion.identity, transform);
                l.intensity = 50f;
                l.range = 10f;
            }

            // Goal object
            if (cell.isGoal && goalPrefab)
            {
                Instantiate(goalPrefab, basePos + Vector3.up * 0.5f, Quaternion.identity, transform);
            }
        }
    }

    private void SpawnWall(Vector3 pos, Quaternion rot, string materialKey)
    {
        if (!wallPrefab) return;
        GameObject go = Instantiate(wallPrefab, pos, rot, transform);
        if (materialLookup.TryGetValue(materialKey, out Material mat))
        {
            Renderer r = go.GetComponent<Renderer>();
            if (r) r.material = mat;
        }
    }
}
