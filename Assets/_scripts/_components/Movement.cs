using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
    private Vector3 endpos;
    private bool moving = false;
    public Tiles currentTile;

    public float movementSpeed = 10;

    void Start()
    {
        endpos = transform.position;
    }
    void Update()
    {
        if (currentTile == null || currentTile.transform.position != transform.position)
        {
            currentTile = TileSystem.GetTile(transform.position);
        }

        if (moving && (transform.position == endpos))
        {
            moving = false;
        }

        transform.position = Vector3.MoveTowards(transform.position, endpos, Time.deltaTime * movementSpeed);
    }

    public void MoveX(int dir)
    {
        //transform.Translate(new Vector3(dir, 0, 0) * movementspeed * Time.deltaTime);
        if (!moving && !TileSystem.GetTile((transform.position + new Vector3(dir, 0, 0))).occupied)
        {
            moving = true;
            endpos = transform.position + new Vector3(dir,0,0);
        }
    }

    public void MoveZ(int dir)
    {
        //transform.Translate(new Vector3(0, 0, dir) * movementspeed * Time.deltaTime);
        if (!moving && !TileSystem.GetTile((transform.position + new Vector3(0, 0, dir))).occupied)
        {
            moving = true;
            endpos = transform.position + new Vector3(0,0,dir);
        }
    }
}