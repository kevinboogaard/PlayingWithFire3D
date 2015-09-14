using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

    public Tiles currentTile;

    void Update()
    {
        if (currentTile.transform.position != transform.position || currentTile == null)
        {
            currentTile = TileSystem.GetTile(transform.position);
        }
    }

    public void MoveX(int dir ,float movementspeed)
    {
        transform.Translate(new Vector3(dir, 0, 0) * movementspeed * Time.deltaTime);
    }

    public void MoveZ(int dir ,float movementspeed)
    {
        transform.Translate(new Vector3(0, 0, dir) * movementspeed * Time.deltaTime);
    }
}
