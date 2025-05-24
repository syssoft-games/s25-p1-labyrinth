using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;

public class MazeLoader : MonoBehaviour
{
    [Header("Pfad zur ASCII-Datei (relativ zum Projekt)")]
    public string asciiFilePath = "Assets/Mazes/maze.txt";

    [Header("Prefabs")]
    public GameObject wallPrefab;
    public GameObject glassPrefab;
    public GameObject metalPrefab;
    public GameObject floorPrefab;
    public GameObject targetPrefab;
    public GameObject playerObject;

    //[Header("Tile Settings")]
    //private float tileSize = 1f;

    void Start()
    {
        LoadMaze();
    }

    void LoadMaze()
    {
        if (!File.Exists(asciiFilePath))
        {
            Debug.LogError($"Datei nicht gefunden: {asciiFilePath}");
            return;
        }

        string[] lines = File.ReadAllLines(asciiFilePath);
        ValidateMaze(lines);

        int width = lines[0].Length;
        int height = lines.Length;

        // Erzeuge Bodenplatte
        if (floorPrefab)
        {
            GameObject floor = Instantiate(floorPrefab, new Vector3((width-1) * 0.5f, 0, height * 0.5f -0.5f), Quaternion.identity);
            floor.transform.localScale = new Vector3(width * 0.1f, 1f, height * 0.1f);
            //Einstellen der Tile-Größe für das Muster
            Renderer component = floor.GetComponent<Renderer>();
            if (component != null)
            {
                component.material.mainTextureScale = new Vector2(width, height);
            }
        }

        bool playerSpawned = false;
        bool targetSpawned = false;

        for (int y = 0; y < height; y++)
        {
            string line = lines[y];
            for (int x = 0; x < width; x++)
            {
                char symbol = line[x];
                Vector3 spawnPos = new Vector3(x, 1f, height - y - 1);

                switch (symbol)
                {
                    case '#':
                        if (wallPrefab) Instantiate(wallPrefab, spawnPos, Quaternion.identity);
                        break;
                    case 'G':
                        if (glassPrefab) Instantiate(glassPrefab, spawnPos, Quaternion.identity);
                        break;
                    case 'M':
                        if (metalPrefab) Instantiate(metalPrefab, spawnPos, Quaternion.identity);
                        break;
                    case 'T':
                        if (targetPrefab && !targetSpawned)
                        {
                            Instantiate(targetPrefab, spawnPos, Quaternion.identity);
                            targetSpawned = true;
                        }
                        break;
                    case 'P':
                        if (playerObject && !playerSpawned)
                        {
                            //Instantiate(playerPrefab, spawnPos + Vector3.up * 1f, Quaternion.identity);
                            playerObject.transform.position = spawnPos;
                            playerSpawned = true;
                        }
                        break;
                    case '.':
                        // Freier Boden, nichts tun
                        break;
                    default:
                        Debug.LogError($"Unbekanntes Symbol '{symbol}' bei ({x}, {y})");
                        break;
                }
            }
        }
    }

    void ValidateMaze(string[] lines)
    {
        if (lines.Length == 0) throw new Exception("Die ASCII-Datei ist leer.");

        int width = lines[0].Length;
        int playerCount = 0;
        int targetCount = 0;

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];

            if (line.Length != width)
                throw new Exception($"Zeile {i + 1} hat eine inkorrekte Länge. Erwartete Länge: {width}. Tatsächliche Länge: {line.Length}");

            if (i == 0 || i == lines.Length - 1)
            {
                if (!IsAllWall(line))
                    throw new Exception($"Zeile {i + 1} ist nicht vollständig mit '#' umrandet.");
            }
            else
            {
                if (line[0] != '#' || line[^1] != '#')
                    throw new Exception($"Zeile {i + 1} hat keine Wände an den Rändern.");
            }

            foreach (char c in line)
            {
                if (c == 'T') targetCount++;
                if (c == 'P') playerCount++;
            }
        }

        if (playerCount != 1)
            throw new Exception($"Es muss genau ein Spieler-Startpunkt (P) existieren. Aktuell gefunden: {playerCount}");
        if (targetCount > 1)
            throw new Exception($"Es darf maximal ein Zielobjekt (T) existieren. Aktuell gefunden: {targetCount}");
    }

    bool IsAllWall(string line)
    {
        foreach (char c in line)
        {
            if (c != '#') return false;
        }
        return true;
    }
}
