using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileSystem : MonoBehaviour {

    public static List<Tiles> _tiles = new List<Tiles>();

    void Start ()
    {
        GameObject parent = new GameObject("Tilemap");
        GameObject parentTiles = new GameObject("Tiles");
        GameObject parentWalls = new GameObject("Walls");
        GameObject parentDestructibles = new GameObject("Destructibles");


        parentTiles.transform.parent = parent.transform;
        parentWalls.transform.parent = parent.transform;
        parentDestructibles.transform.parent = parent.transform;

        DataClass data = GetComponent<DataClass>();
        int amountplayer = data.amountPlayers; // How many players there will be.
        int maxplayers = data.maxPlayers;      // Maximum amount of characters in the game.
        int currentplayer = 0;                 // Current count of characters.
        int currentAmountPlayers = 0;          // Current count of players.
        int amountChance = 50;
        
        for (int x = 0; x < GridSystem.gridNormalKevin.GetLength(0); x++)
        {
            for (int z = 0; z < GridSystem.gridNormalKevin.GetLength(1); z++)
            {
                GameObject tile = SpawnTile(new Vector3(x, 0, z));
                tile.transform.parent = parentTiles.transform;

                if (GridSystem.gridNormalKevin[x, z] == 1)
                {
                    GameObject wall = Spawn(new Vector3(x, 1, z));
                    Rigidbody rigid = wall.AddComponent<Rigidbody>();
                    rigid.constraints = RigidbodyConstraints.FreezeAll;
                    wall.transform.parent = parentWalls.transform;
                }
                else if (GridSystem.gridNormalKevin[x, z] == 2)
                {

                    currentplayer++;
                    
                    if (currentplayer <= maxplayers)
                    {
                        GameObject player = (GameObject)GameObject.CreatePrimitive(PrimitiveType.Capsule);

                        if (currentplayer <= amountplayer)
                        {
                            player.AddComponent<PlayerBehaviour>();
                        }
                        else
                        {
                            player.AddComponent<AI>();
                        }

                        player.transform.position = new Vector3(x, 1, z);
                        Rigidbody rigid = player.AddComponent<Rigidbody>();
                        rigid.constraints = RigidbodyConstraints.FreezeRotation;
                    }
                    
                    
                }
                else if (GridSystem.gridNormalKevin[x, z] == 3)
                {
                    if (Random.Range(0, 100) < 30)
                    {
                        GameObject obstacle = Spawn(new Vector3(x, 1, z));
                        obstacle.transform.tag = "Obstacle";
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
}
