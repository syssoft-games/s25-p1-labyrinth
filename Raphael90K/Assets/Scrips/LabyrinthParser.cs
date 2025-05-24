using UnityEngine;

public class LabyrinthParser : MonoBehaviour
{
    public Texture2D labyrinthImage; // BMP-Bild als Texture2D
    public GameObject playerPrefab; // Player Model
    public GameObject lampPrefab; // Lamp Model
    public GameObject goalPrefab; // Goal Model
    public Material transparentMaterial; // Material für transparente Wände
    public Material mirrorMaterial; // Material für Spiegelwände
    public Material wallMaterial; // Material für normale Wände
    public Material floorMaterial;
    public float wallHeight = 1f; // Höhe der Wände
    public float tileSize = 1f; // Größe der Kacheln (Wände/Boden)


    void Start()
    {
        CreateLabyrinthFloor();
        ParseLabyrinth();
    }

    void ParseLabyrinth()
    {
        // Bildgröße
        int width = labyrinthImage.width;
        int height = labyrinthImage.height;
        
        // Durch die Pixel des Bildes iterieren
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // Farbe des aktuellen Pixels
                Color pixelColor = labyrinthImage.GetPixel(x, y);

                if (pixelColor == Color.white) // Weiß = Boden
                {
                }
                else if (pixelColor == Color.blue) // Blau = Spielerposition
                {
                    SpawnPlayer(x, y);
                }
                else if (pixelColor == new Color(1f, 1f, 0)) // Gelb = Lampe
                {
                    CreateLamp(x, y).transform.SetParent(this.transform);
                }
                else if (pixelColor == new Color(1f, 0, 1f)) // Lila = Ziel
                {
                    CreateGoal(x, y);
                }
                else // Wand
                {
                    // Wandmaterial bestimmen
                    string wallMaterialType = GetWallMaterial(pixelColor);
                    CreateWallTile(x, y, wallMaterialType).transform.SetParent(this.transform);
                }
            }
        }
    }

    private GameObject CreateLamp(int x, int y)
    {
        Vector3 position = new Vector3(x * tileSize, 1f, y * tileSize);
        return Instantiate(lampPrefab, position, Quaternion.identity);
    }

    private void CreateGoal(int x, int y)
    {
        Vector3 position = new Vector3(x * tileSize, 1f, y * tileSize);
        Instantiate(goalPrefab, position, Quaternion.identity);
    }

    // Wandmaterial anhand der Farbe bestimmen
    string GetWallMaterial(Color color)
    {
        if (color == Color.red) // Rot = Spiegel-Wand
        {
            return "Mirror";
        }
        else if (color == Color.green) // Grün = Transparte Wand
        {
            return "Transparent";
        }
        else // Schwarz = Wand
        {
            return "Wall";
        }
    }

    void SpawnPlayer(int x, int y)
    {
        Vector3 position = new Vector3(x * tileSize, 1f, y * tileSize);
        Instantiate(playerPrefab, position, Quaternion.identity);
    }

    // Erstelle einen Wand-Tile
    GameObject CreateWallTile(int x, int y, string materialType)
    {
        Vector3 position = new Vector3(x * tileSize, wallHeight / 2, y * tileSize);
        GameObject wallTile = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wallTile.transform.position = position;
        wallTile.transform.localScale = new Vector3(tileSize, wallHeight, tileSize);

        // Wandmaterial zuweisen
        switch (materialType)
        {
            case "Mirror":
                wallTile.GetComponent<Renderer>().material = mirrorMaterial;
                break;
            case "Wall":
                wallTile.GetComponent<Renderer>().material = wallMaterial;
                break;
            case "Transparent":
                wallTile.GetComponent<Renderer>().material = transparentMaterial;
                break;
            default:
                break;
        }

        return wallTile;
    }

    void CreateLabyrinthFloor()
    {
        int width = labyrinthImage.width;
        int height = labyrinthImage.height;

        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
        floor.transform.SetParent(this.transform);
        floor.name = "LabyrinthFloor";

        // Positionieren in der Mitte des Labyrinths
        Vector3 position = new Vector3((width * tileSize) / 2f - tileSize / 2f, 0,
            (height * tileSize) / 2f - tileSize / 2f);
        floor.transform.position = position;

        // Skalieren passend zur Größe des Labyrinths
        float scaleX = (width * tileSize) / 10f;
        float scaleZ = (height * tileSize) / 10f;
        floor.transform.localScale = new Vector3(scaleX, 1, scaleZ);

        // Material zuweisen
        if (floorMaterial != null)
        {
            floor.GetComponent<Renderer>().material = floorMaterial;
        }
    }
}