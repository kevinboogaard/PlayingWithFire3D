using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileSystem : MonoBehaviour {

    public static List<Tiles> _tiles = new List<Tiles>();

    public int[,] gridMap;

    [SerializeField]
    private int map = 1;

    void Start ()
    {
        GameObject parent = new GameObject("Tilemap");
        GameObject parentTiles = new GameObject("Tiles");
        GameObject parentWalls = new GameObject("Walls");
        GameObject parentDestructibles = new GameObject("Destructibles");

        if (map == 0)
        {
            gridMap = GridSystem.gridNormalAlex;
        }
        else
        {
            gridMap = GridSystem.gridNormalKevin;
        }

        parentTiles.transform.parent = parent.transform;
        parentWalls.transform.parent = parent.transform;
        parentDestructibles.transform.parent = parent.transform;

        DataClass data = GetComponent<DataClass>();
        int amountplayer = data.amountPlayers; // How many players there will be.
        int maxplayers = data.maxPlayers;      // Maximum amount of characters in the game.
        int currentplayer = 0;                 // Current count of characters.
        int currentAmountPlayers = 0;          // Current count of players.
        int amountChance = 50;
        
        int currentPlayablePlayer = 0;

        for (int x = 0; x < gridMap.GetLength(0); x++)
        {
            for (int z = 0; z < gridMap.GetLength(1); z++)
            {
                GameObject tile = SpawnTile(new Vector3(x, 0, z));
                tile.transform.parent = parentTiles.transform;

                if (gridMap[x, z] == 1)
                {
                    GameObject wall = Spawn(new Vector3(x, 1, z));
                    Rigidbody rigid = wall.AddComponent<Rigidbody>();
                    rigid.constraints = RigidbodyConstraints.FreezeAll;
                    wall.transform.parent = parentWalls.transform;
                }
                else if (gridMap[x, z] == 2)
                {
                    currentplayer++;
                    
                    if (currentplayer <= maxplayers)
                    {
                        GameObject player = (GameObject)GameObject.CreatePrimitive(PrimitiveType.Capsule);
                        Camera camera;
                        player.AddComponent<Health>();
                        player.AddComponent<Backpack>();
                        player.AddComponent<Movement>().rotationSpeed = 15;
                        player.GetComponent<Movement>().movementSpeed = 5;
                        player.transform.tag = "Player";
                        if (currentplayer <= amountplayer)
                        {
                            currentPlayablePlayer++;
                            player.AddComponent<PlayerBehaviour>();
                            GameObject cCamera = Instantiate(Resources.Load("Camera"), player.transform.position, player.transform.rotation) as GameObject;
                            cCamera.transform.parent = player.transform;
                            camera = cCamera.GetComponent<Camera>();
                        }
                        else
                        {
                            player.AddComponent<AI>();
                            camera = null;
                        }

                        player.transform.position = new Vector3(x, 1, z);
                        Rigidbody rigid = player.AddComponent<Rigidbody>();
                        rigid.constraints = RigidbodyConstraints.FreezeRotation;
 
                        if (camera != null)
                        {
                            camera.rect = new Rect(new Vector2((float) 0.5 * (currentplayer - 1),0), new Vector2(1 - (float) 0.5 * (amountplayer - 1),1));
                        }
                    }
                    
                }
                else if (gridMap[x, z] == 3)
                {
                    if (Random.Range(0, 100) < 30)
                    {
                        GameObject obstacle = Spawn(new Vector3(x, 1, z));
                        obstacle.transform.tag = "Obstacle";
                        obstacle.AddComponent<Destructible>();
                        obstacle.GetComponent<Renderer>().material.color = Color.green;
                        obstacle.transform.parent = parentDestructibles.transform;
                    }
                }
            }
        }
	}

    GameObject SpawnTile(Vector3 _location)
    {
        GameObject Tile = (GameObject)Instantiate(Resources.Load("Tile"),_location,transform.rotation);
        Tile.AddComponent<Tiles>();
        Tile.transform.tag = "Tile";
        _tiles.Add(Tile.GetComponent<Tiles>());
        return Tile;
    }

    GameObject Spawn(Vector3 _location)
    {
        GameObject cube = (GameObject)GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = _location;

        for (int i = 0; i < _tiles.Count; i++)
        {
            if (_tiles[i].transform.position.x == _location.x && _tiles[i].transform.position.z == _location.z)
            {
                _tiles[i].occupied = cube;
            }
        }
        return cube;
    }

    public static Tiles GetTile(Vector3 location)
    {
        for (int i = 0; i < _tiles.Count; i++)
        {
            if (_tiles[i].transform.position.x == location.x && _tiles[i].transform.position.z == location.z)
            {
                return _tiles[i];
            }
        }

        return null;
    }

    public static void ResetAll()
    {
        foreach (Tiles tile in _tiles)
        {
            tile.f = 0;
            tile.g = 0;
            tile.h = 0;
            tile.closed = false;
            tile.open = false;
            tile.parent = null;
        }
    }
}
