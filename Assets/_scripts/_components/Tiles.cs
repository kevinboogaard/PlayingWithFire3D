using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tiles : MonoBehaviour {
    public GameObject occupied;
    public bool isWarned;

    public List<Tiles> neighbours = new List<Tiles>();

    private int _amountTileSearch = 1;

    public float g;
    public float h;
    public float f;

    public bool closed;
    public bool open;
    public Tiles parent;

    void Start()
    {
        for(int x = -_amountTileSearch; x < _amountTileSearch; x++)
        {
            for (int z = -_amountTileSearch; z < _amountTileSearch; z++)
            {
                Vector3 pos = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);

                if (transform.position != pos)
                {
                    if (TileSystem.GetTile(pos))
                    {
                        neighbours.Add(TileSystem.GetTile(pos));
                    }
                }
            }
        }
    }
}
