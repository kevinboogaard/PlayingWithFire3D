using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Bomb Behaviour of the Bomb.
/// This class is put on every bomb that is spawned. 
/// </summary>
public class Bomb : MonoBehaviour
{
    public static List<Bomb> bombs = new List<Bomb>();
    private List<Tiles> warnedTiles = new List<Tiles>();

    public int firePower = 1;
    public Tiles tilePlaced;

    public float countdown = 2.5f;
    public bool ticking = true;

    private float currentCount;

    private void Start()
    {
        bombs.Add(gameObject.GetComponent<Bomb>());
        tilePlaced = TileSystem.GetTile(transform.position);
        Warn();
    }

    private void Update()
    {
        if (ticking)
        {
            currentCount += Time.deltaTime;

            if (currentCount >= countdown)
            {
                Explode();
            }
        }

    }

    private void Warn()
    {
        warnedTiles.Add(TileSystem.GetTile(transform.position));
        
        for (int x =  -firePower; x <= firePower; x++)
        {
            Tiles tile = TileSystem.GetTile(new Vector3(transform.position.x + x, 0, transform.position.z));

            if (tile != null)
            {
                if (tile.occupied == null || tile.occupied.tag == "Obstacle")
                {
                    warnedTiles.Add(tile);
                }
            }
        }

        for (int z = -firePower; z <= firePower; z++)
        {
            Tiles tile = TileSystem.GetTile(new Vector3(transform.position.x , 0, transform.position.z + z));

            if (tile != null)
            {
                if (tile.occupied == null || tile.occupied.tag == "Obstacle")
                {
                    warnedTiles.Add(tile);
                }
            }
        }

        for (int i = 0; i < warnedTiles.Count; i++)
        {
            if (warnedTiles[i].occupied != null)
            {
                warnedTiles[i].isWarned = true;
                warnedTiles[i].transform.GetComponent<Renderer>().material.color = Color.red;
            }
        }
    }

    public void Explode()
    {
        for (int i = 0; i < warnedTiles.Count; i++)
        {
            if (warnedTiles[i].occupied.tag == "Obstacle")
            {
                warnedTiles[i].occupied.GetComponent<Destructible>().Destroy();
            }

            warnedTiles[i].transform.GetComponent<Renderer>().material.color = Color.white;
        }

        tilePlaced.occupied = null;
        Destroy(gameObject);
    }
}
