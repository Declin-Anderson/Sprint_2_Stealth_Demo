using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.AI;

public class MapGeneration : MonoBehaviour
{
    // The parent object which contains all ground elements
    private GameObject Parent;

    // Public array of all possible tile objects for map generation
    // [0 = Entrance Tile, 1 = Path Tile, 2 = Grass Tile, 3 = Fence Tile]
    public GameObject[] Tiles;

    public GameObject Guard;

    // Layout of the tiles in the graveyard
    // First position is x, second is y, and stored variable is tile type
    private int[,] MapLayout;
    private GameObject[,] TileMap;

    // Dimensions for the graveyard (in tiles)
    private int GraveyardWidth = 10;
    private int GraveyardLength = 10;

    // The position of the entrance tile in the layout array, other tiles are built out from this position
    private int[] GraveyardEntrancePos = new int[] { 4, 0 };

    // The scale of the square tile prefabs
    private float TileSize = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        Parent = gameObject.transform.parent.gameObject;

        MapLayout = new int[GraveyardWidth, GraveyardLength];
        for (int i = 0; i < GraveyardWidth; i++)
        {
            for (int j = 0; j < GraveyardLength; j++)
            {
                MapLayout[i, j] = -1;
            }
        }

        // Hard coding map variables for testing purposes
        MapLayout[4, 1] = 1;
        MapLayout[4, 2] = 1;
        MapLayout[4, 3] = 1;
        MapLayout[5, 3] = 1;
        MapLayout[5, 4] = 1;
        MapLayout[5, 5] = 1;
        MapLayout[4, 5] = 1;
        MapLayout[3, 5] = 1;
        MapLayout[3, 4] = 1;
        MapLayout[3, 3] = 1;
        MapLayout[3, 0] = 3;
        MapLayout[1, 0] = 3;
        MapLayout[2, 1] = 3;
        MapLayout[2, 0] = 3;
        MapLayout[GraveyardEntrancePos[0], GraveyardEntrancePos[1]] = -2;

        for (int i = 0; i < GraveyardWidth; i++)
        {
            for (int j = 0; j < GraveyardLength; j++)
            {
                if (MapLayout[i, j] == -1)
                {
                    MapLayout[i, j] = 2;
                }
            }
        }

        GenerateMap(MapLayout);

        NavMeshBuilder.ClearAllNavMeshes();
        NavMeshBuilder.BuildNavMesh();

        // Hard coding in guard path for testing purposes
        Transform[] GuardPath = new Transform[4];
        GuardPath[0] = TileMap[3, 3].transform;
        GuardPath[1] = TileMap[3, 5].transform;
        GuardPath[2] = TileMap[5, 5].transform;
        GuardPath[3] = TileMap[5, 3].transform;

        int[] GuardLookouts = new int[1];
        GuardLookouts[0] = 2;

        GameObject TempGuard = Instantiate(Guard, GuardPath[0].position, Quaternion.identity);
        TempGuard.GetComponent<Guard>().GenerateGuard(GuardPath, GuardLookouts);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Uses the MapLayout array to create a map of the associated tiles, if the tile is a fence, it will gather its adjecent
    // tiles and pass them to the tile's script
    public void GenerateMap(int[,] Layout)
    {
        MapLayout = Layout;
        GraveyardWidth = MapLayout.GetLength(0);
        GraveyardLength = MapLayout.GetLength(1);

        TileMap = new GameObject[GraveyardWidth, GraveyardLength];

        float EntranceX = gameObject.transform.position.x;
        float EntranceY = gameObject.transform.position.y;
        float EntranceZ = gameObject.transform.position.z;

        for (int i = 0; i < GraveyardWidth; i++)
        {
            for (int j = 0; j < GraveyardLength; j++)
            {
                if (MapLayout[i, j] != -2)
                {
                    int XSpaces = i - GraveyardEntrancePos[0];
                    int ZSpaces = j - GraveyardEntrancePos[1];

                    Vector3 NewTilePos = new Vector3(EntranceX + (TileSize * XSpaces), EntranceY, EntranceZ + (TileSize * ZSpaces));

                    TileMap[i, j] = Instantiate(Tiles[MapLayout[i, j]], NewTilePos, Quaternion.identity);
                    TileMap[i, j].transform.parent = Parent.transform;

                    if (MapLayout[i, j] == 3)
                    {
                        int[] Connections = new int[] { -1, -1, -1, -1 };
                        if (j < GraveyardLength - 1)
                        {
                            Connections[0] = MapLayout[i, j + 1];
                        }
                        if (i < GraveyardWidth - 1)
                        {
                            Connections[1] = MapLayout[i + 1, j];
                        }
                        if (j > 0)
                        {
                            Connections[2] = MapLayout[i, j - 1];
                        }
                        if (i > 0)
                        {
                            Connections[3] = MapLayout[i - 1, j];
                        }

                        TileMap[i, j].GetComponent<FenceTile>().GenerateFence(Connections);
                    }
                }
            }
        }
    }
}
