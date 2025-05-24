using UnityEngine;
using System.IO;
using TMPro;

public class LevelGenerator : MonoBehaviour
{
    public Level level;
    public GameObject hyphenPrefab;
    public GameObject pipePrefab;
    public GameObject plusPrefab;
    public GameObject slashPrefab;
    public GameObject hashPrefab;
    public GameObject atsignPrefab;
    public GameObject percentPrefab;
    public GameObject ampersandPrefab;
    public GameObject asteriskPrefab;
    public GameObject colonPrefab;
    public GameObject periodPrefab;
    public GameObject apostrophePrefab;
    public GameObject bracketsPrefab;
    public GameObject bracesPrefab;
    public GameObject paranthesesPrefab;
    public GameObject caretPrefab;
    public GameObject anglebracketsPrefab;
    public GameObject equalsPrefab;
    public GameObject underscorePrefab;
    public GameObject tildePrefab;
    public GameObject xPrefab;
    public GameObject oPrefab;
    public GameObject zeroPrefab;
    public GameObject vPrefab;
    public GameObject goalPrefab;
    public GameObject candlePrefab;
    public int candlePercentage = 10;

    public float xGap = 1.2f;
    public float zGap = 2f;

    void Start()
    {
        // FileBrowser.ShowLoadDialog((path) => { GenerateLevel(path); }, null,
        // FileBrowser.PickMode.Files, false);
        string path = "Assets/ASCIIArt/";
        switch (level)
        {
            case Level.House:
                path += "house.txt";
                break;
            case Level.Owl:
                path += "owl.txt";
                break;
            case Level.Maze1:
                path += "maze1.txt";
                break;
            case Level.Maze2:
                path += "maze2.txt";
                break;
            case Level.Sonic:
                path += "sonic.txt";
                break;
            default:
                Debug.LogWarning("Unknown level type: " + level);
                return;
        }
        GenerateLevel(new string[] { path });
    }

    void GenerateLevel(string[] path)
    {
        // Clear existing tiles
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Read the file
        string[] lines = File.ReadAllLines(path[0]);

        // Parse and generate tiles
        float xMax = int.MinValue;
        float zMax = 0;
        for (int y = 0; y < lines.Length; y++)
        {
            string line = lines[y];
            for (int x = 0; x < line.Length; x++)
            {
                float height = 0;
                GameObject prefab = null;
                Quaternion rotation = Quaternion.identity; // Default rotation
                switch (line[x])
                {
                    case '|':
                        rotation = Quaternion.Euler(90, 90, 0);
                        prefab = pipePrefab;
                        break;
                    case '+':
                        prefab = plusPrefab;
                        break;
                    case '/':
                        prefab = slashPrefab;
                        rotation = Quaternion.Euler(-90, 0, -45);
                        break;
                    case '\\':
                        prefab = slashPrefab;
                        rotation = Quaternion.Euler(-90, 0, 45);
                        break;
                    case '#':
                        prefab = hashPrefab;
                        break;
                    case '@':
                        prefab = atsignPrefab;
                        break;
                    case '%':
                        prefab = percentPrefab;
                        break;
                    case '&':
                        prefab = ampersandPrefab;
                        break;
                    case '*':
                        prefab = asteriskPrefab;
                        break;
                    case ':':
                        prefab = colonPrefab;
                        break;
                    case '.':
                        height = 0.5f;
                        prefab = periodPrefab;
                        break;
                    case '\'':
                        prefab = apostrophePrefab;
                        break;
                    case '[':
                        prefab = bracketsPrefab;
                        break;
                    case ']':
                        prefab = bracketsPrefab;
                        break;
                    case '{':
                        prefab = bracesPrefab;
                        break;
                    case '}':
                        prefab = bracesPrefab;
                        break;
                    case '(':
                        prefab = paranthesesPrefab;
                        rotation = Quaternion.Euler(90, 0, 90);
                        break;
                    case ')':
                        prefab = paranthesesPrefab;
                        rotation = Quaternion.Euler(90, 0, -90);
                        break;
                    case '^':
                        prefab = caretPrefab;
                        break;
                    case '<':
                        prefab = anglebracketsPrefab;
                        break;
                    case '>':
                        prefab = anglebracketsPrefab;
                        break;
                    case '=':
                        height = 1f;
                        prefab = equalsPrefab;
                        break;
                    case '-':
                    case '_':
                        rotation = Quaternion.Euler(90, 0, 0);
                        prefab = underscorePrefab;
                        break;
                    case '~':
                        prefab = tildePrefab;
                        break;
                    case 'x':
                        prefab = xPrefab;
                        break;
                    case 'o':
                        rotation = Quaternion.Euler(90, 90, 0);
                        prefab = oPrefab;
                        break;
                    case '0':
                        prefab = zeroPrefab;
                        break;
                    case 'V':
                    case 'v':
                        prefab = vPrefab;
                        break;
                    case ' ':
                        break;
                    default:
                        Debug.LogWarning("Unknown character: " + line[x]);
                        break;
                }
                if (prefab != null)
                {
                    GameObject tile = Instantiate(prefab, new Vector3(xGap * x, height, zGap * y), rotation);
                    tile.transform.parent = transform; // Set the parent to the LevelGenerator object
                    if (Random.Range(0, 100) < candlePercentage)
                    {
                        tile = Instantiate(candlePrefab, new Vector3(xGap * x + 0.5f, 0, zGap * y - .5f), Quaternion.identity);
                        tile.transform.parent = transform; // Set the parent to the LevelGenerator object
                    }
                }
                xMax = xGap * x;
                zMax = zGap * y;
            }
        }
        float goalX = Random.Range(xMax, 0);
        float goalZ = Random.Range(zMax, 0);
        GameObject goal = Instantiate(goalPrefab, new Vector3(goalX, 0, goalZ), Quaternion.identity);
        goal.transform.parent = transform; // Set the parent to the LevelGenerator object
        GameManager.Instance.goal = goal;

        // set random light intensity
        if (Random.Range(0, 100) < 50)
        {
            // night - weak sun
            GameManager.Instance.sun.intensity = Random.Range(1, 10);
        }
        else
        {
            // day - strong sun
            GameManager.Instance.sun.intensity = Random.Range(100, 300);
        }
        GameManager.Instance.sun.intensity = Random.Range(0.5f, 300f);
        GameManager.Instance.StartIntro(goalX, goalZ);
    }

}
public enum Level
{
    House, Owl, Maze1, Maze2, Sonic
}